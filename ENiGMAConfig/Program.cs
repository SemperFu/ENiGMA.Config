using Hjson;
using NStack;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Terminal.Gui;

namespace ENiGMAConfig
{
    partial class Program
    {
        // private static object hson;

        private static void Main(string[] args)
        {
            StartGUI();
        }

        private static void UpdateLogPath(ustring e)
        {
            MainConfig.Paths["logs"] = e.ToString();
        }

        private static void UpdateLogFileame(ustring e)
        {
            MainConfig.Logging["fileName"] = e.ToString();
        }

        private static void UpdateWSPort(ustring e)
        {
            string PortString = e.ToString();
            string result = Regex.Replace(PortString, @"[^\d]", "");

            //  if (PortString != result) TF.Text = result; //Comment out for now, causes bug

            int ParsedPort = 8810; //default port

            if (Int32.TryParse(result, out ParsedPort))
            {
                MainConfig.LoginServersWS["port"] = ParsedPort;
            }
        }

        private static void UpdateWSSPort(ustring e)
        {
            string PortString = e.ToString();
            string result = Regex.Replace(PortString, @"[^\d]", "");

            //  if (PortString != result) TF.Text = result; //Comment out for now, causes bug

            int ParsedPort = 8811; //default port

            if (Int32.TryParse(result, out ParsedPort))
            {
                MainConfig.LoginServersWSS["port"] = ParsedPort;
            }
        }

        private static void UpdateEmailFrom(ustring e)
        {
            MainConfig.Email["defaultFrom"] = e.ToString();
        }

        private static void UpdateEmailHost(ustring e)
        {
            MainConfig.EmailTransport["host"] = e.ToString();
        }

        private static void UpdateEmailPort(ustring e)
        {
            string PortString = e.ToString();
            string result = Regex.Replace(PortString, @"[^\d]", "");

            //  if (PortString != result) TF.Text = result; //Comment out for now, causes bug

            int ParsedPort = 8810; //default port

            if (Int32.TryParse(result, out ParsedPort))
            {
                MainConfig.EmailTransport["port"] = ParsedPort;
            }
        }

        private static void UpdateEmailAuthPass(ustring e)
        {
            MainConfig.EmailAuth["pass"] = e.ToString();
        }

        private static void UpdateEmailAuthUser(ustring e)
        {
            MainConfig.EmailAuth["user"] = e.ToString();
        }

        private static void UpdatePrivateKeyPath(ustring e)
        {
            MainConfig.LoginServersSSH["privateKeyPem"] = e.ToString();
        }

        private static void UpdatePrivateKeyPass(ustring e)
        {
            MainConfig.LoginServersSSH["privateKeyPass"] = e.ToString();
        }

        private static void UpdatePreloginTheme(ustring e)
        {
            MainConfig.Theme["preLogin"] = e.ToString();
        }

        private static void UpdateDefaultTheme(NStack.ustring e)
        {
            MainConfig.Theme["default"] = e.ToString();
        }

        private static void UpdatePromptFile(NStack.ustring e)
        {
            MainConfig.General["promptFile"] = e.ToString();
        }

        private static void UpdateMenuFile(NStack.ustring e)
        {
            string f = e.ToString();
            MainConfig.General["menuFile"] = f;
        }

        private static void ButtonEKeyShow_Clicked(Button buttonEKeyShow, TextField textFieldEmailAuthPass)
        {
            if (textFieldEmailAuthPass.Secret)
            {
                textFieldEmailAuthPass.Secret = false;
                buttonEKeyShow.Text = "Hide";
            }
            else
            {
                textFieldEmailAuthPass.Secret = true;
                buttonEKeyShow.Text = "Show";
            }
        }

        private static void ButtonPKeyShow_Clicked(Button ButtonPKeyShow, TextField TextFieldPrivateKeyPass)
        {
            if (TextFieldPrivateKeyPass.Secret)
            {
                TextFieldPrivateKeyPass.Secret = false;
                ButtonPKeyShow.Text = "Hide";
            }
            else
            {
                TextFieldPrivateKeyPass.Secret = true;
                ButtonPKeyShow.Text = "Show";
            }
        }

        private static void UpdateLogLevel(int Selected)
        {
            // "_Error", "_Warn", "_Info", "_Debug", "_Trace"
            switch (Selected)
            {
                case 0:
                    MainConfig.Logging["level"] = "error";
                    break;

                case 1:
                    MainConfig.Logging["level"] = "warn";
                    break;

                case 2:
                    MainConfig.Logging["level"] = "info";
                    break;

                case 3:
                    MainConfig.Logging["level"] = "debug";
                    break;

                case 4:
                    MainConfig.Logging["level"] = "trace";
                    break;

                default: //If missing or invalid - Set to info.

                    MainConfig.Logging["level"] = "info";
                    break;
            }
        }

