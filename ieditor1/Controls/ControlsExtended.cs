using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FOIE
{
    public class PicBox : PictureBox
    {

        public List<ImgPreparer> frames;
        public List<Bitmap> images;
        private readonly Timer t = new Timer();

        public PicBox() { }

        public void PlayAnimation(int picIndex, bool animationPaused)
        {
            int num = 0;
            if (!animationPaused)
            {
                if (frames[picIndex].fps == 0) frames[picIndex].fps = 10;   // 0 fps at some files???
                t.Interval = 1000 / frames[picIndex].fps;

                t.Enabled = true;
                t.Tick += delegate
                {
                    num = ++num % frames[picIndex].images.Count;
                    Image = frames[picIndex].images[num];
                };

            }
            else t.Enabled = false;
        }

    }

    //      TABLE ELEMENTS      -------------------------------------------------------------------------------------------------
    //      PICTURE BOX         -------------------------------------------------------------------------------------------------

    public class PicBoxIcon : PictureBox
    {
        public PicBoxIcon(string name, string controlType, bool controlSuccess = true)
        {
            Name = "typePic";
            Size = new Size(20, 20);
            Location = new Point(175, 0);
            Image = controlSuccess ? (Image)Properties.Resources.ResourceManager.GetObject(Editor.controlTypesResources[controlType]) : new Bitmap(FOIE.Properties.Resources.icon_error);
            BackgroundImageLayout = ImageLayout.Stretch;
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

    public class ButtonToAnimate : Button
    {
        public bool displayButton = false;

        public ButtonToAnimate(string name)

        {
            Name = name;

            Size = new Size(20, 20);
            Location = new Point(125, 0);
            BackgroundImage = Properties.Resources.play;
            BackgroundImageLayout = ImageLayout.Zoom;

            Visible = displayButton;
        }

        public void redrawButtonIcon(bool isStopped)
        {
            BackgroundImage = isStopped ? Properties.Resources.play : Properties.Resources.pause;
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
