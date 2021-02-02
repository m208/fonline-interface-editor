
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
            createTableLine();

            Button hideBttn = new ButtonToHide("hide" + cInfo.name);
            new ToolTip().SetToolTip(hideBttn, "Show/ hide element");
            this.Controls.Add(hideBttn);

            Button zBttn = new ButtonToZ("zBttn" + cInfo.name);
            new ToolTip().SetToolTip(zBttn, "Bring to front/ send to back");
            this.Controls.Add(zBttn);
            
            this.Tag = new tableRowTag { parentName = cInfo.name };
        }

    }
}
