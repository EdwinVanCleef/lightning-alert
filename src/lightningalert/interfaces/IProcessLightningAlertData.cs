using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lightning_alert.lightningalert.interfaces
{
    public interface IProcessLightningAlertData
    {
        Task ReadLightningAlertData(string filePath, int zoomLevel);
        List<string> GetLightningAlertDataSuccessCount();
    }
}
