using lightning_alert.data;
using lightning_alert.lightningalert;
using lightning_alert.lightningalert.interfaces;
using lightning_alert.model;
using lightning_alert.tilesystem;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace lightning_alert_tests
{
    public class ProcessLightningAlertDataTests
    {
        private static IConfiguration InitConfiguration()
        {
            var configPath = Environment.CurrentDirectory.Replace("bin\\Debug\\net6.0", "");
            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(configPath, "appsettings.test.json"))
                .Build();

            return config;
        }

        [Fact]
        public void Test_ReadLightningAlertData_AllDataValid()
        {
            #region "Arrange"
            var config = InitConfiguration();
            var inputDirectory = config!.GetSection("Input:Filepath").Value;
            var inputFilename = config.GetSection("Input:FileName_Successful_Test").Value;
            var inputFileFullPath = Path.Combine(inputDirectory, inputFilename);

            const int ZOOM_LEVEL = 12;
            const int EXPECTED_NUMBER_OF_VALID_DATA = 4;

            IProcessLightningAlertData processLightningAlertData = new ProcessLightningAlertData();
            processLightningAlertData.ReadLightningAlertData(inputFileFullPath, ZOOM_LEVEL);
            #endregion

            #region "Act"
            var result = processLightningAlertData.GetLightningAlertDataSuccessCount().Count;
            #endregion

            #region "Assert"
            Assert.Equal(EXPECTED_NUMBER_OF_VALID_DATA, result);
            #endregion
        }

        [Fact]
        public void Test_ReadLightningAlertData_AllDataInvalid()
        {
            #region "Arrange"
            var config = InitConfiguration();
            var inputDirectory = config!.GetSection("Input:Filepath").Value;
            var inputFilename = config.GetSection("Input:FileName_Invalid_Test").Value;
            var inputFileFullPath = Path.Combine(inputDirectory, inputFilename);

            const int ZOOM_LEVEL = 12;
            const int EXPECTED_NUMBER_OF_VALID_DATA = 0;

            IProcessLightningAlertData processLightningAlertData = new ProcessLightningAlertData();
            processLightningAlertData.ReadLightningAlertData(inputFileFullPath, ZOOM_LEVEL);
            #endregion

            #region "Act"
            var result = processLightningAlertData.GetLightningAlertDataSuccessCount().Count;
            #endregion

            #region "Assert"
            Assert.Equal(EXPECTED_NUMBER_OF_VALID_DATA, result);
            #endregion
        }

        [Fact]
        public void Test_ReadLightningAlertData_FlashTypeHeartbeat()
        {
            #region "Arrange"
            var config = InitConfiguration();
            var inputDirectory = config!.GetSection("Input:Filepath").Value;
            var inputFilename = config.GetSection("Input:FileName_AllHeartbeat_Test").Value;
            var inputFileFullPath = Path.Combine(inputDirectory, inputFilename);

            const int ZOOM_LEVEL = 12;
            const int EXPECTED_NUMBER_OF_VALID_DATA = 0;

            IProcessLightningAlertData processLightningAlertData = new ProcessLightningAlertData();
            processLightningAlertData.ReadLightningAlertData(inputFileFullPath, ZOOM_LEVEL);
            #endregion

            #region "Act"
            var result = processLightningAlertData.GetLightningAlertDataSuccessCount().Count;
            #endregion

            #region "Assert"
            Assert.Equal(EXPECTED_NUMBER_OF_VALID_DATA, result);
            #endregion
        }

        [Theory]
        [InlineData("023112320021", 32.9862554, -98.3471457)]
        [InlineData("023112320003", 32.9905308, -98.34038)]
        [InlineData("023112310322", 33.7412419, -96.6794229)]
        public void Test_ReadLightningAlertData_ValidQuadKey(string expectedQuadKey, 
            double latitude, double longitude)
        {
            #region "Arrange"
            const int ZOOM_LEVEL = 12;
            #endregion

            #region "Act"
            var result = TileSystem.GetQuadKey(longitude, latitude, ZOOM_LEVEL);
            #endregion

            #region "Assert"
            Assert.Equal(expectedQuadKey, result);
            #endregion
        }
    }
}