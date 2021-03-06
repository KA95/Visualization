﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private bool flag = false;
        public bool IsPaused { get; set; }
        public static PictureBox CurrentConditionBox { get; set; }
        public static TextBox CurrentSpentTime { get; set; }
        public static TextBox CurrentRecord { get; set; }
        public static PictureBox SearchTreeBox { get; set; }
        public static TextBox CurrentLowerBound { get; set; }
        public static TextBox BestTime { get; set; }
        public Form1()
        {
            InitializeComponent();
            CurrentConditionBox = pictureBox1;
            SearchTreeBox = pictureBox2;
            CurrentSpentTime = currentSpentTime;
            CurrentLowerBound = currentLowerBound;
            CurrentRecord = currentRecordBox;
            BestTime = bestTime;
            IsPaused = true;
        }

        private void ChooseInputBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;

                try
                {
                    Data.InitializeData(file);
                }
                catch (InputException)
                {

                    MessageBox.Show("Input data error!");
                    return;
                }

                Algorithm.InitializeOutput("output.txt", pictureBox1);
                DrawingHelper.ConditionQueue.Clear();
                DrawingHelper.IsCurrentConditionQueue.Clear();
                int ans = Algorithm.GetAnswer();
                IsPaused = true;
                timer1.Enabled = true;
                Algorithm.WriteAnswer(ans);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawingHelper.DrawTree();
            if(!SearchTreeQueue.NextStep()) timer1.Enabled=false;

            if (IsPaused)
                timer1.Enabled = false;
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            timer1.Interval = 10;
            IsPaused = true;
            timer1.Enabled = false;
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            IsPaused = false;
            timer1.Enabled = true;
        }

        private void NextStepBtn_Click(object sender, EventArgs e)
        {
            IsPaused = true;
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(richTextBox1.Text)) return;
            int firstLine = richTextBox1.GetLineFromCharIndex(0);
            int lastLine = richTextBox1.GetLineFromCharIndex(richTextBox1.Text.Length);

            List<string> lines = new List<string>();
            for (int i = firstLine; i <= lastLine; i++)
            {
                int firstIndexFromLine = richTextBox1.GetFirstCharIndexFromLine(i);
                int firstIndexFromNextLine = richTextBox1.GetFirstCharIndexFromLine(i + 1);

                if (firstIndexFromNextLine == -1)
                {
                    // Get character index of last character in this line:
                    Point pt = new Point(richTextBox1.ClientRectangle.Width, richTextBox1.GetPositionFromCharIndex(firstIndexFromLine).Y);
                    firstIndexFromNextLine = richTextBox1.GetCharIndexFromPosition(pt);
                    firstIndexFromNextLine += 1;
                }
                if (i != lastLine)
                    lines.Add(richTextBox1.Text.Substring(firstIndexFromLine, firstIndexFromNextLine - firstIndexFromLine - 1));
                else
                    lines.Add(richTextBox1.Text.Substring(firstIndexFromLine, firstIndexFromNextLine - firstIndexFromLine));
            }
            StreamWriter currentWriter = new StreamWriter("_tmp.txt", false);

            foreach (string line in lines)
            {
                currentWriter.WriteLine(line);
            }
            currentWriter.Close();

            string file = "_tmp.txt";
            try
            {
                Data.InitializeData(file);
            }
            catch (InputException)
            {
                MessageBox.Show("Input data error!");
                return;
            }
            Algorithm.InitializeOutput("output.txt", pictureBox1);
            DrawingHelper.ConditionQueue.Clear();
            DrawingHelper.IsCurrentConditionQueue.Clear();
            int ans = Algorithm.GetAnswer();
            IsPaused = true;
            timer1.Enabled = true;
            Algorithm.WriteAnswer(ans);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            while (SearchTreeQueue.NextStep())
            {
            }
            DrawingHelper.DrawTree();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;
            DrawingHelper.DrawCondition(DrawingHelper.GetConditionByCoordinates(coordinates.X,coordinates.Y));
        }

    }
    static class Data
    {
        public static int NumberOfJobs { get; set; }
        public static int NumberOfMachines { get; set; }
        public static Job[] Jobs { get; set; }

        public static void InitializeData(string filePath)
        {
            var sr = new StreamReader(filePath);
            try
            {
                var input = sr.ReadLine();
                var nm = input.Split(' ');
                NumberOfMachines = int.Parse(nm[0]);
                NumberOfJobs = int.Parse(nm[1]);
                Jobs = new Job[NumberOfJobs];
                for (var i = 0; i < NumberOfJobs; i++)
                {
                    var job = sr.ReadLine();
                    Jobs[i] = new Job(job, Data.NumberOfMachines);
                }

            }
            catch (Exception)
            {
                throw new InputException();
            }
            finally
            {
                sr.Close();
            }
        }
    }
    class Job
    {
        public Job(string jobString, int nom)
        {

            var operations = jobString.Split(' ');
            NumberOfOperations = int.Parse(operations[0]);
            var jobTime = 0;
            Operations = new Operation[NumberOfOperations];
            for (var i = 1; i <= NumberOfOperations; i++)
            {
                var machine = int.Parse(operations[2 * i - 1]);
                var duration = int.Parse(operations[2 * i]);
                if (machine > nom)
                    throw new InputException();
                jobTime += duration;
                Operations[i - 1] = new Operation(machine, duration);
            }
            FullTime = jobTime;
            OperationsArray = new int[jobTime];
            var t = 0;
            for (var i = 0; i < NumberOfOperations; i++)
            {
                var currentOperatrion = Operations[i];
                for (var j = 0; j < currentOperatrion.Duration; j++)
                {
                    OperationsArray[t] = currentOperatrion.Machine;
                    t++;
                }
            }

        }

        public int NumberOfOperations { get; set; }
        public Operation[] Operations { get; set; }
        public int[] OperationsArray { get; set; } //what machine needs job
        public int[] TimeBeforeChangindMachine { get; set; }
        public int FullTime { get; set; }
    }
    class Operation
    {
        public Operation(int machine, int duration)
        {
            Duration = duration;
            Machine = machine;

        }
        public int Duration { get; set; }
        public int Machine { get; set; }
    }
    public class Condition : IComparable<Condition>, IComparer<Condition>
    {
        public Condition(int[] dj, int[] dm, int lb, int ub, int t)
        {
            DoneInJob = dj;
            DoneOnMachine = dm;
            LowerBound = lb;
            UpperBound = ub;
            SpentTime = t;
        }

        public Condition()
        {
        }

        public int[] DoneInJob { get; set; }
        public int[] DoneOnMachine { get; set; }
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
        public int SpentTime { get; set; }

        public int CompareTo(Condition other)
        {
            return SpentTime.CompareTo(other.SpentTime);
        }
        public int Compare(Condition x, Condition y)
        {
            return x.CompareTo(y);
        }
    }
    class Algorithm
    {
        public static StreamWriter ReleaseStreamWriter { get; set; }
        public static PictureBox CurrentConditionBox { get; set; }
        public static int Answer { get; set; }

        public static void WriteAnswer(int answer)
        {
            ReleaseStreamWriter.WriteLine(answer);
            ReleaseStreamWriter.Close();
        }

        public static int GetAnswer()
        {
            if (Data.NumberOfJobs == 0) return 0;

            #region init

            SearchTree.Initialize();
            SearchTreeQueue.Initialize();
            var fullTimeOnMachine = new int[Data.NumberOfMachines + 1];

            for (var i = 0; i < Data.NumberOfJobs; i++)
                for (var j = 0; j < Data.Jobs[i].NumberOfOperations; j++)
                    fullTimeOnMachine[Data.Jobs[i].Operations[j].Machine] += Data.Jobs[i].Operations[j].Duration;


            var dj = new int[Data.NumberOfJobs];
            for (var i = 0; i < Data.NumberOfJobs; i++)
                dj[i] = 0;

            var dm = new int[Data.NumberOfMachines + 1];
            for (var i = 1; i <= Data.NumberOfMachines; i++)
                dm[i] = 0;

            #endregion

            var queue = new Queue<Condition>();
            var condition = new Condition(dj, dm, 0, Int32.MaxValue, 0);
            condition.LowerBound = GetLowerBound(condition, Data.Jobs, fullTimeOnMachine);
            condition.UpperBound = GetUpperBound(condition, Data.Jobs);

            ///////////////
            SearchTree.AddCondition(-1, condition);
            SearchTree.SetConditionNumber(condition, 0);
            SearchTree.SetType(0, (int)SearchTree.typeEnum.Unseen);
            ////////////////////
            queue.Enqueue(condition);
            var bestUpperBound = Int32.MaxValue;

            var used = new HashSet<int[]>();

            while (queue.Count != 0)
            {

                var currentCondition = queue.First();

                int parent = SearchTree.GetConditionNumber(currentCondition);

                if (currentCondition.LowerBound > bestUpperBound)
                {
                    queue.Dequeue();
                    continue;
                }


                SearchTreeQueue.Push((int)SearchTree.typeEnum.Seen, currentCondition);
                /////////////////
                //SearchTree.SetType(parent, (int)SearchTree.typeEnum.Seen);
                //////////////////
                var cur = currentCondition.DoneInJob;
                used.Add(cur);
                var variants = new List<int>[Data.NumberOfMachines + 1]; //jobs for machine at this moment
                for (int i = 0; i < Data.NumberOfMachines + 1; i++) variants[i] = new List<int>();
                for (var i = 0; i < Data.NumberOfJobs; i++)
                {
                    if (Data.Jobs[i].FullTime > cur[i])
                        variants[Data.Jobs[i].OperationsArray[cur[i]]].Add(i);
                }

                var jobLists = Dfs(new Stack<int>(), variants, 0);

                #region branching

                foreach (var jobList in jobLists)
                {
                    var cond = new Condition
                    {
                        DoneInJob = (int[])cur.Clone(),
                        DoneOnMachine = (int[])currentCondition.DoneOnMachine.Clone()
                    };
                    foreach (var job in jobList)
                    {
                        cond.DoneInJob[job]++;
                        cond.DoneOnMachine[Data.Jobs[job].OperationsArray[cond.DoneInJob[job] - 1]]++;
                    }
                    cond.SpentTime = currentCondition.SpentTime + 1;

                    cond.LowerBound = GetLowerBound(cond, Data.Jobs, fullTimeOnMachine);
                    var upperBound = GetUpperBound(cond, Data.Jobs);
                    cond.UpperBound = upperBound;

                    if (upperBound < bestUpperBound)
                    {
                        bestUpperBound = upperBound;
                        DrawingHelper.BestTime = bestUpperBound;
                    }

                    if (!used.Contains(cond.DoneInJob))
                    {
                        if (cond.LowerBound <= bestUpperBound)
                        {

                            SearchTreeQueue.Push((int)SearchTree.typeEnum.Unseen, cond, parent);

                            ///////////////////////
                            //SearchTree.AddCondition(parent, cond);
                            //SearchTree.SetType(SdearchTree.GetConditionNumber(cond), (int)SearchTree.typeEnum.Unseen);
                            ///////////////////////

                            queue.Enqueue(cond);
                            used.Add(cond.DoneInJob);
                        }
                        else
                        {
                            ////////////////////////
                            //SearchTree.AddCondition(parent, cond);
                            //SearchTree.SetType(SearchTree.GetConditionNumber(cond), (int)SearchTree.typeEnum.Proned);
                            ////////////////////////
                            queue.Enqueue(cond);
                            used.Add(cond.DoneInJob);
                            SearchTreeQueue.Push((int)SearchTree.typeEnum.Proned, cond,parent);

                            
                        }

                    }

                    if (IsAnswer(cond, Data.Jobs))
                    {
                        Answer = cond.SpentTime;
                        return cond.SpentTime;
                    }

                }
                #endregion
                queue.Dequeue();
            }

            int t = 0;
            return 1 / t;
        }

        private static int GetLowerBound(Condition cond, Job[] jobs, int[] fullTimeOnMachine)
        {
            var ans = cond.DoneInJob.Select((t, i) => jobs[i].FullTime - t).Max();
            ans = fullTimeOnMachine.Select((t, i) => t - cond.DoneOnMachine[i]).Concat(new[] { ans }).Max();
            return ans + cond.SpentTime;
        }


        private static bool IsAnswer(Condition cond, Job[] jobs)
        {
            var ans = true;
            for (var i = 0; i < jobs.Count(); i++)
            {
                if (cond.DoneInJob[i] != jobs[i].FullTime)
                    ans = false;

            }

            return ans;
        }

        private static int GetUpperBound(Condition cond, Job[] jobs)
        {
            var numberOfMachines = cond.DoneOnMachine.Count();

            var usedMachine = new bool[numberOfMachines + 1];
            for (var i = 1; i <= numberOfMachines; i++)
                usedMachine[i] = false;
            var left = jobs.Select((t, i) => t.FullTime - cond.DoneInJob[i]).Sum();
            var ans = 0;
            var doneInJob = (int[])cond.DoneInJob.Clone();
            while (left != 0)
            {

                for (var i = 1; i <= numberOfMachines; i++)
                    usedMachine[i] = false;
                for (var i = 0; i < jobs.Length; i++)
                {
                    if (doneInJob[i] == jobs[i].FullTime)
                        continue;

                    var currentMachine = jobs[i].OperationsArray[doneInJob[i]];

                    if (!usedMachine[currentMachine])
                    {
                        usedMachine[currentMachine] = true;
                        doneInJob[i]++;
                        left--;
                    }
                }
                ans++;

            }
            return ans + cond.SpentTime;
        }

        private static List<List<int>> Dfs(Stack<int> current, List<int>[] variants, int depth)
        {
            var ans = new List<List<int>>();
            if (depth == variants.Length)
            {
                ans.Add(current.ToList());
                return ans;
            }

            if (variants[depth].Count == 0)
                return Dfs(current, variants, depth + 1);
            foreach (var item in variants[depth])
            {
                current.Push(item);
                ans.AddRange(Dfs(current, variants, depth + 1));
                current.Pop();
            }
            return ans;
        }

        public static void InitializeOutput(string flpth, PictureBox currentConditionBox)
        {
            ReleaseStreamWriter = new StreamWriter(flpth);
            CurrentConditionBox = currentConditionBox;
        }
    }
    class DrawingHelper
    {
        public static int BestTime { get; set; }
        static readonly string[] ColourValues = new string[] { 
        "#FFFF00", "#FF0000", "#0000FF", "#00FF00", "#FF00FF", "#00FFFF", "#000000", 
        "#800000", "#008000", "#000080", "#808000", "#800080", "#008080", "#808080", 
        "#C00000", "#00C000", "#0000C0", "#C0C000", "#C000C0", "#00C0C0", "#C0C0C0", 
        "#400000", "#004000", "#000040", "#404000", "#400040", "#004040", "#404040", 
        "#200000", "#002000", "#000020", "#202000", "#200020", "#002020", "#202020", 
        "#600000", "#006000", "#000060", "#606000", "#600060", "#006060", "#606060", 
        "#A00000", "#00A000", "#0000A0", "#A0A000", "#A000A0", "#00A0A0", "#A0A0A0", 
        "#E00000", "#00E000", "#0000E0", "#E0E000", "#E000E0", "#00E0E0", "#E0E0E0", 
        };
        private static Queue<Condition> _conditionQueue = null;
        private static Queue<bool> _isCurrentConditionQueue = null;
        public static Queue<Condition> ConditionQueue
        {
            get { return _conditionQueue ?? (_conditionQueue = new Queue<Condition>()); }
            set
            {
                _conditionQueue = value;
            }
        }
        public static Queue<bool> IsCurrentConditionQueue
        {
            get { return _isCurrentConditionQueue ?? (_isCurrentConditionQueue = new Queue<bool>()); }
            set
            {
                _isCurrentConditionQueue = value;
            }
        }
        public static bool IsPaused = true;
        public static void DrawCondition(Condition condition)
        {
            PictureBox box = Form1.CurrentConditionBox;


            if (condition == null)
            {
                box.Refresh();
                return;
            }

            Form1.BestTime.Text = Algorithm.Answer.ToString();
            Form1.CurrentRecord.Text = condition.UpperBound.ToString();
            Form1.CurrentLowerBound.Text = condition.LowerBound.ToString();
            Form1.CurrentSpentTime.Text = condition.SpentTime.ToString();

            
          

            int maxJobLength = 0;
            for (int i = 0; i < Data.NumberOfJobs; i++)
                maxJobLength = Math.Max(maxJobLength, Data.Jobs[i].FullTime);
            float oneX = (float)box.Width / maxJobLength;
            float oneY = (float)box.Height / Data.NumberOfJobs;
            box.Refresh();
            Graphics g = box.CreateGraphics();

            var pen1 = new Pen(Color.Black, 1F);

            for (int i = 0; i < Data.NumberOfJobs; i++)
            {

                for (int j = 0; j < condition.DoneInJob[i]; j++)
                {
                    var rect = new RectangleF(j * oneX, i * oneY, oneX, oneY);
                    g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml(ColourValues[Data.Jobs[i].OperationsArray[j]])), rect);
                }
            }

            for (int i = 0; i < Data.NumberOfJobs; i++)
            {
                int sum = 0;
                for (int j = 0; j < Data.Jobs[i].Operations.Length; j++)
                {
                    var rect = new RectangleF(sum * oneX, i * oneY, oneX * Data.Jobs[i].Operations[j].Duration, oneY);
                    g.DrawRectangle(pen1, sum * oneX, i * oneY, oneX * Data.Jobs[i].Operations[j].Duration, oneY);
                    sum += Data.Jobs[i].Operations[j].Duration;

                    var stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    Font font1 = new Font("Arial", (int)(oneY / 4));

                    g.DrawString(Data.Jobs[i].Operations[j].Machine.ToString(), font1, Brushes.Black, rect, stringFormat);
                }
            }

        }

        private static List<PointF> ConditionPoints { get; set; }

        public static void DrawTree()
        {
            PictureBox box = Form1.SearchTreeBox;

            int[] sumLayers = new int[SearchTree.Layers.Count];
            float[] width = new float[SearchTree.Layers.Count];
            sumLayers[0] = 1;

            for (int i = 1; i < SearchTree.Layers.Count; i++)
                sumLayers[i] = sumLayers[i - 1] + SearchTree.Layers[i];

            box.Refresh();
            Graphics g = box.CreateGraphics();
            float oneY = (float)box.Height / SearchTree.Layers.Count;

            var pen1 = new Pen(Color.Black, 1F);
            for (int i = 0; i < SearchTree.Layers.Count; i++)
            {
                float oneX = (float)box.Width / SearchTree.Layers[i];
                width[i] = oneX;
            }


            ConditionPoints = new List<PointF>();
            ConditionPoints.Add( new PointF(width[0] / 2, oneY / 2));

            for (int i = 1;  i<SearchTree.Conditions.Count;i++)
            {
                int current = i;
                int parent = SearchTree.Parent[i];
                float cX1, cX2, cY1, cY2;
                int c1, c2, r1, r2;
                c1 = c2 = r1 = r2 = 0;

                for (int j = 0; j < SearchTree.Layers.Count; j++)
                {
                    if (current < sumLayers[j])
                    {
                        r1 = j;
                        c1 = current - sumLayers[r1 - 1];
                        break;
                    }
                }
                if (parent != 0)
                    for (int j = 0; j < SearchTree.Layers.Count; j++)
                    {
                        if (parent < sumLayers[j])
                        {
                            r2 = j;
                            c2 = parent - sumLayers[r2 - 1];
                            break;
                        }
                    }

                cX1 = c1 * width[r1] + width[r1] / 2;
                cX2 = c2 * width[r2] + width[r2] / 2;
                cY1 = r1 * oneY + oneY / 2;
                cY2 = r2 * oneY + oneY / 2;
                ConditionPoints.Add(new PointF(cX1,cY1));
                g.DrawLine(pen1, cX1, cY1, cX2, cY2);
            }

            int sum = 0;
            for (int i = 0; i < SearchTree.Layers.Count; i++)
            {
                float oneX = width[i];
                for (int j = 0; j < SearchTree.Layers[i]; j++)
                {
                    float cX = (j * oneX) + oneX / 2;
                    float cY = (i * oneY) + oneY / 2;
                    g.FillEllipse(new SolidBrush(ColorTranslator.FromHtml(ColourValues[SearchTree.Types[sum + j]])), cX - 4, cY - 4, 8, 8);
                    g.DrawEllipse(pen1, cX - 4, cY - 4, 8, 8);
                }
                sum += SearchTree.Layers[i];
            }

        }
        public static Condition GetConditionByCoordinates(int x, int y)
        {
            float fx = x;
            float fy = y;

            if (SearchTree.Conditions == null) return null;

            for (int i = 0; i < SearchTree.Conditions.Count;i++)
            {
                if((fx-ConditionPoints[i].X)*(fx-ConditionPoints[i].X)+(fy-ConditionPoints[i].Y)*(fy-ConditionPoints[i].Y)<=8*8+1)
                    return SearchTree.Conditions[i];
            }

            return null;
        }
    }
    class InputException : Exception
    {

    }
    static class SearchTree
    {

        public static List<Condition> Conditions { get; set; }
        public static List<int> Parent { get; set; }

        private static List<int> conditionLayer;

        public static List<int> Layers { get; set; }

        public static List<int> Types { get; set; }

        private static List<List<int>> children;

        private static Dictionary<Condition, int> numberOfCondition;
        public enum typeEnum { Unseen, Seen, Proned };

        private static int depth;
        private static int root;
        public static int Count { get; set; }

        public static void Initialize()
        {
            Types = new List<int> { 0 };
            conditionLayer = new List<int> { 0 };
            root = 0;
            Conditions = new List<Condition>();
            Parent = new List<int>();
            Layers = new List<int> { 1 };
            children = new List<List<int>>();
            numberOfCondition = new Dictionary<Condition, int>();
            depth = 0;
            Count = 0;
        }

        public static void AddCondition(int parent1, Condition condition)
        {
            Parent.Add(parent1);
            int n = Conditions.Count;

            if (parent1 != -1)
            {
                while (children.Count <= parent1)
                    children.Add(new List<int>());

                children[parent1].Add(n);

                while (Layers.Count <= conditionLayer[parent1] + 1)
                    Layers.Add(0);

                conditionLayer.Add(conditionLayer[parent1] + 1);
                Layers[conditionLayer[parent1] + 1]++;
            }

            Conditions.Add(condition);
            depth = Math.Max(depth, condition.SpentTime + 1);
            Count++;
        }

        public static void SetType(int v, int t)
        {
            while (Types.Count <= v)
                Types.Add(0);
            Types[v] = t;

        }

        public static int GetConditionNumber(Condition condition)
        {
            return numberOfCondition[condition];
        }
        public static void SetConditionNumber(Condition condition,int n)
        {
            numberOfCondition.Add(condition, n);
        }
    }
    static class SearchTreeQueue
    {
        public enum typeEnum { Proned, Seen, Unseen };
        public static Queue<Condition> Conditions { get; set; }
        public static Queue<int> ConditionType { get; set; }
        public static Queue<int> Parent { get; set; }

        private static int counter;
        public static void Initialize()
        {
            counter = 1;
            Conditions = new Queue<Condition>();
            ConditionType = new Queue<int>();
            Parent = new Queue<int>();
        }

        public static void Push(int type, Condition condition, int parent = -1)
        {
            if (type != (int) SearchTree.typeEnum.Seen)
            {
                SearchTree.SetConditionNumber(condition,counter);
                counter++;
            }
            Conditions.Enqueue(condition);
            ConditionType.Enqueue(type);
            Parent.Enqueue(parent);
        }

        public static bool NextStep()
        {
            if(Conditions.Count==0) return false;

            Condition condition = Conditions.Dequeue();
            int t = ConditionType.Dequeue();
            int p = Parent.Dequeue();


            if (t == (int)SearchTree.typeEnum.Seen)
            {
                int n = SearchTree.GetConditionNumber(condition);
                SearchTree.SetType(n, (int)SearchTree.typeEnum.Seen);
            }
            else if (t == (int)SearchTree.typeEnum.Proned)
            {
                SearchTree.AddCondition(p, condition);
                int n = SearchTree.GetConditionNumber(condition);
                SearchTree.SetType(n, (int)SearchTree.typeEnum.Proned);
            }
            else if (t == (int) SearchTree.typeEnum.Unseen)
            {
                SearchTree.AddCondition(p, condition);
                int n = SearchTree.GetConditionNumber(condition);
                SearchTree.SetType(n, (int)SearchTree.typeEnum.Unseen);
            }
            return true;
        }

    }
}
