
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
                //Name = "cb" + cInfo.name,
                Name = "cb",
                Checked = (_cInfo.picIndex == 0),
                Tag = cInfo.parentName,
            };
            this.Controls.Add(CheckBox);


            var openBttn = new ButtonToOpen("open" + cInfo.name);
            new ToolTip().SetToolTip(openBttn, "Browse...");
            this.Controls.Add(openBttn);

            var playBttn = new ButtonToAnimate("play");
            new ToolTip().SetToolTip(playBttn, "Play");

            if (cInfo.animated && cInfo.picIndex == 0)
            {
                playBttn.Visible = true;
                playBttn.displayButton = true;
            }

            this.Controls.Add(playBttn);

            this.Tag = new tableRowTag { parentName = cInfo.parentName };

        }



    }
}
