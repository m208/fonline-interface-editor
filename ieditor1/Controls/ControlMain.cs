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

            string path = Editor.getFullPath(controlImageValue);
            bool imgExist = true;

            if (File.Exists(path))
            {
                controlImage = Editor.GetBitmapFromPath(path);
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
                if (Editor.isValidRect(coords))
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

            picBox.Paint += new PaintEventHandler(picBox_Paint);
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
    }




}
