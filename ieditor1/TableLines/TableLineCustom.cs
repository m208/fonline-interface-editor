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
            cInfo = _cInfo;
            createTableLine();
        }
        
    }
}
