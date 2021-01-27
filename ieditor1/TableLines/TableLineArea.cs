using System;
using System.Drawing;
using System.Windows.Forms;

namespace FOIE.TableLines
{
    class TableLineArea : TableLine
    {
        public TableLineArea(ControlInfo _cInfo)
        {
            cInfo = _cInfo;
            controlTypeImg = "icon_hatch";
            createTableLine();


            Button hideBttn = new Button
            {
                Name = "hide" + cInfo.name,
                Size = new Size(20, 20),
                Location = new Point(150, 0),
                BackgroundImage = Properties.Resources.eye,
                BackgroundImageLayout = ImageLayout.Zoom,
                Tag = "Show",
            };
            hideBttn.Click += (sender, e) => { showHideArea(hideBttn, e); };
            new ToolTip().SetToolTip(hideBttn, "Show/ hide element");
            Panel.Controls.Add(hideBttn);

            Button zBttn = new Button
            {
                Name = "zBttn" + cInfo.name,
                Size = new Size(20, 20),
                Location = new Point(125, 0),
                BackgroundImage = (Image)(FOIE.Properties.Resources.resize),
                BackgroundImageLayout = ImageLayout.Zoom,
                Tag = "Up",
            };
            zBttn.Click += (sender, e) => { changeZLayer(zBttn, e); };
            new ToolTip().SetToolTip(zBttn, "Bring to front/ send to back");
            Panel.Controls.Add(zBttn);

        }
        private void showHideArea(object sender, EventArgs e)
        {
            /*
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
            */
        }
        private void changeZLayer(object sender, EventArgs e)
        {
            //string name = ((Control)sender).Name.Substring("zBttn".Length); // crop "zBttn"
            //PictureBox p = panel1.Controls.Find(name, true).FirstOrDefault() as PictureBox;

            //if (((Control)sender).Tag.Equals("Up"))
            //{
            //    ((Control)sender).Tag = "Down";
            //    p.SendToBack();
            //}
            //else if (((Control)sender).Tag.Equals("Down"))
            //{
            //    ((Control)sender).Tag = "Up";
            //    p.BringToFront();
            //}

        }
    }
}
