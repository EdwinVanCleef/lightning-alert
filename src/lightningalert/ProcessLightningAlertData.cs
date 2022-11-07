using lightning_alert.tilesystem;
using lightning_alert.model;
using lightning_alert.lightningalert.interfaces;
using Newtonsoft.Json;
using lightning_alert.data;
using lightning_alert.logging.Interfaces;
using lightning_alert.logging;

namespace lightning_alert.lightningalert
{
    public sealed class ProcessLightningAlertData : IProcessLightningAlertData
    {
        private List<string> _quadKeyListNotified;
        private List<string> _notifiedList;
        private List<string> _invalidDataList;
        private readonly ILogLightningStrikeData? _logLightningStrikeData;

        public ProcessLightningAlertData(string logFilePath)
        {
            _quadKeyListNotified = new List<string>();
            _notifiedList = new List<string>();
            _invalidDataList = new List<string>();
            _logLightningStrikeData = new LogLightningStrikeData(logFilePath);
        }

        public Task ReadLightningAlertData(string filePath, int zoomLevel)
        {
            using (var reader = new StreamReader(filePath))
            {
                var line = string.Empty;

                _notifiedList = new List<string>();
                _invalidDataList = new List<string>();
                _quadKeyListNotified = new List<string>();

                while ((line = reader.ReadLine()) != null)
                {
                    var lightningData = JsonConvert.DeserializeObject<LightningStrikeData>(line);

                    if (lightningData!.FlashType != Convert.ToInt32(FlashTypes.CloudToCloud) &&
                        lightningData.FlashType != Convert.ToInt32(FlashTypes.CloudToGround))
                    {
                        _invalidDataList.Add("[Invalid FlashType] " + line);

                        continue;
                    }

                    var quadKey = TileSystem.GetQuadKey(lightningData!.Longitude, lightningData.Latitude,
                        zoomLevel);
                    var isQuadKeyExists = _quadKeyListNotified.Where(x => x.Equals(quadKey)).Any();

                    if (CheckQuadKeyIfInAsset(quadKey, isQuadKeyExists) &&
                        !isQuadKeyExists)
                    {
                        _quadKeyListNotified.Add(quadKey);
                    }
                }

                if (_notifiedList.Count > 0)
                {
                    _logLightningStrikeData!.WriteValidLightningStrikeData(_notifiedList);
                }

                if (_invalidDataList.Count > 0)
                {
                    _logLightningStrikeData!.WriteInvalidLightningStrikeData(_invalidDataList);
                }
            }

            return Task.CompletedTask;
        }

        public List<string> GetLightningAlertDataSuccessCount()
        {
            return _quadKeyListNotified;
        }

        private bool CheckQuadKeyIfInAsset(string quadKey, bool isQuadKeyExists)
        {
            string basePath = Environment.CurrentDirectory;
            string assetsPath = Path.Combine(basePath, @"data\assets.json");

            using var reader = new StreamReader(assetsPath);
            var json = reader.ReadToEnd();
            var assets = JsonConvert.DeserializeObject<List<LightningAssetsData>>(json);
            var assetExists = assets!.Where(x => x.QuadKey == quadKey)
                .Select(a => new LightningAssetsData
                {
                    AssetName = a.AssetName,
                    AssetOwner = a.AssetOwner
                });

            if (assetExists.Any() && !isQuadKeyExists)
            {
                var notification = string.Format("lightning alert for {0}:{1}",
                    assetExists.First().AssetOwner,
                    assetExists.First().AssetName);

                Console.WriteLine(notification);
                _notifiedList.Add(notification);
            }

            return assetExists.Any();
        }

    }
}
