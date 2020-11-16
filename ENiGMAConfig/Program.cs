using Hjson;
using Newtonsoft.Json;
using System;
using System.IO;

//using Newtonsoft.Json;
using Terminal.Gui;

namespace ENiGMAConfig
{
    partial class Program
    {
        private static ConfigHJSON MainConfig = new();
        // private static object hson;

        private static void Main(string[] args)
        {
            StartGUI();
        }

        private static void ClearViews()
        {
            if (win.Subviews[0].Subviews.Count > 0)
            {
                win.Subviews[0].RemoveAll();
            }
        }

        private static void LoadJson(string FilePath)
        {
            HjsonOptions ImportOptions = new(){KeepWsc = true};

            MainConfig.Mainfile = HjsonValue.Load(FilePath, ImportOptions);
            JsonValue LoadedJSON = HjsonValue.Load(FilePath);

            string JSONString = LoadedJSON.ToString(Stringify.Plain);

            MainConfig.Deserialized = JsonConvert.DeserializeObject<EnigmaConfigSchema>(JSONString);

            ProcessConfig(); //File Loaded - Process it.
        }

        private static void SaveJson(string FilePath)
        {
          
            FileInfo SavePath = new(FilePath);
            if (SavePath.Exists) //Exists - Lets Backup
            {
                DirectoryInfo BackupDir = new(Path.Combine(SavePath.Directory.FullName, "Backups"));
                if (!BackupDir.Exists) BackupDir.Create();
                DateTime CurrentTime = DateTime.Now;
                string BackupFilename = SavePath.Name + "." + CurrentTime.Year.ToString() + "." + CurrentTime.Month.ToString() + "." + CurrentTime.Day.ToString() + "." + CurrentTime.Minute.ToString() + "." + CurrentTime.Second.ToString() + ".bak";

                string BackupPath = Path.Combine(BackupDir.FullName, BackupFilename);
                SavePath.CopyTo(BackupPath);
            }

            HjsonOptions FileOptions = new() { KeepWsc = true };
            HjsonValue.Save(MainConfig.Mainfile, FilePath, FileOptions);
        }

        private static void ProcessConfig()
        {
            try
            {
                win.Remove(LabelNew);
                MainConfig.MainObjects = MainConfig.Mainfile.Qo();
                MainConfig.General = MainConfig.MainObjects.Qo("general");
                MainConfig.Paths = MainConfig.MainObjects.Qo("paths");
                MainConfig.Logging = MainConfig.MainObjects.Qo("logging").Qo("rotatingFile");
                MainConfig.Theme = MainConfig.MainObjects.Qo("theme");
                MainConfig.LoginServers = MainConfig.MainObjects.Qo("loginServers");
                MainConfig.LoginServersTelnet = MainConfig.LoginServers.Qo("telnet");
                MainConfig.LoginServersSSH = MainConfig.LoginServers.Qo("ssh");
                MainConfig.LoginServersWebSocket = MainConfig.LoginServers.Qo("webSocket");
                MainConfig.LoginServersWS = MainConfig.LoginServersWebSocket.Qo("ws");
                MainConfig.LoginServersWSS = MainConfig.LoginServersWebSocket.Qo("wss");
                MainConfig.Email = MainConfig.MainObjects.Qo("email");
                MainConfig.EmailTransport = MainConfig.Email.Qo("transport");
                if (MainConfig.EmailTransport is not null) MainConfig.EmailAuth = MainConfig.EmailTransport.Qo("auth");
                MainConfig.contentServers = MainConfig.MainObjects.Qo("contentServers");
                MainConfig.messageConferences = MainConfig.MainObjects.Qo("messageConferences");
                MainConfig.messageNetworks = MainConfig.MainObjects.Qo("messageNetworks");
                MainConfig.fileBase = MainConfig.MainObjects.Qo("fileBase");
                MainConfig.scannerTossers = MainConfig.MainObjects.Qo("scannerTossers");
                MainConfig.archivers = MainConfig.MainObjects.Qo("archives");

                AddConfigMenu();
            }
            catch
            {
                MessageBox.Query(40, 7, "Error", "Error Prossing HJSON file.\n Please ensure this is a proper Enigma Config", "Ok");
                ClearViews();

                win.Add(LabelNew, Footer);
            }
        }
    }
}