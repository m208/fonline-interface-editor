using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOIE
{
    public class AppControl
    {
        //private void addTxtControlsLine(string parentName, string name, bool clickAble, bool hideAble, string tb1Text, string tb2Text, bool openAble, string controlType, bool controlSuccess)

        string parentName;
        string name;
        bool clickAble;
        bool hideAble;
        string tb1Text;
        string tb2Text;
        bool openAble;
        string controlType;
        bool controlSuccess;

        public string controlRectValue, controlImageValue = "123";
        
        //public Size controlSize = new Size(0, 0);

        public PictureBox picBox;
        public Bitmap controlImage;


        public AppControl(string rectName, string imgName)
        {
            Size picSize = new Size(0, 0);
            Size controlSize = new Size(0, 0);

            if (Editor.iniArray.ContainsKey(imgName))
            {
                controlImageValue = Editor.iniArray[imgName];
            }

            string path = (Editor.fullPath + controlImageValue);
            bool imgExist = true;

            if (File.Exists(path))
            {
                if (getFileExtension(path) == ".frm")
                {
                    Frm frmImg = new Frm(path);
                    controlImage = frmImg.bitmaps[0];
                } else
                {
                    controlImage = LoadBitmapUnlocked(path);
                }

                picSize = new Size(controlImage.Width, controlImage.Height);

            } else
            {
                controlImage = new Bitmap(FOIE.Properties.Resources.nofile1);
                imgExist = false;
            }


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
            }

            //--------------------------------------------------------------------------------------
            picBox = new PictureBox
            {
                Name = rectName,          
                Size = controlSize,
                Location = new Point(0, 0),
                Image = controlImage,
                BackgroundImageLayout = ImageLayout.None,
            };

            //string hint = Editor.getHintforKey(name);
            //new ToolTip().SetToolTip(picBox, hint);

            picBox.LocationChanged += new EventHandler(picBox_Changed);
            picBox.SizeChanged += new EventHandler(picBox_Changed);
            picBox.Paint += new PaintEventHandler(picBox_Paint);

            ControlMoverOrResizer.Init(picBox);

        }
        //  EventHandlers   -----------------------------------------------------------------

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(new Pen(Color.Red, 2), 0, 0, ((Control)sender).Width, ((Control)sender).Height);
            g.DrawString(((Control)sender).Name, new Font("Tahoma", 8), Brushes.White, 0, 0);
        }
        private void picBox_Changed(object sender, EventArgs e)
        {
            // Callback here
        }


        //  Utils   --------------------------------------------------------------------------
        private string getFileExtension (string path)
        {
            return Path.GetExtension(path);
        }

        private Bitmap LoadBitmapUnlocked(string path)
        {
            using (Bitmap bm = new Bitmap(path))
            {
                return new Bitmap(bm);
            }
        }

        private bool isValidRect(int[] coords)
        {
            if (coords.Length == 4 && coords[2] >= coords[0] && coords[3] >= coords[1])
                return true;
            else return false;
        }



    }
}
