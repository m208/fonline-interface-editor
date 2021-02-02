
using FOIE.TableLines;
using System.Windows.Forms;

namespace FOIE
{
    class TableLinePicture : TableLine
    {
        public TableLinePicture(ControlInfo _cInfo)
        {
            cInfo = _cInfo;
            createTableLine();

            var CheckBox = new CheckBoxImageSwitch
            {
                Name = "cb" + cInfo.name,
                Checked = (_cInfo.picIndex == 0),   
                Tag = cInfo.parentName,
            };
            this.Controls.Add(CheckBox);
 

            var openBttn = new ButtonToOpen("open" + cInfo.name);
            new ToolTip().SetToolTip(openBttn, "Browse...");
            this.Controls.Add(openBttn);

            if (cInfo.animated && cInfo.picIndex == 0)
            {
                var playBttn = new ButtonToAnimate("play" + cInfo.name);
                //new ToolTip().SetToolTip(openBttn, "Browse...");
                this.Controls.Add(playBttn);
            }


            this.Tag = new tableRowTag { parentName = cInfo.parentName };

        }
    }
}
