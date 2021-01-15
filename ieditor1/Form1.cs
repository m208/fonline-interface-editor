using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using FOIE;

namespace ieditor1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            Editor.ReadParam(); // get registry saved settings

            bool configRead = Editor.GetJson();
            Editor.getHints();

            if (configRead)
            {
                generateStripMenu();
                //openDefault();
            } else
            {
                MessageBox.Show("Error reading config file");
                openSettingsForm();
                //this.Close();

            }

        }

        private void generateStripMenu()
        {
            toolStripScreen.DropDownItems.Clear();
            foreach (string line in Editor.configJsonKeys)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = line;
                item.Name = "upMenu" + line;
                item.Click += new EventHandler(ScreenMenu_Click);
                toolStripScreen.DropDownItems.Add(item);
            }
        }

        private void ScreenMenu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string name = item.Name.Substring("upMenu".Length);
            drawDefault(name);
            
        }

        //private void openDefault()
        //{
        //    Editor.fullPath = "F:\\FOnline2S3en\\data\\art\\intrface\\";
        //    Editor.iniRead(Editor.fullPath + "default.ini");
        //    drawDefault("Inventory");
        //    Editor.iniPath = Editor.fullPath + "default.ini";
        //    this.Text = Editor.fullPath;
        //}


        //System.Media.SystemSounds.Beep.Play();


        //  --------------  ADD MAIN PIC    --------------

        private Bitmap LoadBitmapUnlocked(string file_name)
        {
            using (Bitmap bm = new Bitmap(file_name))
            {
                return new Bitmap(bm);
            }
        }

        private void addMainPic(string name, string bgImage)
        {
            string areaValue = "0 0 0 0";
            string picName = "";
            
            Bitmap img;

            Size pSize = new Size(0, 0);
            Size cSize = new Size(0, 0);

            //-----------------------------------------------------------------------------------

            if (Editor.iniArray.ContainsKey(bgImage))
            {
                picName = Editor.iniArray[bgImage];
            }

            string path = (Editor.fullPath + picName);

            bool imgExist = true;
            try
            {
                img = LoadBitmapUnlocked(@path);
                pSize = new Size(img.Width, img.Height);
            }
            catch
            {
                img = new Bitmap(FOIE.Properties.Resources.nofile1);
                imgExist = false;
            }

            //-----------------------------------------------------------------------------------

            if (Editor.iniArray.ContainsKey(name))
            {
                areaValue = Editor.iniArray[name];
                
                int[] coords = Editor.stringToRectArray(areaValue);
                if (isValidRect(coords))
                {
                    cSize = new Size(coords[2] - coords[0], coords[3] - coords[1]);
                }
            }

            if (name == "0")
            //if (name == "" || name == null)
            {
                if (imgExist)   cSize = pSize;
                else            cSize = new Size(0, 0);
            } 

            var picture = new PictureBox
            {
                Name = name,          //"imgMain",
                Size = cSize,
                Location = new Point(0, 0),
                Image = img,
                BackgroundImageLayout = ImageLayout.None,
            };

            picture.LocationChanged += new System.EventHandler(this.pictureBox1_LocationChanged);
            picture.SizeChanged += new System.EventHandler(this.pictureBox1_LocationChanged);

            string areaSize = Editor.getSizeFromStringCoords(areaValue);
            string imgSize = pSize.Width + "x" + pSize.Height;
            
            addTxtControlsLine(name, name, false, false, areaValue, areaSize, false, "Area", true);

            addTxtControlsLine(name, bgImage, false, false, picName, imgSize, true, "Picture", imgExist);

            picture.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            panel1.Controls.Add(picture);

        }

        //  --------------  ADD CONTROL NO PICTURE  --------------

        private void addGameControl(string name)
        {
            string value = "0 0 0 0";
            if (Editor.iniArray.ContainsKey(name))
            {
                value = Editor.iniArray[name];
            }

            int[] coords = Editor.stringToRectArray(value);

            Size cSize = new Size(0, 0);
            if (isValidRect(coords))
            {
                cSize = new Size(coords[2] - coords[0], coords[3] - coords[1]);
            }

            Bitmap img = new Bitmap(FOIE.Properties.Resources.hatch);

            var picture = new PictureBox
            {
                Name = name,
                Location = new Point(coords[0], coords[1]),
                Size = cSize,
                BackgroundImage = img,
                BackgroundImageLayout = ImageLayout.Tile,
                BackColor = Color.Transparent,
            };


            string imgMain = Editor.currentBackground;
            PictureBox bgImg = panel1.Controls.Find(imgMain, true).FirstOrDefault() as PictureBox;
            
            new ToolTip().SetToolTip(picture, name);

            bgImg.Controls.Add(picture);
            PictureBox picBox = this.Controls.Find(name, true).FirstOrDefault() as PictureBox;
            picBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            picBox.LocationChanged += new System.EventHandler(this.pictureBox1_LocationChanged);
            picBox.SizeChanged += new System.EventHandler(this.pictureBox1_LocationChanged);
            // picBox.MouseDown += (sender, e) => picBox.BringToFront();
           
            picBox.MouseDown += (sender, e) => picBoxClickHighlight(picBox, e);
            picBox.MouseUp += (sender, e) => picBoxClickHighlightOff(picBox, e);

            ControlMoverOrResizer.Init(picBox);
 
            string areaSize = Editor.getSizeFromStringCoords(value);

            addTxtControlsLine(null, name, true, true, value, areaSize, false, "Area", true);
        }


        //  --------------    ADD only text field   --------------

        private void addCustomField(string name)
        {
            string value = "";
            if (Editor.iniArray.ContainsKey(name))
            {
                value = Editor.iniArray[name];
            }
                addTxtControlsLine(null, name, false, false, value, "", false, "Custom", true);

        }

        //  --------------    ADD addBttnControl    --------------
        private void addBttnControl(string[] line)
        {
            string areaValue = "0 0 0 0";
            if (Editor.iniArray.ContainsKey(line[0]))
            {
                areaValue = Editor.iniArray[line[0]];
            }

            Bitmap img;

            int[] coords = Editor.stringToRectArray(areaValue);

            Size cSize = new Size(0, 0);
            if (isValidRect(coords))
            {
                cSize = new Size(coords[2] - coords[0], coords[3] - coords[1]);
            }

            string name = line[0];

            var picture = new PictureBox
            {
                Name = name,
                Location = new Point(coords[0], coords[1]),
                Size = cSize,
                BackColor = Color.Transparent,
            };

            string imgMain = Editor.currentBackground;
            PictureBox bgImg = panel1.Controls.Find(imgMain, true).FirstOrDefault() as PictureBox;

            new ToolTip().SetToolTip(picture, name);

            bgImg.Controls.Add(picture);
            PictureBox picBox = this.Controls.Find(name, true).FirstOrDefault() as PictureBox;
            picBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            picBox.MouseDown += (sender, e) => picBoxClickHighlight(picBox, e);
            picBox.MouseUp += (sender, e) => picBoxClickHighlightOff(picBox, e);

            picBox.LocationChanged += new System.EventHandler(this.pictureBox1_LocationChanged);
            picBox.SizeChanged += new System.EventHandler(this.pictureBox1_LocationChanged);


            ControlMoverOrResizer.Init(picBox);

            
            string imgSize = cSize.Width + "x" + cSize.Height;


            addTxtControlsLine(name, name, true, true, areaValue, imgSize, false, "Area", true);

            //---------------------------------------------------------------------
            for (int i = 1; i < line.Length; i++)
            {
                string picSize = "";
                bool picExist = true;
                string value = (Editor.iniArray.ContainsKey(line[i])) ? Editor.iniArray[line[i]] : "";


                string path = Editor.fullPath + value;
                try
                {
                    //img = new Bitmap(@path);
                    img = LoadBitmapUnlocked(@path);
                    cSize = new Size(img.Width, img.Height);
                    picSize = cSize.Width + "x" + cSize.Height;
                    
                }
                catch
                {
                    img = new Bitmap(FOIE.Properties.Resources.nofile1);
                    picSize = "error";
                    picExist = false;
                }
                addTxtControlsLine(line[0], line[i], false, false, value, picSize, true, "Picture", picExist);
            }
            
        }
        //------------------------------------------------------------------------------------------------

        private void panelClickHighlight(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name.Substring("lbl".Length);
            PictureBox p = panel1.Controls.Find(name, true).FirstOrDefault() as PictureBox;
            p.BringToFront();
            p.BackColor = Color.Violet;

            Button zb = this.Controls.Find("zBttn"+name, true).FirstOrDefault() as Button;
            zb.Tag = "Up";

            //changeZLayer(zb, e);

        }
        private void panelClickHighlightOff(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name.Substring(3);
            PictureBox p = panel1.Controls.Find(name, true).FirstOrDefault() as PictureBox;
            p.BackColor = Color.Transparent;
        }

        //------------------------------------------------------------------------------------------------


        private void picBoxClickHighlight(object sender, EventArgs e)
        {
            //((Control)sender).BringToFront();
            string name = ((Control)sender).Name;
            Panel p = panel2.Controls.Find("panel" + name, true).FirstOrDefault() as Panel;
            p.BackColor = Color.Violet;
        }
        private void picBoxClickHighlightOff(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name;
            Panel p = panel2.Controls.Find("panel" + name, true).FirstOrDefault() as Panel;

            tableRowTag tableRowTag = (tableRowTag)((Panel)p).Tag;
            Color prevColor = (Color)(tableRowTag.Get("BgColor"));
            p.BackColor = prevColor;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(new Pen(Color.Red, 2), 0, 0, ((Control)sender).Width, ((Control)sender).Height);
            g.DrawString(((Control)sender).Name, new Font("Tahoma", 8), Brushes.White, 0, 0);
        }

        private void pictureBox1_LocationChanged(object sender, EventArgs e)
        {
            string itemName = (string)((Control)sender).Name;
            int[] coords =
             {
                ((Control)sender).Location.X,
                ((Control)sender).Location.Y,
                ((Control)sender).Width + ((Control)sender).Location.X,
                ((Control)sender).Height + ((Control)sender).Location.Y
            };
            string newValue = string.Join(" ", coords);

            var cName = "tb" + ((Control)sender).Name;
            TextBox tb = this.Controls.Find(cName, true).FirstOrDefault() as TextBox;
            tb.Text = newValue;


            //UpdateElementSize();
            var sName = "size" + ((Control)sender).Name;
            TextBox stb = this.Controls.Find(sName, true).FirstOrDefault() as TextBox;
            stb.Text = ((Control)sender).Width + "x" + ((Control)sender).Height;

            //Update Array data
            Editor.iniArray[itemName] = newValue;
        }


        //---------------------------------------------------------------------------------------------------------

        private void addTxtControlsLine(string parentName, string name, bool clickAble, bool hideAble, string tb1Text, string tb2Text, bool openAble, string controlType, bool controlSuccess)
        {
            tableRowTag tableRowTag = new tableRowTag();
            tableRowTag.Set("Group", parentName);
            

            var pnl = new Panel
            {
                Name = "panel" + name,
                Size = new Size(450, 25),
                Location = new Point(0, 25 * Editor.lineCounter),
                Tag = tableRowTag,
            };

            panel2.Controls.Add(pnl);
            

            var llbl = new LinkLabel
                {
                    Name = "lbl" + name,
                    Size = new Size(150, 25),
                    Location = new Point(5, 0),
                    Text = name,
                    //LinkColor = Color.Black,
                };
           
                var lbl = new Label
                {
                    Name = "lbl" + name,
                    Size = new Size(150, 25),
                    Location = new Point(5, 0),
                    Text = name,
                };

            // tool tips from dictionary
            string hint = "";
            if (Editor.hintBook.ContainsKey(name))
            {
                hint = Editor.hintBook[name];
            }
            new ToolTip().SetToolTip(llbl, hint);
            new ToolTip().SetToolTip(lbl, hint);


            string imgSrc = (controlSuccess) ? Editor.controlTypesResources[controlType] : Editor.controlTypesResources["error"];
            object img = FOIE.Properties.Resources.ResourceManager.GetObject(imgSrc);

            var pb = new PictureBox
                {
                    Name = "typePic" + name,
                    Size = new Size(20, 20),
                    Location = new Point(175, 0),
                    Image = (Image) img,
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Tag = controlType,
            };

            var tb1 = new TextBox
            {
                Name = "tb" + name,
                Size = new Size(100, 25),
                Location = new Point(200, 0),
                Text = tb1Text,
                Tag = controlType,
            };
            tb1.TextChanged += (sender, e) => { updateTxtBox(tb1, e); };

            var tb2 = new TextBox
            {
                Name = "size" + name,
                Size = new Size(60, 25),
                Location = new Point(305, 0),
                Text = tb2Text,
                Enabled = false,
            };

            new ToolTip().SetToolTip(pb, controlType);

            
            Panel p = panel2.Controls.Find("panel"+ name, true).FirstOrDefault() as Panel;

            p.Controls.Add(tb1);
            p.Controls.Add(tb2);
            p.Controls.Add(pb);

            if (hideAble)
            {

                var hideBttn = new Button
                {
                    Name = "hide" + name,
                    Size = new Size(20, 20),
                    Location = new Point(150, 0),
                    BackgroundImage = (Image)(FOIE.Properties.Resources.eye),
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Tag = "Show",
                };
                hideBttn.Click += (sender, e) => { showHideArea(hideBttn, e); };
                new ToolTip().SetToolTip(hideBttn, "Show / hide element");
                p.Controls.Add(hideBttn);

                var zBttn = new Button
                {
                    Name = "zBttn" + name,
                    Size = new Size(20, 20),
                    Location = new Point(125, 0),
                    BackgroundImage = (Image)(FOIE.Properties.Resources.resize),
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Tag = "Up",
                };
                zBttn.Click += (sender, e) => { changeZLayer(zBttn, e); };
                new ToolTip().SetToolTip(zBttn, "Bring to front/ send to back");
                p.Controls.Add(zBttn);

            }


            if (controlType == "Picture")
            {
                var CheckBox = new CheckBox
                {
                    Name = "cb" + name,
                    Size = new Size(20, 20),
                    Location = new Point(155, 0),
                    Checked = false,
                    AutoCheck = false,
                    Tag = parentName,
                };

                CheckBox.Click += (sender, e) => { showHideImg(CheckBox, e); };
                p.Controls.Add(CheckBox);
            }


            if (openAble)
            {
                var openBttn = new Button
                {
                    Name = "open" + name,
                    Size = new Size(20, 20),
                    Location = new Point(370, 0),
                    BackgroundImage = (Image)(FOIE.Properties.Resources.folder1),
                    BackgroundImageLayout = ImageLayout.Zoom,
                };

                openBttn.Click += (sender, e) => { changeImg(openBttn, e); };
                p.Controls.Add(openBttn);
            }



            if (clickAble)
            {
                p.Controls.Add(llbl);
                LinkLabel l = p.Controls.Find("lbl" + name, true).FirstOrDefault() as LinkLabel;
                l.MouseDown += (sender, e) => panelClickHighlight(l, e);
                l.MouseUp += (sender, e) => panelClickHighlightOff(l, e);
            }
            else p.Controls.Add(lbl);
            
            Editor.lineCounter++;
        }


