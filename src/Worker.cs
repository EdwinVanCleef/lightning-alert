using lightning_alert.lightningalert;
using lightning_alert.lightningalert.interfaces;

namespace lightning_alert
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker>? _logger;
        private readonly IConfiguration? _configuration;
        private readonly IProcessLightningAlertData _processLightningAlertData;

        private string? _inputFileFullPath;
        private string? _archiveFileFullPath;

        private readonly string? _zoomLevel;

        public Worker(ILogger<Worker>? logger, IConfiguration? configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _processLightningAlertData = new ProcessLightningAlertData();

            _zoomLevel = _configuration!.GetSection("ZoomLevel").Value;

            SetInputApplicationDirectoryPaths();
        }

        private void SetInputApplicationDirectoryPaths()
        {
            var inputDirectory = _configuration!.GetSection("Input:Filepath").Value;
            var inputFilename = _configuration.GetSection("Input:Filename").Value;
            _inputFileFullPath = Path.Combine(inputDirectory, inputFilename);

            if (!Directory.Exists(inputDirectory))
            {
                Directory.CreateDirectory(inputDirectory);
            }
        }

        private void SetArchiveApplicationDirectoryPaths()
        {
            var inputFilename = _configuration!.GetSection("Input:Filename").Value;
            var archiveDirectory = _configuration.GetSection("Archive:Filepath").Value;
            var archiveFilename = string.Format("Processed_{0}_{1}", DateTime.Now.ToString("MMddyyyyhhmmss"), inputFilename);
            _archiveFileFullPath = Path.Combine(archiveDirectory, archiveFilename);

            if (!Directory.Exists(archiveDirectory))
            {
                Directory.CreateDirectory(archiveDirectory);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger!.LogInformation("Lightning alert worker running at: {time}", DateTimeOffset.Now);

                if (!File.Exists(_inputFileFullPath))
                {
                    _logger!.LogInformation("No input file to process at: {time} Sleeping for 5 seconds", 
                        DateTimeOffset.Now);

                    await Task.Delay(5000, stoppingToken);
                    continue;
                }

                _logger!.LogInformation("Reading input file at: {time}", DateTimeOffset.Now);

                await _processLightningAlertData.ReadLightningAlertData(_inputFileFullPath, 
                    Convert.ToInt32(_zoomLevel));

                _logger!.LogInformation("Lightning data processing finished at: {time}", DateTimeOffset.Now);

                SetArchiveApplicationDirectoryPaths();
                File.Move(_inputFileFullPath, _archiveFileFullPath!);

                _logger!.LogInformation("Moved processed file to {archive} at: {time}", _archiveFileFullPath, 
                    DateTimeOffset.Now);

                _logger!.LogInformation("Waiting for Lightning data to process sleeping for 10 seconds");

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}