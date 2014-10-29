﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private bool flag = false;
        public bool IsPaused { get; set; }
        public static PictureBox CurrentConditionBox { get; set; }
        public static PictureBox NextConditionBox { get; set; }
        public static TextBox CurrentSpentTime { get; set; }
        public static TextBox NextSpentTime { get; set; }
        public static TextBox CurrentLowerBound { get; set; }
        public static TextBox NextLowerBound { get; set; }
        public static TextBox BestTime { get; set; }
        public Form1()
        {
            InitializeComponent();
            CurrentConditionBox = pictureBox1;
            NextConditionBox = pictureBox2;
            CurrentSpentTime = currentSpentTime;
            NextSpentTime = nextSpentTime;
            CurrentLowerBound = currentLowerBound;
            NextLowerBound = nextLowerBound;
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
                catch (InputException exception)
                {

                    MessageBox.Show("Input data error!");
                    return;
                }
                
                Algorithm.InitializeOutput("output.txt", pictureBox1, pictureBox2);
                DrawingHelper.ConditionQueue.Clear();
                DrawingHelper.IsCurrentConditionQueue.Clear();
                int ans = Algorithm.GetAnswer(Data.FileData);
                IsPaused = true;
                timer1.Enabled = true;
                Algorithm.WriteAnswer(ans);
            }
        }
    
        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawingHelper.Draw();
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
            timer1.Interval = 50;
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
                if(i!=lastLine)
                    lines.Add(richTextBox1.Text.Substring(firstIndexFromLine, firstIndexFromNextLine - firstIndexFromLine-1));
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
            catch (InputException exception)
            {
                MessageBox.Show("Input data error!");
                return;
            }
            Algorithm.InitializeOutput("output.txt", pictureBox1, pictureBox2);
            DrawingHelper.ConditionQueue.Clear();
            DrawingHelper.IsCurrentConditionQueue.Clear();
            int ans = Algorithm.GetAnswer(Data.FileData);
            IsPaused = true;
            timer1.Enabled = true;
            Algorithm.WriteAnswer(ans);
        }

    }
    class Data
    {
        public static Data FileData { get; set; }
        public int NumberOfJobs { get; set; }
        public int NumberOfMachines { get; set; }
        public Job[] Jobs { get; set; }

        public static void InitializeData(string filePath)
        {
            var sr = new StreamReader(filePath);
            try
            {
                var data = new Data();
                var input = sr.ReadLine();
                var nm = input.Split(' ');
                data.NumberOfMachines = int.Parse(nm[0]);
                data.NumberOfJobs = int.Parse(nm[1]);
                data.Jobs = new Job[data.NumberOfJobs];
                for (var i = 0; i < data.NumberOfJobs; i++)
                {
                    var job = sr.ReadLine();
                    data.Jobs[i] = new Job(job,data.NumberOfMachines);
                }
                FileData = data;
                
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
        public Job(string jobString,int nom)
        {

            var operations = jobString.Split(' ');
            NumberOfOperations = int.Parse(operations[0]);
            var jobTime = 0;
            Operations = new Operation[NumberOfOperations];
            for (var i = 1; i <= NumberOfOperations; i++)
            {
                var machine = int.Parse(operations[2 * i - 1]);
                var duration = int.Parse(operations[2 * i]);
                if(machine>nom) 
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
        public static PictureBox NextConditionBox { get; set; }
 
        public static void WriteAnswer(int answer)
        {
            ReleaseStreamWriter.WriteLine(answer);
            ReleaseStreamWriter.Close();
        }

        public static int GetAnswer(Data data)
        {
            if (data.NumberOfJobs == 0) return 0;
            #region init

            var fullTimeOnMachine = new int[data.NumberOfMachines + 1];

            for (var i = 0; i < data.NumberOfJobs; i++)
                for (var j = 0; j < data.Jobs[i].NumberOfOperations; j++)
                        fullTimeOnMachine[data.Jobs[i].Operations[j].Machine] += data.Jobs[i].Operations[j].Duration;
        

            var dj = new int[data.NumberOfJobs];
            for (var i = 0; i < data.NumberOfJobs; i++)
                dj[i] = 0;

            var dm = new int[data.NumberOfMachines + 1];
            for (var i = 1; i <= data.NumberOfMachines; i++)
                dm[i] = 0;

            #endregion

            var queue = new Queue<Condition>();
            var condition = new Condition(dj, dm, 0, Int32.MaxValue, 0);
            
            queue.Enqueue(condition);
            var bestUpperBound = Int32.MaxValue;

            var used = new HashSet<int[]>();

            while (queue.Count != 0)
            {

                var currentCondition = queue.First();
                if (currentCondition.LowerBound > bestUpperBound)
                {
                    queue.Dequeue();
                    continue;
                }
                DrawingHelper.AddCondition(currentCondition, true);
                var cur = currentCondition.DoneInJob;
                used.Add(cur);
                var variants = new List<int>[data.NumberOfMachines + 1]; //jobs for machine at this moment
                for (int i = 0; i < data.NumberOfMachines + 1; i++) variants[i] = new List<int>();
                for (var i = 0; i < data.NumberOfJobs; i++)
                {
                    if (data.Jobs[i].FullTime > cur[i])
                        variants[data.Jobs[i].OperationsArray[cur[i]]].Add(i);
                }

                var jobLists = Dfs(new Stack<int>(), variants, 0);

                #region branching
                foreach (var jobList in jobLists)
                {
                    var cond = new Condition { DoneInJob = (int[])cur.Clone(), DoneOnMachine = (int[])currentCondition.DoneOnMachine.Clone() };
                    foreach (var job in jobList)
                    {
                        cond.DoneInJob[job]++;
                        cond.DoneOnMachine[data.Jobs[job].OperationsArray[cond.DoneInJob[job] - 1]]++;
                    }
                    cond.SpentTime = currentCondition.SpentTime + 1;

                    cond.LowerBound = GetLowerBound(cond, data.Jobs, fullTimeOnMachine);
                    var upperBound = GetUpperBound(cond, data.Jobs);
                    cond.UpperBound = upperBound;

                    if (upperBound < bestUpperBound)
                    {
                        bestUpperBound = upperBound;
                        DrawingHelper.BestTime = bestUpperBound;
                    }

                    if (cond.LowerBound <= bestUpperBound)//prone by record
                    {
                        if (!used.Contains(cond.DoneInJob))//prone by repeating
                        {
                            DrawingHelper.AddCondition(cond, false);
                            queue.Enqueue(cond);
                            used.Add(cond.DoneInJob);
                        }

                    }

                    if (IsAnswer(cond, data.Jobs))
                        return cond.SpentTime;

                }
                #endregion
                queue.Dequeue();
            }
            int t = 0;
            return 1/t;
        }

        private static int GetLowerBound(Condition cond, Job[] jobs, int[] fullTimeOnMachine)
        {
            var ans = cond.DoneInJob.Select((t, i) => jobs[i].FullTime - t).Max();
            ans = fullTimeOnMachine.Select((t, i) => t - cond.DoneOnMachine[i]).Concat(new[] {ans}).Max();
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

        public static void InitializeOutput(string flpth, PictureBox currentConditionBox, PictureBox nextConditionBox)
        {
            ReleaseStreamWriter = new StreamWriter(flpth);
            CurrentConditionBox = currentConditionBox;
            NextConditionBox = nextConditionBox;
        }
    }

    class DrawingHelper
    {
        public static int BestTime { get; set; }
        static readonly string[] ColourValues = new string[] { 
        "#FF0000", "#00FF00", "#0000FF", "#FFFF00", "#FF00FF", "#00FFFF", "#000000", 
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
        public static Queue<Condition> ConditionQueue {
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
        public static void DrawCondition(Condition condition, PictureBox box)
        {
            int maxJobLength = 0;
            for (int i = 0; i < Data.FileData.NumberOfJobs; i++)
                maxJobLength = Math.Max(maxJobLength, Data.FileData.Jobs[i].FullTime);
            float oneX = (float)box.Width/maxJobLength;
            float oneY = (float)box.Height/Data.FileData.NumberOfJobs;
            box.Refresh();
            Graphics g = box.CreateGraphics();
            
            var pen1 = new Pen(Color.Black, 1F);

            for (int i = 0; i < Data.FileData.NumberOfJobs; i++)
            {
                int sum = 0;
                for (int j = 0; j <condition.DoneInJob[i]; j++)
                {
                    var rect = new RectangleF(j * oneX, i * oneY, oneX, oneY);
                    g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml(ColourValues[Data.FileData.Jobs[i].OperationsArray[j]])), rect);
                }
            }

            for (int i = 0; i < Data.FileData.NumberOfJobs; i++)
            {
                int sum = 0;
                for (int j = 0; j < Data.FileData.Jobs[i].Operations.Length; j++)
                {
                    var rect = new RectangleF(sum * oneX, i * oneY, oneX * Data.FileData.Jobs[i].Operations[j].Duration, oneY);
                    g.DrawRectangle(pen1, sum*oneX, i*oneY, oneX*Data.FileData.Jobs[i].Operations[j].Duration, oneY);
                    sum += Data.FileData.Jobs[i].Operations[j].Duration;

                    var stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    Font font1 = new Font("Arial",(int)(oneY/4));

                    g.DrawString(Data.FileData.Jobs[i].Operations[j].Machine.ToString(), font1, Brushes.Black, rect, stringFormat);
                }
            }

        }

        public static void AddCondition(Condition condition, bool isCurrentCondition)
        {
            ConditionQueue.Enqueue(condition);
            IsCurrentConditionQueue.Enqueue(isCurrentCondition);
        }

        public static void Draw()
        {
            if (ConditionQueue.Count != 0)
            {
                Condition condition = ConditionQueue.Dequeue();
                if (IsCurrentConditionQueue.Dequeue())
                {
                    DrawCondition(condition,Form1.CurrentConditionBox);
                    Form1.CurrentLowerBound.Text = condition.LowerBound.ToString();
                    Form1.CurrentSpentTime.Text = condition.SpentTime.ToString();
                }
                else
                {
                    DrawCondition(condition, Form1.NextConditionBox);
                    Form1.NextLowerBound.Text = condition.LowerBound.ToString();
                    Form1.NextSpentTime.Text = condition.SpentTime.ToString();
                    Form1.BestTime.Text = BestTime.ToString();

                }
                
            }
        }

    }

    class InputException : Exception
    {
        
    }
}
