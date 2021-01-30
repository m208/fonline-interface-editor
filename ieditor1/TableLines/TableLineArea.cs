using FOIE.Controls;
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

            Button hideBttn = new ButtonToHide("hide" + cInfo.name);
            new ToolTip().SetToolTip(hideBttn, "Show/ hide element");
            Panel.Controls.Add(hideBttn);

            Button zBttn = new ButtonToZ("zBttn" + cInfo.name);
            new ToolTip().SetToolTip(zBttn, "Bring to front/ send to back");
            Panel.Controls.Add(zBttn);
            
            Panel.Tag = new tableRowTag { parentName = cInfo.name };
        }

    }
}
