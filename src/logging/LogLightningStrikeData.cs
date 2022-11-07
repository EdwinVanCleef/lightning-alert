using lightning_alert.logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lightning_alert.logging
{
    internal sealed class LogLightningStrikeData : ILogLightningStrikeData
    {
        private readonly string _logFilePath;

        public LogLightningStrikeData(string logFilePath)
        {
            _logFilePath = logFilePath; 
        }

        public void WriteInvalidLightningStrikeData(List<string> invalidDataList)
        {
            var fileName = string.Format("{0}_Invalid_Data.log",
                DateTime.Now.ToString("MMddyyyyhhmmss"));

            WriteLogData(fileName, invalidDataList);
        }

        public void WriteValidLightningStrikeData(List<string> notifiedList)
        {
            var fileName = string.Format("{0}_Alerted_LightningStrikes.log",
                DateTime.Now.ToString("MMddyyyyhhmmss"));

            WriteLogData(fileName, notifiedList);
        }

        private void WriteLogData(string filename, List<string> list)
        {
            using var writer = new StreamWriter(Path.Combine(_logFilePath, filename));
            foreach (var info in list)
            {
                writer.WriteLine(info);
            }
        }
    }
}