//--------------------------------------------------------------------------------------------------------------

        private void changeZLayer(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name.Substring("zBttn".Length); // crop "zBttn"
            PictureBox p = panel1.Controls.Find(name, true).FirstOrDefault() as PictureBox;

            if (((Control)sender).Tag.Equals("Up"))
            {
                ((Control)sender).Tag = "Down";
                p.SendToBack();
            }
            else if (((Control)sender).Tag.Equals("Down"))
            {
                ((Control)sender).Tag = "Up";
                p.BringToFront();
            }

        }

        

        private void showHideArea(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name.Substring("hide".Length); // crop "hide"
            PictureBox p = panel1.Controls.Find(name, true).FirstOrDefault() as PictureBox;

            if (((Control)sender).Tag.Equals("Show"))
            {
                ((Control)sender).BackgroundImage = (Image)(FOIE.Properties.Resources.hide3);
                ((Control)sender).Tag = "Hide";
                p.Visible = false;
            }
            else if (((Control)sender).Tag.Equals("Hide"))
            {
                ((Control)sender).BackgroundImage = (Image)(FOIE.Properties.Resources.eye);
                ((Control)sender).Tag = "Show";
                p.Visible = true;
               // p.BringToFront();
            }

        }

        //-------------------------------------------------------------------------------------

        private void showHideImg(object sender, EventArgs e)
        {
            CheckBox cb = (Control)sender as CheckBox;
            if(cb.Checked) return;

            // else
            
            var allCBoxes = GetAll(this, typeof(CheckBox));

            string currentGroup = (string)cb.Tag;
            string currentItem = cb.Name;

            foreach (CheckBox c in allCBoxes)
            {
                if (c.Tag.Equals(currentGroup))
                {
                    c.Checked = false;
                }
            }
                cb.Checked = true;
                setImage(currentGroup, currentItem.Substring("cb".Length)); // substring = crop "cb"
        }

        //------------------------------------------------------

        private void changeImg(object sender, EventArgs e)
        {
            string newPic = getFileName("Images|*.png; *.jpg; *frm", true);
            if (newPic != null)
            {
                string picName = newPic.Substring(Editor.fullPath.Length);
                string currentItem = (((Control)sender).Name).Substring("open".Length);

                TextBox tb = this.Controls.Find("tb" + currentItem, true).FirstOrDefault() as TextBox;
                tb.Text = picName;

                CheckBox cb = this.Controls.Find("cb" + currentItem, true).FirstOrDefault() as CheckBox;


                if (currentItem == Editor.currentBackground)
                {

                    PictureBox pb = panel1.Controls.Find(currentItem, true).FirstOrDefault() as PictureBox;
                    
                    try
                    {
                        pb.Image = new Bitmap(@newPic);
                    }
                    catch
                    {
                        pb.Image = null;
                    }


                }
                else
                {

                    readImageStats(currentItem);

                    if (cb.Checked)
                    {
                        string currentGroup = (string)cb.Tag;
                        setImage(currentGroup, currentItem);
                    }
                }
                
            }
        }

       //------------------------------------------------------------------------------------------------------------------

        private void saveIniToFile()
        {
            saveFileDialog1.InitialDirectory = Editor.fullPath;
            saveFileDialog1.Filter = "Ini file|*.ini";
            string saveTo;

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                saveTo = null;
            }
            else
            {
                while (!saveFileDialog1.FileName.Contains(Editor.fullPath))
                {
                    MessageBox.Show("Please select file which is in the default folder", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    saveFileDialog1.ShowDialog();

                }

                saveTo = saveFileDialog1.FileName;
            }

            if (saveTo != null)
            {
                fileSaver(saveTo);
            }
        }


        private void fileSaver(string filePath)
        {
            Dictionary<string, string> unassignedKeys = new Dictionary<string, string>(Editor.iniArray);    // duplicate ini array

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filePath))
            {
                foreach (string line in Editor.configJsonKeys)
                {
                    file.WriteLine("#");
                    file.WriteLine("#" + line);
                    file.WriteLine("#");
                    file.WriteLine("");

                    string[] fields = { "Main", "Controls", "Custom" };

                    foreach (string key in fields)
                    {

                        var Controls = Editor.configJSON[line][key] as JArray;

                        foreach (var i in Controls)
                        {
                            if (i.GetType().Name == "JArray")
                            {
                                string[] array = (string[])i.ToObject(typeof(string[]));
                                foreach (string n in array)
                                {
                                    if (Editor.iniArray.ContainsKey(n))
                                    {
                                        file.WriteLine(n + " = " + Editor.iniArray[n]);
                                        unassignedKeys.Remove(n);
                                    }
                                 }
                            }
                            else
                            {
                                string name = (string)i.ToObject(typeof(string));
                                if (Editor.iniArray.ContainsKey(name))
                                {
                                    file.WriteLine(name + "=" + Editor.iniArray[name]);
                                    unassignedKeys.Remove(name);
                                }
                            }
                        }
                    }

                    file.WriteLine("");
                }

                if (unassignedKeys.Count > 0)
                {
                    file.WriteLine("#");
                    file.WriteLine("#Unassigned Keys");
                    file.WriteLine("#");
                    file.WriteLine("");

                    foreach (KeyValuePair<string, string> entry in unassignedKeys)
                    {
                        file.WriteLine(entry.Key + "=" + entry.Value);
                    }
                }

            }
        }


        //------------------------------------------------------------------------------------------------------------------

        private void openProject()
        {

            string newIni = getFileName("Ini file|*.ini", false);
            if (newIni != null)
            {
                string directoryName = Path.GetDirectoryName(newIni);
                Editor.fullPath = directoryName + "\\";
                Editor.iniPath = newIni;

                Editor.iniRead(newIni);
                drawDefault(Editor.configJsonKeys[0]);

                this.Text = newIni;

                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
            }
        }



        private string getFileName(string Filter, bool sameFolderRequired)
        {
            openFileDialog1.InitialDirectory = Editor.fullPath;
            openFileDialog1.Filter = Filter; 

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return null;
            }
            else
            {
                if (sameFolderRequired) { 
                    while (!openFileDialog1.FileName.Contains(openFileDialog1.InitialDirectory))
                    {
                        MessageBox.Show("Please select file which is in the default folder", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        openFileDialog1.ShowDialog();
                    }
                }
                return openFileDialog1.FileName;
            }
        }

        //------------------------------------------------------------------------------------------------------------------

        private void drawDefault(string item)
        {
            clearGui();
            this.panel2.Visible = false;


            if (Editor.configJSON[item]["Main"] != null)
            {
                var bG = Editor.configJSON[item]["Main"] as JArray;

                if(bG.Count > 0)
                {
                    string mainImg = (string)bG[0].ToObject(typeof(string));
                    string mainBg = (string)bG[1].ToObject(typeof(string));

                    Editor.currentBackground = mainImg;

                    addMainPic(mainImg, mainBg);
                }
            }



            if (Editor.configJSON[item]["Controls"] != null)
            {
                var Controls = Editor.configJSON[item]["Controls"] as JArray;

                foreach (var i in Controls)
                {
                    if (i.GetType().Name == "JArray")
                    {
                        string[] array = (string[])i.ToObject(typeof(string[]));
                            addBttnControl(array);
                    }
                    else
                    {
                        string name = (string)i.ToObject(typeof(string));
                            addGameControl(name);
                    }
                }
            }
           

            var CustomFields = Editor.configJSON[item]["Custom"] as JArray;
                foreach (var i in CustomFields)
                {
                    string name = (string)i.ToObject(typeof(string));
                        addCustomField(name);
                }

                colorizeRows();
                setPicsforControls();
                this.panel2.Visible = true;

        }

        //-----------------------------------------------------------------------

        private void colorizeRows()
        {
            bool switchColor = true;
            string lastGroup = "";

            Color odd = Color.MintCream;
            Color even = Color.LightGray;

            foreach (Control i in panel2.Controls)
            {
                if (i is Panel)
                {
                    tableRowTag tableRowTag = (tableRowTag)(((Panel)i).Tag);
                    string currentGroup = (string)(tableRowTag.Get("Group"));       //reassign
     

                    if (currentGroup != lastGroup || currentGroup == null)  {
                        switchColor = !switchColor;
                    }
                    lastGroup = currentGroup;

                    Color currntColor = (switchColor) ? odd : even;
                    ((Panel)i).BackColor = currntColor;

                    tableRowTag newTag = new tableRowTag();
                    newTag.Set("BgColor", currntColor);
                    newTag.Set("Group", currentGroup);           //reassign
                    
                    ((Panel)i).Tag = newTag;
                }
            }

        }

        //--------------------------------------------------------------------------
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }
    
        //-----------------------------------------------------------------------

        private void setPicsforControls()
        {
            var allCBoxes = GetAll(this, typeof(CheckBox));
            string currentItem;
            string lastGroup = "";
            foreach (CheckBox c in allCBoxes) 
            {
                string currentGroup = (string)c.Tag;

                if (currentGroup != lastGroup)
                {
                    c.Checked = true;
                    lastGroup = currentGroup;
                    
                    currentItem = c.Name.Substring("cb".Length); //crop "cb"
                    setImage(currentGroup, currentItem);
                }
            }
        }

        private void setImage(string areaName, string itemName)
        {
            PictureBox pb = panel1.Controls.Find(areaName, true).FirstOrDefault() as PictureBox;
            TextBox tb = this.Controls.Find("tb" + itemName, true).FirstOrDefault() as TextBox;

            string path = Editor.fullPath + tb.Text;

            try
            {
                //Bitmap img = new Bitmap(@path);
                Bitmap img = LoadBitmapUnlocked(@path);
                pb.Image = img;
            }
            catch
            {
                pb.Image = null;
            }

            
        }
        //------------------------------------------------------------------------
        private void setImageUpdateStatus(string itemName, bool success)
        {
            PictureBox pb = this.Controls.Find("typePic" + itemName, true).FirstOrDefault() as PictureBox;
            string controlType = (string)(pb.Tag);

            string imgSrc = (success) ? Editor.controlTypesResources[controlType] : Editor.controlTypesResources["error"];
            object img = FOIE.Properties.Resources.ResourceManager.GetObject(imgSrc);

            pb.Image = (Image)img;
        }
        //------------------------------------------------------------------------
        private void readImageStats(string itemName)
        {
            TextBox tb = this.Controls.Find("tb" + itemName, true).FirstOrDefault() as TextBox;
            string path = Editor.fullPath + tb.Text;

            PictureBox pb = this.Controls.Find("typePic" + itemName, true).FirstOrDefault() as PictureBox;
            string controlType = (string)(pb.Tag);

            TextBox stb = this.Controls.Find("size" + itemName, true).FirstOrDefault() as TextBox;
            string newSize = "error";
            string imgSrc = Editor.controlTypesResources["error"];

            try
            {
                //Bitmap img = new Bitmap(@path);
                Bitmap img = LoadBitmapUnlocked(@path);
                newSize = img.Width + "x" + img.Height;
                imgSrc = Editor.controlTypesResources[controlType];
                //Update Array data
                Editor.iniArray[itemName] = tb.Text;
            }
            catch
            {              
            }

            object imgOk = FOIE.Properties.Resources.ResourceManager.GetObject(imgSrc);
            pb.Image = (Image)imgOk;
            stb.Text = newSize;
        }
        //------------------------------------------------------------------------

        private void updateTxtBox(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name.Substring("tb".Length);
            string type = (string)((Control)sender).Tag;

            if (type == "Picture")
            {
                CheckBox cb = this.Controls.Find("cb" + name, true).FirstOrDefault() as CheckBox;
                readImageStats(name);
                if (cb.Checked)
                {
                    string currentGroup = (string)cb.Tag;
                    setImage(currentGroup, name);
                }
            }
            else if (type == "Area")
            {
                string str = ((Control)sender).Text;
                int[] coords = Editor.stringToRectArray(str);

                if (coords != null && isValidRect(coords))
                {
                        changeArea(name, coords);
                        setImageUpdateStatus(name, true);
                }
                else
                {
                    setImageUpdateStatus(name, false);
                    TextBox stb = this.Controls.Find("size" + name, true).FirstOrDefault() as TextBox;
                    stb.Text = "Error!";
                }

            } else if (type == "Custom")
            {
                // Update Array data
                // nothing more
                Editor.iniArray[name] = ((Control)sender).Text;
            }

        }
        //----------------------------------------------
        private void changeArea(string itemName, int[] coords)
        {
            PictureBox pb = this.Controls.Find(itemName, true).FirstOrDefault() as PictureBox;
            pb.Location = new Point(coords[0], coords[1]);
            pb.Size = new Size(coords[2] - coords[0], coords[3] - coords[1]);
            pb.Refresh();
        }


        private bool isValidRect(int[] coords)
        {
            if (coords.Length == 4 && coords[2] >= coords[0] && coords[3] >= coords[1])
                return true;
            else return false;
        }

        //-----------------------------------------------------------------------


        private void clearGui()
        {
            Editor.lineCounter = 0;
            panel1.Controls.Clear();
            panel2.Controls.Clear();


            // Disable horisontal scroll bar trick
            panel2.AutoScroll = false;
            panel2.HorizontalScroll.Enabled = false;
            panel2.AutoScroll = true;
        }

        public void getSettingsUpdate()
        {
            Editor.WriteParam();
            refreshJSON();
            drawDefault(Editor.configJsonKeys[0]);
        }


        public void refreshJSON()
        {
            Editor.GetJson();
            generateStripMenu();
        }

        private void openSettingsForm()
        {
            SettingsForm sf = new SettingsForm();
            var result = sf.ShowDialog();
            if (result == DialogResult.OK)
            {
                //string val = sf.ReturnValue1;
                getSettingsUpdate();
            }
        }

        // ------------ STRIP  MENU ----------------------------------------------------

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openProject();
        }
            
        private void refreshJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshJSON();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileSaver(Editor.iniPath);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveIniToFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openSettingsForm();
        }



    }
}

