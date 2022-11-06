using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lightning_alert.lightningalert.interfaces
{
    internal interface IProcessLightningAlertData
    {
        Task ReadLightningAlertData(string filePath, int zoomLevel);
    }
}
