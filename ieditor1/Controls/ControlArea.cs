using System;
using System.Drawing;
using System.Windows.Forms;

namespace FOIE
{
    public class AddControlArea : AddControlMain
    {
        //public Form1 parentForm;
        public AddControlArea() { }

        public AddControlArea(string name)
        {
            generateControl(name);
        }

        public void generateControl(string name)
        {

            string value = "0 0 0 0";
            if (Editor.iniArray.ContainsKey(name))
            {
                value = Editor.iniArray[name];
            }

            int[] coords = Editor.stringToRectArray(value);

            Size cSize = new Size(0, 0);
            if (Editor.isValidRect(coords))
            {
                cSize = new Size(coords[2] - coords[0], coords[3] - coords[1]);
            }

            Bitmap img = new Bitmap(FOIE.Properties.Resources.hatch);

            picBox = new PicBox
            {
                Name = name,
                Location = new Point(coords[0], coords[1]),
                Size = cSize,
                BackgroundImage = img,
                BackgroundImageLayout = ImageLayout.Tile,
                BackColor = Color.Transparent,

            };



            string hint = Editor.getHintforKey(name);
            new ToolTip().SetToolTip(picBox, hint);

            picBox.Paint += new PaintEventHandler(picBox_Paint);
            ControlMoverOrResizer.Init(picBox);

            ControlInfo cInfo = new ControlInfo
            {
                name = name,
                clType = "Area",
                textValue = value,
                textInfo = Editor.getSizeFromStringCoords(value),
            };
            controlInfo.Add(cInfo);


        }



        private void picBoxClickHighlight(object sender, EventArgs e)
        {
            //((Control)sender).BringToFront();
            string name = ((Control)sender).Name;
            //Panel p = panel2.Controls.Find("panel" + name, true).FirstOrDefault() as Panel;
            //p.BackColor = Color.Violet;
        }
        private void picBoxClickHighlightOff(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name;
            //            Panel p = panel2.Controls.Find("panel" + name, true).FirstOrDefault() as Panel;

            //          tableRowTag tableRowTag = (tableRowTag)((Panel)p).Tag;
            //       Color prevColor = (Color)(tableRowTag.Get("BgColor"));
            //        p.BackColor = prevColor;
        }
    }
}
