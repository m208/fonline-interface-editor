using FOIE.Controls;
using ieditor1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FOIE
{
    public class AddControlMain
    {

        public PicBox picBox;

        public List<ControlInfo> controlInfo = new List<ControlInfo>();
        public string controlRectValue, controlImageValue;
        
        
        
        public Bitmap controlImage;
        public List<Bitmap> controlImages = new List<Bitmap>();

        public AddControlMain() { }

        public AddControlMain(string rectName, string imgName)
        {
            
            string controlImageValue = "";
            string controlRectValue  = "0 0 0 0";
            Size picSize = new Size(0, 0);
            Size controlSize = new Size(0, 0);

            if (Editor.iniArray.ContainsKey(imgName))
            {
                controlImageValue = Editor.iniArray[imgName];
            }

            string path = getFullPath(controlImageValue);
            bool imgExist = true;

            if (File.Exists(path))
            {
                controlImage = GetBitmap(path);
                picSize = new Size(controlImage.Width, controlImage.Height);
            }
            else
            {
                controlImage = new Bitmap(FOIE.Properties.Resources.nofile1);
                imgExist = false;
            }

            controlImages.Add(controlImage);

            if (Editor.iniArray.ContainsKey(rectName))
            {
                controlRectValue = Editor.iniArray[rectName];

                int[] coords = Editor.stringToRectArray(controlRectValue);
                if (isValidRect(coords))
                {
                    controlSize = new Size(coords[2] - coords[0], coords[3] - coords[1]);
                }
            }

            if (rectName == "0")
            {
                if (imgExist) controlSize = picSize;
                else controlSize = new Size(0, 0);

                //controlRectValue = ?
            }

            //--------------------------------------------------------------------------------------
            picBox = new PicBox
            {
                Name = rectName,
                Size = controlSize,
                Location = new Point(0, 0),
                Image = controlImage,
                BackgroundImageLayout = ImageLayout.None,
            };

            string hint = Editor.getHintforKey(rectName);
            new ToolTip().SetToolTip(picBox, hint);

            //picBox.LocationChanged += new EventHandler(picBox_Changed);
            //picBox.SizeChanged += new EventHandler(picBox_Changed);
            picBox.Paint += new PaintEventHandler(picBox_Paint);

           // picBox.MouseDown += (sender, e) => picBoxClickHighlight(picBox, e);
           // picBox.MouseUp += (sender, e) => picBoxClickHighlightOff(picBox, e);

            ControlMoverOrResizer.Init(picBox);


            ControlInfo cInfo = new ControlInfo
            {
                name = rectName,
                clType = "AreaMain",
                textValue = controlRectValue,
                textInfo = controlSize.Width + "x" + controlSize.Height,

            };
            controlInfo.Add(cInfo);

            cInfo = new ControlInfo
            {
                name = imgName,
                clType = "Picture",
                textValue = controlImageValue,
                textInfo = picSize.Width + "x" + picSize.Height,
                controlSuccess = imgExist,
            };
            controlInfo.Add(cInfo);
            
        }
        //  EventHandlers   -----------------------------------------------------------------

        public void picBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(new Pen(Color.Red, 2), 0, 0, ((Control)sender).Width, ((Control)sender).Height);
            g.DrawString(((Control)sender).Name, new Font("Tahoma", 8), Brushes.White, 0, 0);
        }
        public void picBox_Changed(object sender, EventArgs e)
        {
            // Callback here
        }


        //  Utils   --------------------------------------------------------------------------
        public string getFullPath(string filename)
        {
            return Editor.fullPath + filename;
        }

        public Bitmap GetBitmap(string path)
        {
            Bitmap controlImage;
            if (getFileExtension(path) == ".frm")
            {
                Frm frmImg = new Frm(path);
                controlImage = frmImg.bitmaps[0];
            }
            else
            {
                controlImage = LoadBitmapUnlocked(path);
            }
            return controlImage;
        }

        private string getFileExtension(string path)
        {
            return Path.GetExtension(path).ToLower();
        }

        private Bitmap LoadBitmapUnlocked(string path)
        {
            using (Bitmap bm = new Bitmap(path))
            {
                return new Bitmap(bm);
            }
        }

        public bool isValidRect(int[] coords)
        {
            if (coords.Length == 4 && coords[2] >= coords[0] && coords[3] >= coords[1])
                return true;
            else return false;
        }

    }
    //--------------------------------------------------------------------------------------------

    public class ControlInfo
    {
        public string name;
        public string parentName;
        public string clType;
        public string textValue;
        public string textInfo;
        public bool controlSuccess = true;
        public int picIndex;
        //public Form1 parentForm;
    }




}
