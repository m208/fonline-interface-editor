using System;
using System.Drawing;
using System.Windows.Forms;

namespace FOIE.Controls
{
    public class PicBox : PictureBox
    {

    }


    public class ButtonToHide : Button
    {

        public ButtonToHide(string name)

        {
            Name = name;

            Size = new Size(20, 20);
            Location = new Point(150, 0);
            BackgroundImage = Properties.Resources.eye;
            BackgroundImageLayout = ImageLayout.Zoom;
            Tag = "Show";
        }
    }

    public class ButtonToZ : Button
    {

        public ButtonToZ(string name)

        {
            Name = name;

            Size = new Size(20, 20);
            Location = new Point(125, 0);
            BackgroundImage = Properties.Resources.resize;
            BackgroundImageLayout = ImageLayout.Zoom;
            Tag = "Up";
        }
    }

    public class ButtonToOpen : Button
    {

        public ButtonToOpen(string name)

        {
            Name = name;

            Size = new Size(20, 20);
            Location = new Point(370, 0);
            BackgroundImage = Properties.Resources.folder1;
            BackgroundImageLayout = ImageLayout.Zoom;
            Tag = "Up";
        }
        

    }

    public class CheckBoxImageSwitch: CheckBox
    {
        public CheckBoxImageSwitch()
        {
            Size = new Size(20, 20);
            Location = new Point(155, 0);
            AutoCheck = false;
        }

    }
}
