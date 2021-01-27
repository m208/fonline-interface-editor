using FOIE.TableLines;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace FOIE
{
    class TableLinePicture : TableLine
    {

        public TableLinePicture(ControlInfo _cInfo)
        {
            cInfo = _cInfo;
            controlTypeImg = (cInfo.controlSuccess) ? "icon_image" : "icon_error";

            createTableLine();

            bool checkedBox = _cInfo.picIndex == 0;
            var CheckBox = new CheckBox
            {
                Name = "cb" + cInfo.name,
                Size = new Size(20, 20),
                Location = new Point(155, 0),
                Checked = checkedBox,
                AutoCheck = false,
                //Tag = parentName,
            };

            //CheckBox.Click += (sender, e) => { showHideImg(CheckBox, e); };
            Panel.Controls.Add(CheckBox);

            var openBttn = new Button
            {
                Name = "open" + cInfo.name,
                Size = new Size(20, 20),
                Location = new Point(370, 0),
                BackgroundImage = (Image)(FOIE.Properties.Resources.folder1),
                BackgroundImageLayout = ImageLayout.Zoom,
            };

            //openBttn.Click += (sender, e) => { changeImg(openBttn, e); };
            Panel.Controls.Add(openBttn);


        }

    }
}
