using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FOIE.Controls
{
    public class PicBox : PictureBox
    {
        public List<Bitmap> images;

        public PicBox() {}

    }

    //      TABLE ELEMENTS      -------------------------------------------------------------------------------------------------
    //      PICTURE BOX         -------------------------------------------------------------------------------------------------

    public class PicBoxIcon : PictureBox
    {
        public PicBoxIcon(string name, string controlType)
        {
            Name = "typePic";
            Size = new Size(20, 20);
            Location = new Point(175, 0);
            Image = (Image)Properties.Resources.ResourceManager.GetObject(Editor.controlTypesResources[controlType]);
            BackgroundImageLayout = ImageLayout.Stretch;
            //Tag = controlType;
        }
        public void drawError()
        {
            Image = new Bitmap(FOIE.Properties.Resources.icon_error);
        }
        public void drawIcon(string controlType)
        {
            Image = (Image)Properties.Resources.ResourceManager.GetObject(Editor.controlTypesResources[controlType]);
        }
    }

    //      LABELS -------------------------------------------------------------------------------------------------

    class LabelHeader : Label
    {
        public LabelHeader(string name)
        {
            Name = "lbl" + name;
            Size = new Size(100, 25);
            Location = new Point(5, 0);
            Text = name;
        }
    }

    class LinkLabelHeader : LinkLabel
    {
        public LinkLabelHeader(string name)
        {
            Name = "lbl" + name;
            Size = new Size(100, 25);
            Location = new Point(5, 0);
            Text = name;
        }
    }

    //      TEXT BOXES   -------------------------------------------------------------------------------------------------

    class TextBoxForValues : TextBox
    {
        public TextBoxForValues(string name, string value, string clType)
        {
            Name = "tb" + name;
            Size = new Size(100, 25);
            Location = new Point(200, 0);
            Text = value;
        }
    }

    class TextBoxForInfo : TextBox
    {
        public TextBoxForInfo(string name, string value)
        {
            Name = "size" + name;
            Size = new Size(60, 25);
            Location = new Point(305, 0);
            Text = value;
            Enabled = false;
        }
    }

    //      BUTTONS     -------------------------------------------------------------------------------------------------

    public class ButtonToHide : Button
    {
        public bool isShowed = true;

        public ButtonToHide(string name)
        {
            Name = name;

            Size = new Size(20, 20);
            Location = new Point(150, 0);
            BackgroundImage = Properties.Resources.eye;
            BackgroundImageLayout = ImageLayout.Zoom;
        }

        public void redrawButtonIcon()
        {
            BackgroundImage = isShowed ? Properties.Resources.eye : Properties.Resources.hide3;
        }
    }

    public class ButtonToZ : Button
    {
        public bool isOnTop = true;

        public ButtonToZ(string name)

        {
            Name = name;

            Size = new Size(20, 20);
            Location = new Point(125, 0);
            BackgroundImage = Properties.Resources.resize;
            BackgroundImageLayout = ImageLayout.Zoom;
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
        }


    }

    //      CHECK BOX -------------------------------------------------------------------------------------------------

    public class CheckBoxImageSwitch : CheckBox
    {
        public CheckBoxImageSwitch()
        {
            Size = new Size(20, 20);
            Location = new Point(155, 0);
            AutoCheck = false;
        }

    }
}
