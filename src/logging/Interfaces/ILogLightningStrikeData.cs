using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lightning_alert.logging.Interfaces
{
    internal interface ILogLightningStrikeData
    {
        void WriteValidLightningStrikeData(List<string> notifiedList);
        void WriteInvalidLightningStrikeData(List<string> invalidDataList);

    }
}
