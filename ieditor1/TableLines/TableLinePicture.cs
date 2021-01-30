using FOIE.Controls;
using FOIE.TableLines;
using System.Windows.Forms;

namespace FOIE
{
    class TableLinePicture : TableLine
    {
        public TableLinePicture(ControlInfo _cInfo)
        {
            cInfo = _cInfo;
            controlTypeImg = (cInfo.controlSuccess) ? "icon_image" : "icon_error";

            createTableLine();

            var CheckBox = new CheckBoxImageSwitch
            {
                Name = "cb" + cInfo.name,
                Checked = (_cInfo.picIndex == 0),   
                Tag = cInfo.parentName,
            };
            Panel.Controls.Add(CheckBox);

            var openBttn = new ButtonToOpen("open" + cInfo.name);
            new ToolTip().SetToolTip(openBttn, "Browse...");
            Panel.Controls.Add(openBttn);

            Panel.Tag = new tableRowTag { parentName = cInfo.parentName };

        }
    }
}
