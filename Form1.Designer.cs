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
            checkedListBox1 = new CheckedListBox();
            numericUpDown1 = new NumericUpDown();
            label1 = new Label();
            button5 = new Button();
            buttonSave = new Button();
            buttonDelete = new Button();
            contextMenuStrip2 = new ContextMenuStrip(components);
            deleteClassToolStripMenuItem = new ToolStripMenuItem();
            tbTracker = new TextBox();
            label2 = new Label();
            btSepTrack = new Button();
            comboBox1 = new ComboBox();
            linkLabel1 = new LinkLabel();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            contextMenuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 52);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(120, 379);
            listBox1.TabIndex = 0;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.KeyDown += listBox1_KeyDown;
            listBox1.KeyPress += listBox1_KeyPress;
            listBox1.MouseDown += listBox1_MouseDown;
            listBox1.PreviewKeyDown += listBox1_PreviewKeyDown;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
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
            button1.Location = new Point(12, 8);
            button1.Name = "button1";
            button1.Size = new Size(120, 38);
            button1.TabIndex = 1;
            button1.Text = "Add Frame";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(794, 8);
            button2.Name = "button2";
            button2.Size = new Size(120, 29);
            button2.TabIndex = 2;
            button2.Text = "Add Class";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Location = new Point(138, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(650, 427);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // checkedListBox1
            // 
            checkedListBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(new object[] { "None (0)" });
            checkedListBox1.Location = new Point(794, 70);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(120, 310);
            checkedListBox1.TabIndex = 7;
            checkedListBox1.ThreeDCheckBoxes = true;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            checkedListBox1.MouseDown += checkedListBox1_MouseDown;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            numericUpDown1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            numericUpDown1.Location = new Point(794, 402);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(64, 33);
            numericUpDown1.TabIndex = 8;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(794, 384);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 9;
            label1.Text = "Tracking Id";
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button5.Location = new Point(864, 402);
            button5.Name = "button5";
            button5.Size = new Size(50, 33);
            button5.TabIndex = 10;
            button5.Text = "Add";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonSave.Location = new Point(794, 438);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(120, 39);
            buttonSave.TabIndex = 11;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += button6_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDelete.Location = new Point(668, 438);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(120, 39);
            buttonDelete.TabIndex = 12;
            buttonDelete.Text = "Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += button7_Click;
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.ImageScalingSize = new Size(20, 20);
            contextMenuStrip2.Items.AddRange(new ToolStripItem[] { deleteClassToolStripMenuItem });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(138, 26);
            // 
            // deleteClassToolStripMenuItem
            // 
            deleteClassToolStripMenuItem.Name = "deleteClassToolStripMenuItem";
            deleteClassToolStripMenuItem.Size = new Size(137, 22);
            deleteClassToolStripMenuItem.Text = "Delete Class";
            deleteClassToolStripMenuItem.Click += deleteClassToolStripMenuItem_Click;
            // 
            // tbTracker
            // 
            tbTracker.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tbTracker.Location = new Point(12, 454);
            tbTracker.Margin = new Padding(3, 2, 3, 2);
            tbTracker.Name = "tbTracker";
            tbTracker.Size = new Size(120, 23);
            tbTracker.TabIndex = 13;
            tbTracker.KeyPress += tbTracker_KeyPress;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(12, 434);
            label2.Name = "label2";
            label2.Size = new Size(158, 15);
            label2.TabIndex = 14;
            label2.Text = "TrackID for Seperate Tracking";
            // 
            // btSepTrack
            // 
            btSepTrack.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btSepTrack.Location = new Point(138, 454);
            btSepTrack.Margin = new Padding(3, 2, 3, 2);
            btSepTrack.Name = "btSepTrack";
            btSepTrack.Size = new Size(82, 23);
            btSepTrack.TabIndex = 15;
            btSepTrack.Text = "Fill";
            btSepTrack.UseVisualStyleBackColor = true;
            btSepTrack.Click += btSepTrack_Click;
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(794, 42);
            comboBox1.Margin = new Padding(3, 2, 3, 2);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(120, 23);
            comboBox1.Sorted = true;
            comboBox1.TabIndex = 16;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // linkLabel1
            // 
            linkLabel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            linkLabel1.BackColor = Color.Transparent;
            linkLabel1.Location = new Point(12, 462);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(902, 15);
            linkLabel1.TabIndex = 18;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Easy Image Annotator - Info";
            linkLabel1.TextAlign = ContentAlignment.MiddleCenter;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(923, 480);
            Controls.Add(comboBox1);
            Controls.Add(btSepTrack);
            Controls.Add(label2);
            Controls.Add(tbTracker);
            Controls.Add(buttonDelete);
            Controls.Add(buttonSave);
            Controls.Add(button5);
            Controls.Add(label1);
            Controls.Add(numericUpDown1);
            Controls.Add(checkedListBox1);
            Controls.Add(pictureBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Controls.Add(linkLabel1);
            KeyPreview = true;
            Name = "Form1";
            ShowIcon = false;
            Text = "Image Annotator";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            contextMenuStrip2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private Button button1;
        private Button button2;
        private PictureBox pictureBox1;
        private CheckedListBox checkedListBox1;
        private NumericUpDown numericUpDown1;
        private Label label1;
        private Button button5;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem selectFirstFrameToolStripMenuItem;
        private ToolStripMenuItem selectSecondFrameToolStripMenuItem;
        private ToolStripMenuItem fillInbetweenToolStripMenuItem;
        private Button buttonSave;
        private Button buttonDelete;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem deleteClassToolStripMenuItem;
        private TextBox tbTracker;
        private Label label2;
        private Button btSepTrack;
        private ComboBox comboBox1;
        private LinkLabel linkLabel1;
    }
}
