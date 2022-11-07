using lightning_alert.lightningalert;
using lightning_alert.lightningalert.interfaces;
using Microsoft.Extensions.Configuration;

namespace lightning_alert_tests
{
    public class ProcessLightningAlertDataTests
    {
        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            return config;
        }

        [Fact]
        public void Test_ReadLightningAlertData_ShowCorrectConsoleMessage()
        {
            #region "Arrange"
            var config = InitConfiguration();
            var inputDirectory = config!.GetSection("Input:Filepath").Value;
            var inputFilename = config.GetSection("Input:Filename").Value;
            var inputFileFullPath = Path.Combine(inputDirectory, inputFilename);
            const int ZOOM_LEVEL = 12; 

            IProcessLightningAlertData processLightningAlertData = new ProcessLightningAlertData();
            #endregion

            #region "Act"
            processLightningAlertData.ReadLightningAlertData(inputFileFullPath, ZOOM_LEVEL);
            #endregion

            #region "Assert"
            // continue later https://makolyte.com/csharp-how-to-unit-test-code-that-reads-and-writes-to-the-console/
            #endregion
        }
    }
}