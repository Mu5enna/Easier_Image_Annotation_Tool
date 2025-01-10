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
using static Image_Annotation_Tool.Calculations;
using System.Drawing;

namespace stajcsharp
{
    public partial class Form1 : Form
    {
        //resim isim ve yollarý
        private Dictionary<string, string> imagePaths = new Dictionary<string, string>();
        //seçimlerin listesi
        private List<SelectionRectangle> rectangles = new List<SelectionRectangle>();
        //seçilen kare idsi ile attributelarýnýn checkedlistboxdaki indexe göre baðlantýsý
        private Dictionary<int, int> selectionAttPairs = new Dictionary<int, int>();
        //attribute isimlerinin classlarla baðlantýsý
        private Dictionary<int, string> attClass = new Dictionary<int, string>();
        //seçilen kare idsinin track id baðlantýsý
        private Dictionary<int, int> trackIds = new Dictionary<int, int>();
        //renkler
        private static List<Pen> pens = new List<Pen>() { Pens.Green, Pens.Yellow, Pens.Purple, Pens.Orange, Pens.Pink, Pens.Brown, Pens.Cyan, Pens.Magenta, Pens.Gray, Pens.Black, Pens.White, Pens.Beige };
        private SelectionRectangle selectedRectangle = null;
        private string returned, resizeHandle = string.Empty, newFolderPath;
        private int returned2;
        private Rectangle selectionRect = Rectangle.Empty;
        private bool isDragging = false, isResizing = false;
        private Point startPoint, currentMousePoint;
        private int index = 0, rightClickIndex;
        private string path1, path2;
        SelectionRectangle? clickedRectangle;
        private bool isFirst = true;
        public Form1()
        {
            InitializeComponent();
            checkedListBox1.SetItemChecked(0, true);
            attClass.Add(0, "None");
        }

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
            if (isFirst==false)
            {
                pictureBox1.Image = null;
                checkedListBox1.Items.Clear();
                rectangles.Clear();
                selectionAttPairs.Clear();
                selectedRectangle = null;
                imagePaths.Clear();
                attClass.Clear();
                trackIds.Clear();
                clickedRectangle = null;
                attClass.Add(0, "None");
                checkedListBox1.Items.Add("None (0)");
            }

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
                    }

                    listBox1.Items.Clear();
                    imagePaths.Clear();

