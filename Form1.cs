using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using Newtonsoft.Json;
using Image_Annotation_Tool;

namespace stajcsharp
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> imagePaths = new Dictionary<string, string>();
        private Dictionary<int, string> attributeBelong = new Dictionary<int, string>();
        private List<SelectionRectangle> rectangles = new List<SelectionRectangle>();
        private List<SelectionRectangle> rectCopy = new List<SelectionRectangle>();
        private Dictionary<int, int> selectionAttPairs = new Dictionary<int, int>();
        private Dictionary<string, int> attClass = new Dictionary<string, int>();
        private Dictionary<int, int> trackIds = new Dictionary<int, int>();
        private SelectionRectangle selectedRectangle = null;
        private string returned, resizeHandle = string.Empty, newFolderPath;
        private int returned2;
        private Rectangle selectionRect = Rectangle.Empty;
        private bool isDragging = false, isResizing = false;
        private Point startPoint, currentMousePoint;
        public static int rectId = 0;
        private int index = 0, rightClickIndex;
        private string path1, path2;
        SelectionRectangle? clickedRectangle;
        public Form1()
        {
            InitializeComponent();
            checkedListBox1.SetItemChecked(0, true);
        }

        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Multiselect = true,
            Filter = "Resim Dosyalarý|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
            Title = "Resim Dosyalarýný Seç"
        };

        public void ShowForm2DialogBox()
        {
            Form2 testDialog = new Form2();
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                returned = testDialog.textBox1.Text;
                returned2 = Int32.Parse(testDialog.textBox2.Text);
            }
            else
            {
                returned = "Cancelled";
            }
            testDialog.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolder = folderBrowserDialog.SelectedPath;

                    string folderName = Path.GetFileName(selectedFolder);

                    newFolderPath = Path.Combine(Path.GetDirectoryName(selectedFolder), folderName + "_json");

                    if (!Directory.Exists(newFolderPath))
                    {
                        Directory.CreateDirectory(newFolderPath);
                        MessageBox.Show($"Yeni klasör oluþturuldu: {newFolderPath}", "Bilgi");
                    }
                    else
                    {
                        MessageBox.Show($"Klasör zaten mevcut: {newFolderPath}", "Bilgi");
                    }

                    listBox1.Items.Clear();
                    imagePaths.Clear();

                    string[] imageFiles = Directory.GetFiles(selectedFolder, "*.*", SearchOption.TopDirectoryOnly)
                                                    .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                                   file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                                   file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                                   file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                                                   file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                                                    .ToArray();


                    foreach (string filePath in imageFiles)
                    {
                        string fileName = Path.GetFileName(filePath); 
                        imagePaths[fileName] = filePath;
                        listBox1.Items.Add(fileName);
                    }

                    foreach (string filePath in imageFiles)
                    {
                        string pathName = Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(filePath));
                        if (!File.Exists(pathName + ".json"))
                        {
                            File.Create(pathName + ".json").Dispose();
                            File.WriteAllText(pathName+".json","{\n}");
                        }
                    }

                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedFileName = listBox1.SelectedItem.ToString();
                if (imagePaths.TryGetValue(selectedFileName, out string selectedImagePath))
                {
                    pictureBox1.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowForm2DialogBox();
            if (returned != "Cancelled")
            {
                attClass.Add(returned, returned2);
                checkedListBox1.Items.Add(returned);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //if (pictureBox1.Image != null)

            if (e.Button == MouseButtons.Right)
            {
                // Týklanan dikdörtgeni bul
                rectCopy = rectangles;
                rectCopy = rectCopy.OrderBy(r => r.Rect.Width * r.Rect.Height).ToList();
                clickedRectangle = rectCopy.FirstOrDefault(r => r.Rect.Contains(e.Location));
                if (clickedRectangle != null)
                {
                    // Seçili dikdörtgeni güncelle
                    rectangles.ForEach(r => r.IsSelected = false); // Tüm seçimleri deselect yap
                    clickedRectangle.IsSelected = true;
                    selectedRectangle = clickedRectangle;
                    checkAtt();
                    checkTrack();
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                if (clickedRectangle != null && rectCopy.FirstOrDefault(r => r.Rect.Contains(e.Location)) == clickedRectangle)
                {
                    // Mevcut seçimde bir tutma noktasý týklandý mý kontrol et
                    resizeHandle = clickedRectangle.GetResizeHandle(e.Location);

                    if (!string.IsNullOrEmpty(resizeHandle))
                    {
                        isResizing = true;
                    }
                    else
                    {
                        isDragging = true;
                    }
                }
                else
                {
                    // Yeni bir dikdörtgen ekle
                    var newRectangle = new SelectionRectangle
                    {
                        Rect = new Rectangle(e.Location, Size.Empty),
                        IsSelected = true,
                        Id = rectId++
                    };

                    rectangles.ForEach(r => r.IsSelected = false); // Tüm seçimleri deselect yap
                    rectangles.Add(newRectangle);
                    rectCopy.Add(newRectangle);
                    rectCopy = rectCopy.OrderBy(r => r.Rect.Width * r.Rect.Height).ToList();
                    clickedRectangle = newRectangle;
                    clickedRectangle.IsSelected = true;
                    selectedRectangle = clickedRectangle;
                    checkedListBox1.SetItemChecked(0, true);
                    numericUpDown1.Value = 0;


                    isDragging = false;
                    isResizing = false;
                }

                startPoint = e.Location;
                (sender as PictureBox).Invalidate();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedRectangle != null && selectedRectangle.IsSelected)
            {
                if (isDragging)
                {
                    int offsetX = e.X - startPoint.X;
                    int offsetY = e.Y - startPoint.Y;
                    selectedRectangle.Rect = new Rectangle(
                        selectedRectangle.Rect.X + offsetX,
                        selectedRectangle.Rect.Y + offsetY,
                        selectedRectangle.Rect.Width,
                        selectedRectangle.Rect.Height);
                    startPoint = e.Location;
                }
                else if (isResizing)
                {
                    selectedRectangle.ResizeRectangle(resizeHandle, e.Location);
                }
                else if (e.Button == MouseButtons.Left && selectedRectangle.IsSelected)
                {
                    int width = e.X - startPoint.X;
                    int height = e.Y - startPoint.Y;
                    selectedRectangle.Rect = new Rectangle(startPoint.X, startPoint.Y, width, height);
                }
            }

            (sender as PictureBox).Invalidate();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            isResizing = false;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var rect in rectangles)
            {
                rect.Draw(e.Graphics);
            }
        }
        public class SelectionRectangle
        {
            public Rectangle Rect { get; set; }
            public bool IsSelected { get; set; }
            public int Id { get; set; }

            private const int HandleSize = 12;

            public void Draw(Graphics graphics)
            {
                graphics.DrawRectangle(IsSelected ? Pens.Red : Pens.GreenYellow, Rect);

                if (IsSelected)
                {
                    foreach (var handle in GetResizeHandles())
                    {
                        graphics.FillRectangle(Brushes.Blue, handle);
                    }
                }

                if (Rect.Width > 0 && Rect.Height > 0)
                {
                    var font = new Font("Arial", 10, FontStyle.Bold);
                    var textBrush = Brushes.Black;
                    graphics.DrawString($"{Id}", font, textBrush, Rect.Location);
                }
            }

            public Rectangle[] GetResizeHandles()
            {
                if (!IsSelected) return Array.Empty<Rectangle>();

                return new Rectangle[]
                {
                new Rectangle(Rect.Left - HandleSize / 2, Rect.Top - HandleSize / 2, HandleSize, HandleSize), // Sol üst
                new Rectangle(Rect.Right - HandleSize / 2, Rect.Top - HandleSize / 2, HandleSize, HandleSize), // Sað üst
                new Rectangle(Rect.Left - HandleSize / 2, Rect.Bottom - HandleSize / 2, HandleSize, HandleSize), // Sol alt
                new Rectangle(Rect.Right - HandleSize / 2, Rect.Bottom - HandleSize / 2, HandleSize, HandleSize), // Sað alt
                new Rectangle(Rect.Left - HandleSize / 2, Rect.Top + Rect.Height / 2 - HandleSize / 2, HandleSize, HandleSize), // Sol
                new Rectangle(Rect.Right - HandleSize / 2, Rect.Top + Rect.Height / 2 - HandleSize / 2, HandleSize, HandleSize), // Sað
                new Rectangle(Rect.Left + Rect.Width / 2 - HandleSize / 2, Rect.Top - HandleSize / 2, HandleSize, HandleSize), // Üst
                new Rectangle(Rect.Left + Rect.Width / 2 - HandleSize / 2, Rect.Bottom - HandleSize / 2, HandleSize, HandleSize), // Alt
                };
            }

            public string GetResizeHandle(Point mousePoint)
            {
                if (!IsSelected) return string.Empty;

                var handles = GetResizeHandles();
                var handleNames = new[] { "TopLeft", "TopRight", "BottomLeft", "BottomRight", "Left", "Right", "Top", "Bottom" };

                for (int i = 0; i < handles.Length; i++)
                {
                    if (handles[i].Contains(mousePoint))
                    {
                        return handleNames[i];
                    }
                }

                return string.Empty;
            }

            public void ResizeRectangle(string handle, Point mousePoint)
            {
                if (!IsSelected) return;

                switch (handle)
                {
                    case "TopLeft":
                        Rect = new Rectangle(mousePoint.X, mousePoint.Y, Rect.Right - mousePoint.X, Rect.Bottom - mousePoint.Y);
                        break;
                    case "TopRight":
                        Rect = new Rectangle(Rect.Left, mousePoint.Y, mousePoint.X - Rect.Left, Rect.Bottom - mousePoint.Y);
                        break;
                    case "BottomLeft":
                        Rect = new Rectangle(mousePoint.X, Rect.Top, Rect.Right - mousePoint.X, mousePoint.Y - Rect.Top);
                        break;
                    case "BottomRight":
                        Rect = new Rectangle(Rect.Left, Rect.Top, mousePoint.X - Rect.Left, mousePoint.Y - Rect.Top);
                        break;
                    case "Left":
                        Rect = new Rectangle(mousePoint.X, Rect.Top, Rect.Right - mousePoint.X, Rect.Height);
                        break;
                    case "Right":
                        Rect = new Rectangle(Rect.Left, Rect.Top, mousePoint.X - Rect.Left, Rect.Height);
                        break;
                    case "Top":
                        Rect = new Rectangle(Rect.Left, mousePoint.Y, Rect.Width, Rect.Bottom - mousePoint.Y);
                        break;
                    case "Bottom":
                        Rect = new Rectangle(Rect.Left, Rect.Top, Rect.Width, mousePoint.Y - Rect.Top);
                        break;
                }
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox1.ClearSelected();
            if (selectedRectangle is not null)
            {
                if (index == 0)
                {
                    selectionAttPairs.Remove(selectedRectangle.Id);
                }
                else if (index != -1)
                {
                    selectionAttPairs.Remove(selectedRectangle.Id);
                    selectionAttPairs.Add(selectedRectangle.Id, index);
                }
            }

            foreach (var attPair in selectionAttPairs)
            {
                MessageBox.Show((attPair.Key, attPair.Value).ToString());
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (index == -1)
                return;
            else if (index == e.Index)
                e.NewValue = CheckState.Checked;
            else
            {
                var oldIndex = index;
                index = -1;
                checkedListBox1.SetItemChecked(oldIndex, false);
                index = e.Index;
            }
        }

        private void checkAtt()
        {
            if (selectionAttPairs.ContainsKey(selectedRectangle.Id))
            {
                checkedListBox1.SetItemChecked(selectionAttPairs[selectedRectangle.Id], true);
            }
            else
            {
                checkedListBox1.SetItemChecked(0, true);
            }
        }

        private void checkTrack()
        {
            if (trackIds.ContainsKey(selectedRectangle.Id))
            {
                numericUpDown1.Value = trackIds[selectedRectangle.Id];
            }
            else
            {
                numericUpDown1.Value = 0;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            trackIds.Add(selectedRectangle.Id, (int)numericUpDown1.Value);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) 
            {
                rightClickIndex = listBox1.IndexFromPoint(e.Location);

                if (rightClickIndex != ListBox.NoMatches) 
                {
                    listBox1.SelectedIndex = rightClickIndex;

                    contextMenuStrip1.Show(listBox1, e.Location); 
                }
            }
        }

        private void selectFirstFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedItem = listBox1.Items[rightClickIndex].ToString();

            string originalFilePath = imagePaths[selectedItem];

            path1 = originalFilePath;
            MessageBox.Show(path1);
        }

        private void selectSecondFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedItem = listBox1.Items[rightClickIndex].ToString();

            string originalFilePath = imagePaths[selectedItem];

            path2 = originalFilePath;
            MessageBox.Show(path2);
        }

        private void fillInbetweenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string prevAdd = "", currAdd, endAdd = "", begAdd = "";
            bool ifFound = false;
            int diff = 0;

            foreach(var img in imagePaths)
            {
                if (ifFound)
                {
                    diff++;
                }
                if (img.Value == path1)
                {
                    begAdd = img.Value;
                    ifFound = true;
                }
                else if (img.Value == path2)
                {
                    endAdd = img.Value;
                    ifFound = false;
                    break;
                }
            }

            ifFound = false;
            string begFilePath = Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(begAdd) + ".json");
            string endFilePath = Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(endAdd) + ".json");

            foreach(var img in imagePaths)
            {
                currAdd = img.Value;
                if (ifFound)
                {
                    string prevFilePath = Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(prevAdd) + ".json");
                    string currFilePath = Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(currAdd) + ".json");
                    Calculations.calcNewPoints(prevFilePath, currFilePath, Calculations.calcPI(Calculations.CalcDiffs(begFilePath, endFilePath), diff));
                    prevAdd = currAdd;
                }
                if (img.Value == path1)
                {
                    prevAdd = img.Value;
                    ifFound = true;
                }
                else if( img.Value == path2){
                    ifFound = false;
                    break;
                }
            }

            

        }
    }
}