        private static void UpdateLogLevel(RadioGroup.SelectedItemChangedArgs selectedItemChangedArgs)
        {
            // "_Error", "_Warn", "_Info", "_Debug", "_Trace"
            switch (selectedItemChangedArgs.SelectedItem)
            {
                case 0:
                    MainConfig.Logging["level"] = "error";
                    break;

                case 1:
                    MainConfig.Logging["level"] = "warn";
                    break;

                case 2:
                    MainConfig.Logging["level"] = "info";
                    break;

                case 3:
                    MainConfig.Logging["level"] = "debug";
                    break;

                case 4:
                    MainConfig.Logging["level"] = "trace";
                    break;

                default: //If missing or invalid - Set to info.

                    MainConfig.Logging["level"] = "info";
                    break;
            }
        }

        private static void UpdateLoggingLevel(int Level)
        {
        }

        private static void UpdateSSHEnabled(bool SSHChecked)
        {
            MainConfig.LoginServersSSH["enabled"] = SSHChecked;
        }

        private static void UpdateTelnetEnabled(bool TelnetChecked)
        {
            MainConfig.LoginServersTelnet["enabled"] = TelnetChecked;
        }

        private static void UpdateWSEnabled(bool WSChecked)
        {
            MainConfig.LoginServersWS["enabled"] = WSChecked;
        }

        private static void UpdateWSSEnabled(bool WSSChecked)
        {
            MainConfig.LoginServersWSS["enabled"] = WSSChecked;
        }

        private static void UpdateSSHPort(ustring e)
        {
            string PortString = e.ToString();
            string result = Regex.Replace(PortString, @"[^\d]", "");

            //  if (PortString != result) TF.Text = result; //Comment out for now, causes bug

            int ParsedPort = 8889; //default port

            if (Int32.TryParse(result, out ParsedPort))
            {
                MainConfig.LoginServersSSH["port"] = ParsedPort;
            }
        }

        private static void UpdateTelnetPort(ustring e)
        {
            string PortString = e.ToString();
            string result = Regex.Replace(PortString, @"[^\d]", "");

            //  if (PortString != result) TF.Text = result; //Comment out for now, causes bug

            int ParsedPort = 8888; //default port

            if (Int32.TryParse(result, out ParsedPort))
            {
                MainConfig.LoginServersTelnet["port"] = ParsedPort;
            }
        }

        private static void UpdateBBSName(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;

            //Name validation if needed
            string f = TF.Text.ToString();

            MainConfig.General["boardName"] = f;
            //  TF.Text = "UpdateCheck";
        }

        private static void UpdateBBSName(NStack.ustring e)
        {
            //Name validation if needed
            string f = e.ToString();

            MainConfig.General["boardName"] = f;
            //  TF.Text = "UpdateCheck";
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
            HjsonOptions ImportOptions = new HjsonOptions();
            ImportOptions.KeepWsc = true;

            MainConfig.Mainfile = HjsonValue.Load(FilePath, ImportOptions);
            ProcessConfig(); //File Loaded - Process it.
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
                MainConfig.EmailAuth = MainConfig.EmailTransport.Qo("auth");
                MainConfig.contentServers = MainConfig.MainObjects.Qo("contentServers");
                MainConfig.messageConferences = MainConfig.MainObjects.Qo("messageConferences");
                MainConfig.messageNetworks = MainConfig.MainObjects.Qo("messageNetworks");
                MainConfig.fileBase = MainConfig.MainObjects.Qo("fileBase");
                MainConfig.scannerTossers = MainConfig.MainObjects.Qo("scannerTossers");
                MainConfig.archivers = MainConfig.MainObjects.Qo("archives");

                // AddConfigMenu();
            }
            catch
            {
                MessageBox.Query(40, 7, "Error", "Error Prossing HJSON file.\n Please ensure this is a proper Enigma Config", "Ok");
                ClearViews();

                win.Add(LabelNew, Footer);
            }
        }

        private static void SaveJson(string FilePath)
        {
            HjsonOptions FileOptions = new HjsonOptions();
            FileOptions.KeepWsc = true;

            FileInfo SavePath = new FileInfo(FilePath);
            if (SavePath.Exists) //Exists - Lets Backup
            {
                DirectoryInfo BackupDir = new DirectoryInfo(Path.Combine(SavePath.Directory.FullName, "Backups"));
                if (!BackupDir.Exists) BackupDir.Create();
                DateTime CurrentTime = DateTime.Now;
                string BackupFilename = SavePath.Name + "." + CurrentTime.Year.ToString() + "." + CurrentTime.Month.ToString() + "." + CurrentTime.Day.ToString() + "." + CurrentTime.Minute.ToString() + "." + CurrentTime.Second.ToString() + ".bak";

                string BackupPath = Path.Combine(BackupDir.FullName, BackupFilename);
                SavePath.CopyTo(BackupPath);
            }

            HjsonValue.Save(MainConfig.Mainfile, FilePath, FileOptions);
        }
    }
}