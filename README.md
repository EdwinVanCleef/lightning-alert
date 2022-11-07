# Lightning Alert
Reads lightning events data as a stream from standard input (one lightning strike per line as a JSON object, and matches that data against a source of assets (also in JSON format) to produce an alert.

This application will translate the data stored in the input directory and will show valid lightning alerts in the console. 
After each input file processing, the input file will be transferred to the archive directory.

Archived Filename Convention: Processed_Datetime MMddyyyyhhmmss_Filename of the input file

```
Processed_11062022092211_lightning.json
```

For each valid lightning strike data, the application will show a message to the console:

```
lightning alert for 6720:Dante Street
```

The application will only show lightning alerts if:
1. The FlashType value is either 0 or 1
2. The converted quadkey from longitude and latitude is found in Assets.json
3. No duplicate alerts should be shown

## Prerequisites 
Before starting, please install the following:
1. Install Visual Studio 2019 or 2022 in your machine.
2. Install .NetCore 6.x

Unit Tests setup:
1. Add XUnit package

## Getting Started
1. Clone the repo into a new directory: git clone git@github
2. Prepare your copy of Assets.json and lightning.json. 
   Assets.json contains the lightning data in quadkey format, while the lightning.json contains the lightning data in longitude and latitude format.
3. Put the Assets.json in \lightning-alert\src\data directory. 
4. Put the lightning.json data in lightning-alert\input directory.
5. Open the appsettings.json file and update the file paths based on where you cloned the application.

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "Input": {
    "Filepath": "\\lightning-alert\\input", -> Update this to your input directory path
    "FileName": "lightning.json"
  },
  "Archive": {
    "Filepath": "\\lightning-alert\\archive" -> Update this to your archive directory path
  },
  "ZoomLevel": 12,
  "LogFilePath": "\\lightning-alert\\logging" -> Update this to your logging directory path
}
```
6. You may now run the application
7. You can check the log files located in the LogFilePath directory that you configured in the appsettings.json file

## Running the tests
1. Open the appsettings.test.json file and update the file paths based on where you cloned the application.

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "Input": {
    "Filepath": "\\lightning-alert\\input", -> Update this to your input directory path
    "FileName": "lightning.json"
  },
  "Archive": {
    "Filepath": "\\lightning-alert\\archive" -> Update this to your archive directory path
  },
  "ZoomLevel": 12,
  "LogFilePath": "\\lightning-alert\\logging" -> Update this to your logging directory path
}
```
2. Open Visual Studio 
3. Open Test Explorer. Test > Test Explorer
4. Press the Run Button or Right click on the test header and select Run

## Logging
You can locate the log files on this directory: lightning-alert\logging
There are 2 types of logs:
1. Alerted Lightning Strikes - logs all valid alerted Lightning Strikes. 
2. Invalid Data - Logs all data with flash type not equal to 0 or 1.

## Built With
.Net Core 6
C#
XUnit for unit testing

## Authors
* **Joel Carlo Menor** 

## Acknowledgments
* Hat tip to DTN software engineering team for this fun and challenging task.
