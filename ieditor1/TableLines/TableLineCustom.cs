using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOIE.TableLines
{
    class TableLineCustom: TableLine
    {
        
        public TableLineCustom(ControlInfo _cInfo)
        {
            controlTypeImg = "typography";
            cInfo = _cInfo;
            createTableLine();
        }
        
    }
}