                    string[] imageFiles = Directory.GetFiles(selectedFolder, "*.*", SearchOption.TopDirectoryOnly)
                                                    .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                                   file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                                   file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
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
                            File.WriteAllText(pathName + ".json", "{\n}");
                        }
                    }

                    string attTxt = Path.Combine(newFolderPath, "attributes.txt");
                    if (!File.Exists(attTxt))
                    {
                        File.Create(attTxt).Dispose();
                        File.WriteAllText(attTxt, "attributeName , attributeClass (order matters, do not change anything in this file)");
                    }
                    else
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(attTxt);
                            string line = sr.ReadLine();
                            line = sr.ReadLine();
                            while (line != null)
                            {
                                if (String.IsNullOrWhiteSpace(line))
                                {
                                    line = sr.ReadLine();
                                    continue;
                                }
                            
                                
                                checkedListBox1.Items.Add(line.Split(" , ")[0] + " (" + line.Split(" , ")[1] + ")");
                                attClass.Add(Int32.Parse(line.Split(" , ")[1]), line.Split(" , ")[0]);
                                line = sr.ReadLine();
                            }
                            sr.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hata: " + ex);
                        }
                    }
                    isFirst = false;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                rectangles.Clear();
                selectionAttPairs.Clear();
                trackIds.Clear();


                string selectedFileName = listBox1.SelectedItem.ToString();
                if (imagePaths.TryGetValue(selectedFileName, out string selectedImagePath))
                {
                    pictureBox1.Image = Image.FromFile(selectedImagePath);
                }

                string path = Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(selectedImagePath) + ".json");

                var jsonData = File.ReadAllText(path);

                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(jsonData);

                foreach (var entry in jsonObject)
                {
                    string entryID = entry.Key;
                    List<float> boxCoor = new List<float>() { entry.Value.Box[0], entry.Value.Box[1], entry.Value.Box[2], entry.Value.Box[3] };

                    Rectangle rectInPictureBox = ImageCoordinatesToPictureBox(pictureBox1, new Rectangle(
                        (int)boxCoor[0],
                        (int)boxCoor[1],
                        (int)(boxCoor[2] - boxCoor[0]),
                        (int)(boxCoor[3] - boxCoor[1])
                    ));

                    // Dikdörtgeni rectangles listesine ekle
                    var newRectangle = new SelectionRectangle
                    {
                        Rect = rectInPictureBox,
                        IsSelected = false,
                        Id = int.Parse(entryID),
                    };
                    rectangles.Add(newRectangle);
                    trackIds[newRectangle.Id] = (int)entry.Value.TrackId;
                    selectionAttPairs[newRectangle.Id] = (int)entry.Value.Class;
                }
                numericUpDown1.Value = 0;
                checkedListBox1.SetItemChecked(0, true);
                pictureBox1.Invalidate();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowForm2DialogBox();
            if (returned != "Cancelled")
            {
                if (!attClass.ContainsKey(returned2) && !attClass.ContainsValue(returned))
                {
                    attClass.Add(returned2, returned);
                    checkedListBox1.Items.Add(returned + " (" + returned2 + ")");

                    try
                    {
                        string attTxt = Path.Combine(newFolderPath, "attributes.txt");
                        if (attTxt != null)
                        {
                            File.AppendAllText(attTxt, $"\n{returned} , {returned2}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex);
                    }
                }
                else
                {
                    MessageBox.Show("Bu class deðeri zaten var");
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Týklanan dikdörtgeni bul
                    rectangles = rectangles.OrderBy(r => r.Rect.Width * r.Rect.Height).ToList();
                    clickedRectangle = rectangles.FirstOrDefault(r => r.Rect.Contains(e.Location));
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
                    if (clickedRectangle != null && rectangles.FirstOrDefault(r => r.Rect.Contains(e.Location)) == clickedRectangle)
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
                            Id = rectangles.Any() ? rectangles.Max(r => r.Id) + 1 : 0
                        };

                        rectangles.ForEach(r => r.IsSelected = false); // Tüm seçimleri deselect yap
                        rectangles.Add(newRectangle);
                        rectangles = rectangles.OrderBy(r => r.Rect.Width * r.Rect.Height).ToList();
                        clickedRectangle = newRectangle;
                        clickedRectangle.IsSelected = true;
                        selectedRectangle = clickedRectangle;
                        checkedListBox1.SetItemChecked(0, true);
                        numericUpDown1.Value = 0;
                        selectionAttPairs.Add(newRectangle.Id, 0);
                        trackIds.Add(newRectangle.Id, 0);

                        isDragging = false;
                        isResizing = false;
                    }

                    startPoint = e.Location;
                    (sender as PictureBox).Invalidate();
                }
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
                    // Dikdörtgenin sol üst ve sað alt köþelerini dinamik olarak ayarla
                    int x1 = Math.Min(startPoint.X, e.X);
                    int y1 = Math.Min(startPoint.Y, e.Y);
                    int x2 = Math.Max(startPoint.X, e.X);
                    int y2 = Math.Max(startPoint.Y, e.Y);

                    selectedRectangle.Rect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                }
            }

            (sender as PictureBox).Invalidate();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            isResizing = false;

            if (selectedRectangle != null && selectedRectangle.IsSelected)
            {
                if (selectedRectangle.Rect.Width < 5 && selectedRectangle.Rect.Height < 5)
                {
                    rectangles.Remove(selectedRectangle);
                    trackIds.Remove(selectedRectangle.Id);
                    selectionAttPairs.Remove(selectedRectangle.Id);
                    selectedRectangle = null;
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var rect in rectangles)
            {
                rect.Draw(e.Graphics, trackIds, pens);
            }
        }

        public class SelectionRectangle
        {
            public Rectangle Rect { get; set; }
            public bool IsSelected { get; set; }
            public int Id { get; set; }

            private const int HandleSize = 12;

            public void Draw(Graphics graphics, Dictionary<int, int> trackIds, List<Pen> pens)
            {
                // Track ID'ye göre pen seç
                int trackId = trackIds.ContainsKey(Id) ? trackIds[Id] : -1; // Eðer ID yoksa varsayýlan deðer
                Pen rectPen = trackId > 0 ? pens[trackId % pens.Count] : Pens.Gray;

                using (var pen = new Pen(rectPen.Color, 3)) // Kalem rengini ve kalýnlýðýný ayarla
                {
                    graphics.DrawRectangle(pen, Rect);
                }

                if (IsSelected)
                {
                    foreach (var handle in GetResizeHandles())
                    {
                        graphics.FillRectangle(Brushes.Blue, handle);
                    }
                }

                if (Rect.Width > 0 && Rect.Height > 0)
                {
                    var font = new Font("Arial", 14, FontStyle.Bold);
                    var textBrush = Brushes.LightBlue;
                    graphics.DrawString($"{Id}", font, textBrush, Rect.Location);
                }
            }

            public Rectangle[] GetResizeHandles()
            {
                if (!IsSelected) return Array.Empty<Rectangle>();

                int adjustedHandleSize = (int)(HandleSize * 0.75); // Handle boyutunun %75'ini hesapla
                int offset = (HandleSize - adjustedHandleSize) / 2; // Handle pozisyonunu ayarlamak için offset hesapla

                return new Rectangle[]
                {
                    new Rectangle(Rect.Left + offset, Rect.Top + offset, adjustedHandleSize, adjustedHandleSize), // Sol üst
                    new Rectangle(Rect.Right - adjustedHandleSize - offset, Rect.Top + offset, adjustedHandleSize, adjustedHandleSize), // Sað üst
                    new Rectangle(Rect.Left + offset, Rect.Bottom - adjustedHandleSize - offset, adjustedHandleSize, adjustedHandleSize), // Sol alt
                    new Rectangle(Rect.Right - adjustedHandleSize - offset, Rect.Bottom - adjustedHandleSize - offset, adjustedHandleSize, adjustedHandleSize), // Sað alt
                    new Rectangle(Rect.Left + offset, Rect.Top + Rect.Height / 2 - adjustedHandleSize / 2, adjustedHandleSize, adjustedHandleSize), // Sol
                    new Rectangle(Rect.Right - adjustedHandleSize - offset, Rect.Top + Rect.Height / 2 - adjustedHandleSize / 2, adjustedHandleSize, adjustedHandleSize), // Sað
                    new Rectangle(Rect.Left + Rect.Width / 2 - adjustedHandleSize / 2, Rect.Top + offset, adjustedHandleSize, adjustedHandleSize), // Üst
                    new Rectangle(Rect.Left + Rect.Width / 2 - adjustedHandleSize / 2, Rect.Bottom - adjustedHandleSize - offset, adjustedHandleSize, adjustedHandleSize), // Alt
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
                    int selectionClass = Int32.Parse(checkedListBox1.Items[index].ToString().Replace(")", "").Split(" (")[1]);
                    selectionAttPairs[selectedRectangle.Id] = selectionClass;
                }
                button6_Click(sender, e);
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (pictureBox1.Image is null)
                return;
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
                checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(attClass[selectionAttPairs[selectedRectangle.Id]] + " (" + selectionAttPairs[selectedRectangle.Id] + ")"), true);
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
            if (selectedRectangle != null)
            {
                if (trackIds.ContainsKey(selectedRectangle.Id))
                {
                    trackIds[selectedRectangle.Id] = (int)numericUpDown1.Value;
                }
                else
                {
                    trackIds.Add(selectedRectangle.Id, (int)numericUpDown1.Value);
                }
                button6_Click(sender, e);
            }
            pictureBox1.Invalidate();
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
        }

        private void selectSecondFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string selectedItem = listBox1.Items[rightClickIndex].ToString();

            string originalFilePath = imagePaths[selectedItem];

            path2 = originalFilePath;
        }

        private void fillInbetweenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string prevAdd = "", currAdd, endAdd = "", begAdd = "";
            bool ifFound = false;
            int diff = 0;

            foreach (var img in imagePaths)
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

            foreach (var img in imagePaths)
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
                else if (img.Value == path2)
                {
                    ifFound = false;
                    break;
                }
            }
        }

        private Point ConvertToImageCoordinates(Point pictureBoxPoint)
        {
            if (pictureBox1.Image == null)
                return Point.Empty;

            // PictureBox ve görüntü boyutlarýný al
            var pictureBoxSize = pictureBox1.ClientSize;
            var imageSize = pictureBox1.Image.Size;

            // Ölçek oranýný hesapla
            double ratioWidth = (double)pictureBoxSize.Width / imageSize.Width;
            double ratioHeight = (double)pictureBoxSize.Height / imageSize.Height;
            double scaleRatio = Math.Min(ratioWidth, ratioHeight);

            // Görüntünün görüntülendiði alaný hesapla
            int displayedImageWidth = (int)(imageSize.Width * scaleRatio);
            int displayedImageHeight = (int)(imageSize.Height * scaleRatio);

            int offsetX = (pictureBoxSize.Width - displayedImageWidth) / 2;
            int offsetY = (pictureBoxSize.Height - displayedImageHeight) / 2;

            // PictureBox noktasýný resim koordinatlarýna dönüþtür
            int imageX = (int)((pictureBoxPoint.X - offsetX) / scaleRatio);
            int imageY = (int)((pictureBoxPoint.Y - offsetY) / scaleRatio);

            return new Point(imageX, imageY);
        }

        private List<Point> GetRectangleCornersInImageCoordinates(Rectangle rect)
        {
            var corners = new List<Point>
                {
                    ConvertToImageCoordinates(new Point(rect.Left, rect.Top)), // Sol üst
                    ConvertToImageCoordinates(new Point(rect.Right, rect.Bottom)) // Sað alt
                };

            return corners;
        }


        private void button6_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                imagePaths.TryGetValue(listBox1.SelectedItem.ToString(), out string selectedImagePath);
                string jsonPath = Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(selectedImagePath) + ".json");
                string jsonFile = File.ReadAllText(jsonPath);
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(jsonFile);
                var updJsonObject = new Dictionary<string, JsonData>();

                foreach (var rect in rectangles)
                {

                    List<Point> imageCoordinates = GetRectangleCornersInImageCoordinates(rect.Rect);

                    updJsonObject[rect.Id.ToString()] = new JsonData
                    {
                        Box = new List<float> { imageCoordinates[0].X, imageCoordinates[0].Y, imageCoordinates[1].X, imageCoordinates[1].Y },
                        Class = selectionAttPairs[rect.Id],
                        TrackId = trackIds[rect.Id]
                    };

                }

                jsonObject = updJsonObject;
                string updJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                File.WriteAllText(jsonPath, updJson);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedRectangle != null)
            {
                List<Point> imageCoordinates = GetRectangleCornersInImageCoordinates(selectedRectangle.Rect);
            }
        }

        private Rectangle ImageCoordinatesToPictureBox(PictureBox pictureBox, Rectangle imageRect)
        {
            if (pictureBox.Image == null) return Rectangle.Empty;

            var image = pictureBox.Image;
            var pbSize = pictureBox.ClientSize;

            float imageAspect = (float)image.Width / image.Height;
            float pbAspect = (float)pbSize.Width / pbSize.Height;

            float scale;
            int offsetX = 0, offsetY = 0;

            if (pbAspect > imageAspect)
            {
                // PictureBox yatay olarak geniþ
                scale = (float)pbSize.Height / image.Height;
                offsetX = (int)((pbSize.Width - image.Width * scale) / 2);
            }
            else
            {
                // PictureBox dikey olarak uzun
                scale = (float)pbSize.Width / image.Width;
                offsetY = (int)((pbSize.Height - image.Height * scale) / 2);
            }

            return new Rectangle(
                (int)(imageRect.X * scale + offsetX),
                (int)(imageRect.Y * scale + offsetY),
                (int)(imageRect.Width * scale),
                (int)(imageRect.Height * scale)
            );
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (selectedRectangle != null)
            {
                rectangles.Remove(selectedRectangle);
                selectionAttPairs.Remove(selectedRectangle.Id);
                trackIds.Remove(selectedRectangle.Id);
                pictureBox1.Invalidate();
            }
            button6_Click(sender, e);
        }

        private void checkedListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                rightClickIndex = checkedListBox1.IndexFromPoint(e.Location);

                if (rightClickIndex != CheckedListBox.NoMatches)
                {
                    checkedListBox1.SelectedIndex = rightClickIndex;

                    contextMenuStrip2.Show(checkedListBox1, e.Location);
                }
            }
        }

        private void deleteClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            attClass.Remove(Int32.Parse(checkedListBox1.Items[rightClickIndex].ToString().Replace(")", "").Split(" (")[1]));
            string attTxt = Path.Combine(newFolderPath, "attributes.txt");

            // Satýrlarý filtrele ve tamamen boþ olmayanlarý al
            var filteredLines = File.ReadLines(attTxt)
                .Where(line => !line.StartsWith(checkedListBox1.Items[rightClickIndex].ToString().Split(" (")[0]) && !string.IsNullOrWhiteSpace(line))
                .ToArray();

            // Filtrelenmiþ satýrlarý dosyaya yaz
            File.WriteAllLines(attTxt, filteredLines);

            // CheckedListBox'tan öðeyi kaldýr
            checkedListBox1.Items.RemoveAt(rightClickIndex);
        }
    }
}