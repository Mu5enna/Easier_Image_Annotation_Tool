namespace stajcsharp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            listBox1 = new ListBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            selectFirstFrameToolStripMenuItem = new ToolStripMenuItem();
            selectSecondFrameToolStripMenuItem = new ToolStripMenuItem();
            fillInbetweenToolStripMenuItem = new ToolStripMenuItem();
            button1 = new Button();
            button2 = new Button();
            pictureBox1 = new PictureBox();
            button3 = new Button();
            button4 = new Button();
            checkedListBox1 = new CheckedListBox();
            numericUpDown1 = new NumericUpDown();
            label1 = new Label();
            button5 = new Button();
            button6 = new Button();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 41);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(120, 394);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.MouseDown += listBox1_MouseDown;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { selectFirstFrameToolStripMenuItem, selectSecondFrameToolStripMenuItem, fillInbetweenToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(181, 70);
            // 
            // selectFirstFrameToolStripMenuItem
            // 
            selectFirstFrameToolStripMenuItem.Name = "selectFirstFrameToolStripMenuItem";
            selectFirstFrameToolStripMenuItem.Size = new Size(180, 22);
            selectFirstFrameToolStripMenuItem.Text = "Select First Frame";
            selectFirstFrameToolStripMenuItem.Click += selectFirstFrameToolStripMenuItem_Click;
            // 
            // selectSecondFrameToolStripMenuItem
            // 
            selectSecondFrameToolStripMenuItem.Name = "selectSecondFrameToolStripMenuItem";
            selectSecondFrameToolStripMenuItem.Size = new Size(180, 22);
            selectSecondFrameToolStripMenuItem.Text = "Select SecondFrame";
            selectSecondFrameToolStripMenuItem.Click += selectSecondFrameToolStripMenuItem_Click;
            // 
            // fillInbetweenToolStripMenuItem
            // 
            fillInbetweenToolStripMenuItem.Name = "fillInbetweenToolStripMenuItem";
            fillInbetweenToolStripMenuItem.Size = new Size(180, 22);
            fillInbetweenToolStripMenuItem.Text = "Fill Inbetween";
            fillInbetweenToolStripMenuItem.Click += fillInbetweenToolStripMenuItem_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(120, 23);
            button1.TabIndex = 1;
            button1.Text = "Add Frame";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(794, 12);
            button2.Name = "button2";
            button2.Size = new Size(120, 23);
            button2.TabIndex = 2;
            button2.Text = "Add Attribute";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Location = new Point(138, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(650, 423);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button3.Location = new Point(668, 441);
            button3.Name = "button3";
            button3.Size = new Size(120, 23);
            button3.TabIndex = 5;
            button3.Text = "Import";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button4.Location = new Point(794, 441);
            button4.Name = "button4";
            button4.Size = new Size(120, 23);
            button4.TabIndex = 6;
            button4.Text = "Export";
            button4.UseVisualStyleBackColor = true;
            // 
            // checkedListBox1
            // 
            checkedListBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(new object[] { "None" });
            checkedListBox1.Location = new Point(794, 41);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(120, 346);
            checkedListBox1.TabIndex = 7;
            checkedListBox1.ThreeDCheckBoxes = true;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            numericUpDown1.Location = new Point(794, 412);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(64, 23);
            numericUpDown1.TabIndex = 8;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(794, 394);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 9;
            label1.Text = "Tracking Id";
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button5.Location = new Point(864, 412);
            button5.Name = "button5";
            button5.Size = new Size(50, 23);
            button5.TabIndex = 10;
            button5.Text = "Add";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button6.Location = new Point(542, 441);
            button6.Name = "button6";
            button6.Size = new Size(120, 23);
            button6.TabIndex = 11;
            button6.Text = "Save";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(923, 474);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(label1);
            Controls.Add(numericUpDown1);
            Controls.Add(checkedListBox1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(pictureBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Name = "Form1";
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Button button1;
        private Button button2;
        private PictureBox pictureBox1;
        private Button button3;
        private Button button4;
        private CheckedListBox checkedListBox1;
        private NumericUpDown numericUpDown1;
        private Label label1;
        private Button button5;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem selectFirstFrameToolStripMenuItem;
        private ToolStripMenuItem selectSecondFrameToolStripMenuItem;
        private ToolStripMenuItem fillInbetweenToolStripMenuItem;
        private Button button6;
    }
}
