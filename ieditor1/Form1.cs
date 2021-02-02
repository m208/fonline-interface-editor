using FOIE;
using FOIE.Controls;
using FOIE.TableLines;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
            }
            else
            {
                MessageBox.Show("Error reading config file");
                openSettingsForm();
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
                if (sameFolderRequired)
                {
                    while (!openFileDialog1.FileName.Contains(openFileDialog1.InitialDirectory))
                    {
                        MessageBox.Show("Please select file which is in the default folder", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        openFileDialog1.ShowDialog();
                    }
                }
                return openFileDialog1.FileName;
            }
        }

        //-----------------------------------------------------------------------

        private void colorizeRows()
        {
            bool switchColor = true;
            string lastGroup = "";

            Color odd = Color.MintCream;
            Color even = Color.LightGray;

            foreach (Panel p in panel2.Controls.OfType<Panel>())
            {
                tableRowTag tag = (tableRowTag)(((Panel)p).Tag);
                string currentGroup = (string)(tag.parentName);

                if (currentGroup != lastGroup || currentGroup == null)
                {
                    switchColor = !switchColor;
                }
                lastGroup = currentGroup;

                Color currntColor = (switchColor) ? odd : even;
                ((Panel)p).BackColor = currntColor;

                tableRowTag newTag = new tableRowTag
                {
                    parentName = "currentGroup",
                    color = currntColor
                };

                ((Panel)p).Tag = newTag;

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

        private void clearGui()
        {
            Editor.lineCounter = 0;
            Editor.tableRows.Clear();
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

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void test2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newIni = "F:\\FOIE\\defFO2intrface\\01.ini";
            string directoryName = Path.GetDirectoryName(newIni);
            Editor.fullPath = directoryName + "\\";
            Editor.iniPath = newIni;

            Editor.iniRead(newIni);
            drawDefault(Editor.configJsonKeys[1]);
        }

        //------------------------------------------------------------------

        private void drawDefault(string item)
        {
            clearGui();


            if (Editor.configJSON[item]["Main"] != null)
            {
                var bG = Editor.configJSON[item]["Main"] as JArray;

                if (bG.Count > 0)
                {
                    string mainImg = (string)bG[0].ToObject(typeof(string));
                    string mainBg = (string)bG[1].ToObject(typeof(string));

                    Editor.currentBackground = mainImg;

                    AddControlMain main = new AddControlMain(mainImg, mainBg);
                    panel1.Controls.Add(main.picBox);

                    var line = new TableLine(main.controlInfo[0]);
                    Editor.tableRows.Add(line);

                    line = new TableLinePicture(main.controlInfo[1]);
                    Editor.tableRows.Add(line);
                }
            }

            PictureBox bgImg = panel1.Controls.Find(Editor.currentBackground, true).FirstOrDefault() as PictureBox;

            if (Editor.configJSON[item]["Controls"] != null)
            {
                var Controls = Editor.configJSON[item]["Controls"] as JArray;

                foreach (var i in Controls)
                {
                    if (i.GetType().Name == "JArray")
                    {
                        string[] array = (string[])i.ToObject(typeof(string[]));
                        AddControlButton area = new AddControlButton(array);
                        bgImg.Controls.Add(area.picBox);

                        TableLineArea line = new TableLineArea(area.controlInfo[0]);
                        Editor.tableRows.Add(line);

                        for (int k = 1; k < area.controlInfo.Count; k++)
                        {
                            TableLinePicture picLine = new TableLinePicture(area.controlInfo[k]);
                            Editor.tableRows.Add(picLine);
                        }
                    }
                    else
                    {
                        string name = (string)i.ToObject(typeof(string));

                        AddControlArea area = new AddControlArea(name);
                        bgImg.Controls.Add(area.picBox);

                        TableLineArea line = new TableLineArea(area.controlInfo[0]);
                        Editor.tableRows.Add(line);

                    }
                }
            }

            var CustomFields = Editor.configJSON[item]["Custom"] as JArray;
            foreach (var i in CustomFields)
            {
                string name = (string)i.ToObject(typeof(string));

                if (Editor.iniArray.ContainsKey(name))
                {
                    string value = Editor.iniArray[name];
                    ControlInfo cInfo = new ControlInfo
                    {
                        name = name,
                        clType = "Custom",
                        textValue = value,
                    };

                    TableLine line = new TableLine(cInfo);
                    Editor.tableRows.Add(line);
                }
            }

            panel2.Visible = false;
            drawTable();
            colorizeRows();
            panel2.Visible = true;

            setEventHandlers();
        }

        private void drawTable()
        {
            panel2.SuspendLayout();
            foreach (TableLine line in Editor.tableRows)
            {
                line.Location = new Point(0, 25 * Editor.lineCounter);
                panel2.Controls.Add(line);
                Editor.lineCounter++;
            }
            panel2.ResumeLayout(false);
        }

        private void setEventHandlers()
        {
            PicBox bg = panel1.Controls.Find(Editor.currentBackground, true).FirstOrDefault() as PicBox;
            foreach (PicBox pb in bg.Controls.OfType<PicBox>())
            {
                pb.LocationChanged += (sender, e) => picBox_LocSizeChanged(pb, e);  // ok ++
                pb.SizeChanged += (sender, e) => picBox_LocSizeChanged(pb, e);
                pb.MouseDown += (sender, e) => picBoxClickHighlight(pb, e);
                pb.MouseUp += (sender, e) => picBoxClickHighlightOff(pb, e);
            }

            foreach (Panel p in panel2.Controls.OfType<Panel>())
            {
                foreach (TextBoxForValues tb in p.Controls.OfType<TextBoxForValues>())
                {
                    tb.TextChanged += (sender, e) => updateTxtBox(tb, e);           // ok +
                }
                foreach (ButtonToHide bttn in p.Controls.OfType<ButtonToHide>())
                {
                    bttn.Click += (sender, e) => { showHideArea(bttn, e); };        // ok ++
                }
                foreach (ButtonToZ bttn in p.Controls.OfType<ButtonToZ>())
                {
                    bttn.Click += (sender, e) => { changeZLayer(bttn, e); };        // ok ++
                }
                foreach (ButtonToOpen bttn in p.Controls.OfType<ButtonToOpen>())
                {
                    bttn.Click += (sender, e) => { openImgFile(bttn, e); };           // ok ++
                }
                foreach (CheckBoxImageSwitch cb in p.Controls.OfType<CheckBoxImageSwitch>())
                {
                    cb.Click += (sender, e) => { selectPicToShow(cb, e); };               // ok
                }
                foreach (LinkLabel l in p.Controls.OfType<LinkLabel>())
                {
                    l.MouseDown += (sender, e) => panelClickHighlight(l, e);        // ok +
                    l.MouseUp += (sender, e) => panelClickHighlightOff(l, e);
                }
            }
        }


        //      EVENT HANDLERS  ----------------------------------------------
        private void picBox_LocSizeChanged(object sender, EventArgs e)
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

            var cName = "panel" + ((Control)sender).Name;
            TableLine line = this.Controls.Find(cName, true).FirstOrDefault() as TableLine;
            line.updateValue(newValue);
            line.updateInfo(((Control)sender).Width + "x" + ((Control)sender).Height);

            Editor.iniArray[itemName] = newValue;
        }

        //------------------------------------------------------------------------------------------------------------------

        private void picBoxClickHighlight(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name;
            TableLine line = panel2.Controls.Find("panel" + name, true).FirstOrDefault() as TableLine;
            line.highLiteRow(true);

        }
        private void picBoxClickHighlightOff(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name;
            TableLine line = panel2.Controls.Find("panel" + name, true).FirstOrDefault() as TableLine;
            line.highLiteRow(false);
        }

        //------------------------------------------------------------------------------------------------------------------

        private void updateTxtBox(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name.Substring("tb".Length);
            string str = ((Control)sender).Text;

            TableLine line = panel2.Controls.Find("panel" + name, true).FirstOrDefault() as TableLine;
            string type = line.cInfo.clType;

            if (type == "Picture")
            {
                Bitmap newImg = readImage(str);

                string currentGroup = line.cInfo.parentName;
                int picIndex = line.cInfo.picIndex;
                PicBox pb = panel1.Controls.Find(currentGroup, true).FirstOrDefault() as PicBox;

                if (newImg == null)
                {
                    line.drawErrorIcon();
                    line.updateInfo("Error!");
                    newImg = new Bitmap(FOIE.Properties.Resources.nofile1);
                }
                else
                {
                    line.drawOkIcon();
                    line.updateInfo(newImg.Width + "x" + newImg.Height);
                }

                pb.images[picIndex] = newImg;


                CheckBox cb = this.Controls.Find("cb" + name, true).FirstOrDefault() as CheckBox;
                if (cb.Checked)
                {
                    pb.Image = newImg;
                }
            }
            else if (type == "Area" || type == "AreaMain")
            {
                int[] coords = Editor.stringToRectArray(str);

                if (coords != null && Editor.isValidRect(coords))
                {
                    PictureBox pb = this.Controls.Find(name, true).FirstOrDefault() as PictureBox;
                    pb.Location = new Point(coords[0], coords[1]);
                    pb.Size = new Size(coords[2] - coords[0], coords[3] - coords[1]);
                    pb.Refresh();

                    line.drawOkIcon();
                    line.updateInfo(Editor.getSizeFromStringCoords(str));
                }
                else
                {
                    line.drawErrorIcon();
                    line.updateInfo("Error!");
                }

            }

            Editor.iniArray[name] = ((Control)sender).Text;
        }

        //------------------------------------------------------------------------------------------------------------------

        private void showHideArea(object sender, EventArgs e)
        {
            ButtonToHide b = (ButtonToHide)sender;
            string name = b.Name.Substring("hide".Length); // crop "hide"

            PictureBox p = panel1.Controls.Find(name, true).FirstOrDefault() as PictureBox;

            if (b.isShowed) p.Visible = false;
            else p.Visible = true;

            b.isShowed = !b.isShowed;
            b.redrawButtonIcon();
        }

        //-------------------------------------------------------------------

        private void changeZLayer(object sender, EventArgs e)
        {
            ButtonToZ b = (ButtonToZ)sender;
            string name = b.Name.Substring("zBttn".Length); // crop "zBttn"

            PictureBox p = panel1.Controls.Find(name, true).FirstOrDefault() as PictureBox;

            if (b.isOnTop) p.SendToBack();
            else p.BringToFront();

            b.isOnTop = !b.isOnTop;
        }

        //-------------------------------------------------------------------

        private void openImgFile(object sender, EventArgs e)
        {
            string newPic = getFileName("Images|*.png; *.jpg; *frm; *.fofrm", true);  
            if (newPic != null)
            {
                string picName = newPic.Substring(Editor.fullPath.Length);
                string currentItem = (((Control)sender).Name).Substring("open".Length);

                Editor.iniArray[currentItem] = picName;

                TableLine line = this.Controls.Find("panel" + currentItem, true).FirstOrDefault() as TableLine;
                line.updateValue(picName);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void selectPicToShow(object sender, EventArgs e)
        {
            CheckBoxImageSwitch cb = (Control)sender as CheckBoxImageSwitch;
            if (cb.Checked) return;

            // else

            var allCBoxes = GetAll(this, typeof(CheckBoxImageSwitch));

            string name = cb.Name.Substring("cb".Length); // substring = crop "cb"
            TableLine line = panel2.Controls.Find("panel" + name, true).FirstOrDefault() as TableLine;
            int picIndex = line.cInfo.picIndex;
            string currentGroup = (string)cb.Tag;

            foreach (CheckBoxImageSwitch c in allCBoxes)
            {
                if (c.Tag == cb.Tag)
                {
                    c.Checked = false;
                }
            }
            cb.Checked = true;

            PicBox pb = panel1.Controls.Find(currentGroup, true).FirstOrDefault() as PicBox;
            pb.Image = pb.images[picIndex];
        }

        //-------------------------------------------------------------------
        private void picBox_Changed(object sender, EventArgs e)
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

            var sName = "size" + ((Control)sender).Name;
            TextBox stb = this.Controls.Find(sName, true).FirstOrDefault() as TextBox;
            stb.Text = ((Control)sender).Width + "x" + ((Control)sender).Height;

            Editor.iniArray[itemName] = newValue;
        }

        //---------------------------------------------------------------------------------------

        private void panelClickHighlight(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name.Substring("lbl".Length);
            PictureBox p = panel1.Controls.Find(name, true).FirstOrDefault() as PictureBox;
            p.BringToFront();
            p.BackColor = Color.Violet;

            ButtonToZ zb = this.Controls.Find("zBttn" + name, true).FirstOrDefault() as ButtonToZ;
            zb.isOnTop = true;
        }

        private void panelClickHighlightOff(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name.Substring(3);
            PictureBox p = panel1.Controls.Find(name, true).FirstOrDefault() as PictureBox;
            p.BackColor = Color.Transparent;
        }

        //------------------------------------------------------------------------

        private Bitmap readImage(string fileName)
        {
            string path = Editor.getFullPath(fileName);
            if (File.Exists(path))
            {
                ImgPreparer image = new ImgPreparer(path);
                return image.images[0];

            }
            else return null;
        }

        //------------------------------------------------------------------------



    }
}

