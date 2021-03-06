﻿namespace WindowsFormsApplication2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PauseBtn = new System.Windows.Forms.Button();
            this.PlayBtn = new System.Windows.Forms.Button();
            this.NextStepBtn = new System.Windows.Forms.Button();
            this.ChooseInputBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bestTime = new System.Windows.Forms.TextBox();
            this.currentLowerBound = new System.Windows.Forms.TextBox();
            this.currentSpentTime = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.currentRecordBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(947, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(244, 227);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // PauseBtn
            // 
            this.PauseBtn.Location = new System.Drawing.Point(12, 548);
            this.PauseBtn.Name = "PauseBtn";
            this.PauseBtn.Size = new System.Drawing.Size(125, 22);
            this.PauseBtn.TabIndex = 4;
            this.PauseBtn.Text = "Pause";
            this.PauseBtn.UseVisualStyleBackColor = true;
            this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
            // 
            // PlayBtn
            // 
            this.PlayBtn.Location = new System.Drawing.Point(143, 548);
            this.PlayBtn.Name = "PlayBtn";
            this.PlayBtn.Size = new System.Drawing.Size(125, 22);
            this.PlayBtn.TabIndex = 5;
            this.PlayBtn.Text = "Play";
            this.PlayBtn.UseVisualStyleBackColor = true;
            this.PlayBtn.Click += new System.EventHandler(this.PlayBtn_Click);
            // 
            // NextStepBtn
            // 
            this.NextStepBtn.Location = new System.Drawing.Point(274, 548);
            this.NextStepBtn.Name = "NextStepBtn";
            this.NextStepBtn.Size = new System.Drawing.Size(125, 22);
            this.NextStepBtn.TabIndex = 6;
            this.NextStepBtn.Text = "Next step";
            this.NextStepBtn.UseVisualStyleBackColor = true;
            this.NextStepBtn.Click += new System.EventHandler(this.NextStepBtn_Click);
            // 
            // ChooseInputBtn
            // 
            this.ChooseInputBtn.Location = new System.Drawing.Point(405, 548);
            this.ChooseInputBtn.Name = "ChooseInputBtn";
            this.ChooseInputBtn.Size = new System.Drawing.Size(125, 22);
            this.ChooseInputBtn.TabIndex = 7;
            this.ChooseInputBtn.Text = "Choose input";
            this.ChooseInputBtn.UseVisualStyleBackColor = true;
            this.ChooseInputBtn.Click += new System.EventHandler(this.ChooseInputBtn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Text files |*.txt";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(943, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "Current condition";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(836, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 24);
            this.label4.TabIndex = 11;
            this.label4.Text = "Answer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(819, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "Lower bound";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(842, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 24);
            this.label6.TabIndex = 14;
            this.label6.Text = "Spent time";
            // 
            // bestTime
            // 
            this.bestTime.Location = new System.Drawing.Point(827, 43);
            this.bestTime.Name = "bestTime";
            this.bestTime.Size = new System.Drawing.Size(100, 20);
            this.bestTime.TabIndex = 16;
            // 
            // currentLowerBound
            // 
            this.currentLowerBound.Location = new System.Drawing.Point(841, 191);
            this.currentLowerBound.Name = "currentLowerBound";
            this.currentLowerBound.Size = new System.Drawing.Size(100, 20);
            this.currentLowerBound.TabIndex = 17;
            // 
            // currentSpentTime
            // 
            this.currentSpentTime.Location = new System.Drawing.Point(841, 239);
            this.currentSpentTime.Name = "currentSpentTime";
            this.currentSpentTime.Size = new System.Drawing.Size(100, 20);
            this.currentSpentTime.TabIndex = 18;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(947, 305);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(226, 223);
            this.richTextBox1.TabIndex = 21;
            this.richTextBox1.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(1078, 278);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 24);
            this.label8.TabIndex = 22;
            this.label8.Text = "New input";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(976, 534);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 39);
            this.button1.TabIndex = 23;
            this.button1.Text = "Get new data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(6, 7);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(808, 535);
            this.pictureBox2.TabIndex = 24;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(536, 548);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 22);
            this.button2.TabIndex = 25;
            this.button2.Text = "End it";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(869, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 24);
            this.label2.TabIndex = 26;
            this.label2.Text = "Record";
            // 
            // currentRecordBox
            // 
            this.currentRecordBox.Location = new System.Drawing.Point(841, 128);
            this.currentRecordBox.Name = "currentRecordBox";
            this.currentRecordBox.Size = new System.Drawing.Size(100, 20);
            this.currentRecordBox.TabIndex = 27;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 580);
            this.Controls.Add(this.currentRecordBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.currentSpentTime);
            this.Controls.Add(this.currentLowerBound);
            this.Controls.Add(this.bestTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChooseInputBtn);
            this.Controls.Add(this.NextStepBtn);
            this.Controls.Add(this.PlayBtn);
            this.Controls.Add(this.PauseBtn);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button PauseBtn;
        private System.Windows.Forms.Button PlayBtn;
        private System.Windows.Forms.Button NextStepBtn;
        private System.Windows.Forms.Button ChooseInputBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox bestTime;
        private System.Windows.Forms.TextBox currentLowerBound;
        private System.Windows.Forms.TextBox currentSpentTime;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox currentRecordBox;
    }
}

