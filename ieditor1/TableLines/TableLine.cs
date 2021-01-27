using ieditor1;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FOIE.TableLines
{
    public class TableLine
    {

        public Panel Panel = new Panel();
        public ControlInfo cInfo;
        public string controlTypeImg = "icon_hatch";        // area by default area

        public TableLine() { }

        public TableLine(ControlInfo _cInfo)
        {
            cInfo = _cInfo;
            createTableLine();
        }

        public void createTableLine()
        {

            Panel = new Panel
            {
                Name = "panel" + cInfo.name,
                Size = new Size(450, 25),
                Location = new Point(0, 25 * Editor.lineCounter),   // didnt work out here
                //Tag = tableRowTag,
            };

            // ------ LABELS ---------------------------------------------------

            string hint = Editor.getHintforKey(cInfo.name);     // tool tips from dictionary

            if (cInfo.clType == "Area")
            {
                LinkLabel llbl = new LinkLabel
                {
                    Name = "lbl" + cInfo.name,
                    Size = new Size(100, 25),
                    Location = new Point(5, 0),
                    Text = cInfo.name,
                };
                new ToolTip().SetToolTip(llbl, hint);
                Panel.Controls.Add(llbl);
            }
            else
            {
                Label lbl = new Label
                {
                    Name = "lbl" + cInfo.name,
                    Size = new Size(100, 25),
                    Location = new Point(5, 0),
                    Text = cInfo.name,
                };
                new ToolTip().SetToolTip(lbl, hint);
                Panel.Controls.Add(lbl);
            }

            // ------ PICTURE ---------------------------------------------------

            object img = FOIE.Properties.Resources.ResourceManager.GetObject(controlTypeImg);

            PictureBox pb = new PictureBox
            {
                Name = "typePic" + cInfo.name,
                Size = new Size(20, 20),
                Location = new Point(175, 0),
                Image = (Image)img,
                BackgroundImageLayout = ImageLayout.Stretch,
                Tag = cInfo.clType,
            };

            Panel.Controls.Add(pb);

            // ------ TEXT BOXES ---------------------------------------------------

            TextBox tb1 = new TextBox
            {
                Name = "tb" + cInfo.name,
                Size = new Size(100, 25),
                Location = new Point(200, 0),
                Text = cInfo.textValue,
                Tag = cInfo.clType,
            };

            TextBox tb2 = new TextBox
            {
                Name = "size" + cInfo.name,
                Size = new Size(60, 25),
                Location = new Point(305, 0),
                Text = cInfo.textInfo,
                Enabled = false,
            };

            Panel.Controls.Add(tb1);
            Panel.Controls.Add(tb2);

        }


    }
}

