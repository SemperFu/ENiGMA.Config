using Hjson;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Terminal.Gui;

namespace ENiGMAConfig
{
    internal class Program
    {
        // private static object hson;

        private static ConfigHJSON MainConfig = new ConfigHJSON();
        private static FileInfo OpenedConfigFile;
        private static Toplevel top = Application.Top;
        private static Window win = new Window(new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), "ENiGMA½ v0.0.9-alpha Config Editor");
        private static Label LabelNew = new Label(2, 2, "Open or Create new file ");

        private static Label Footer = new Label(3, 24, "Press F9 (on Unix, ESC+9) to activate the menu");

        private static void Main(string[] args)
        {
            StartGUI();
        }

        private static void StartGUI()
        {
            Application.Init();

            // Creates the top-level window to show

            top.Add(win);

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_New", "Creates new file", NewFile),
                new MenuItem ("_Open", "Open existing file", Open),
                 new MenuItem ("_Close", "Close existing file", Close),
                new MenuItem ("_Save", "Save current file", Save),
                new MenuItem ("Save_As", "Saves new file", SaveAs),
                new MenuItem ("_Quit", "", () => { if (Quit ()) top.Running = false; })
            }),
            new MenuBarItem ("_Edit", new MenuItem [] {
                new MenuItem ("_Copy", "", null),
                new MenuItem ("C_ut", "", null),
                new MenuItem ("_Paste", "", null)
            }),
              new MenuBarItem ("_Help", new MenuItem [] {
                 new MenuItem ("_About", "", About),
            })
        });

            top.Add(menu);

            win.Add(LabelNew, Footer);

            Application.Run();
        }

        private static void About()
        {
            MessageBox.Query(60, 7, "About", "ddd", "ok");
        }

        private static void AddConfigMenu()
        {
            //win.Remove(LabelNew);
            // Add some controls
            //Label LabelBoardName = new Label("boardName: ") { X = 3, Y = 2 };
            Label LabelBoardName = new Label(3, 2, "BBS Name:");
            TextField TextfieldBoardName = new TextField(19, 2, 30, MainConfig.General.Qs("boardName").Replace("½", "")); //Crashes textfield. Bug already submitted
            TextfieldBoardName.Changed += new EventHandler(UpdateBBSName);

            Label LabelMenuFile = new Label(3, 4, "Menu File:");
            TextField TextFieldMenuFile = new TextField(19, 4, 30, MainConfig.General.Qs("menuFile"));
            TextFieldMenuFile.Changed += new EventHandler(UpdateMenuFile);
            Label LabelPromptFile = new Label(3, 5, "Prompt File:");
            TextField TextFieldPromptFile = new TextField(19, 5, 30, MainConfig.General.Qs("promptFile"));
            TextFieldPromptFile.Changed += new EventHandler(UpdatePromptFile);

            Label LabelDefaultTheme = new Label(3, 7, "Default Theme:");
            TextField TextFieldDefaultTheme = new TextField(19, 7, 30, MainConfig.Theme.Qs("default"));
            TextFieldDefaultTheme.Changed += new EventHandler(UpdateDefaultTheme);
            Label LabelPreLoginTheme = new Label(3, 8, "Prelogin Theme: ");
            TextField TextFieldPreLoginTheme = new TextField(19, 8, 30, MainConfig.Theme.Qs("preLogin"));
            TextFieldPreLoginTheme.Changed += new EventHandler(UpdatePreloginTheme);
            win.Add(LabelBoardName, TextfieldBoardName, LabelMenuFile, TextFieldMenuFile, LabelPromptFile, TextFieldPromptFile, LabelDefaultTheme, TextFieldDefaultTheme, LabelPreLoginTheme, TextFieldPreLoginTheme);

            FrameView FrameViewLoginServers = new FrameView(new Rect(1, 10, 62, 10), "Login Servers");

            CheckBox CheckBoxTelnet = new CheckBox(1, 0, "telnet");
            CheckBoxTelnet.Checked = MainConfig.LoginServersTelnet.Qb("enabled");
            CheckBoxTelnet.Toggled += new EventHandler(UpdateTelnetEnabled);
            Label LabelTelnetPort = new Label(17, 0, "port: ");
            TextField TextFieldTelnetPort = new TextField(24, 0, 5, MainConfig.LoginServersTelnet.Qstr("port"));
            TextFieldTelnetPort.Changed += new EventHandler(UpdateTelnetPort);

            CheckBox CheckBoxSSH = new CheckBox(1, 1, "ssh");
            CheckBoxSSH.Checked = MainConfig.LoginServersSSH.Qb("enabled");
            CheckBoxSSH.Toggled += new EventHandler(UpdateSSHEnabled);

            Label LabelSSHPort = new Label(17, 1, "port: ");
            TextField TextFieldSSHPort = new TextField(24, 1, 5, MainConfig.LoginServersSSH.Qstr("port"));
            TextFieldSSHPort.Changed += new EventHandler(UpdateSSHPort);

            Label LabelPrivateKeyPath = new Label(1, 3, "privateKeyPem: ");
            TextField TextFieldPrivateKeyPath = new TextField(17, 3, 42, MainConfig.LoginServersSSH.Qstr("privateKeyPem"));
            TextFieldPrivateKeyPath.Changed += new EventHandler(UpdatePrivateKeyPath);

            Label LabelPrivateKeyPass = new Label(1, 4, "privateKeyPass: ");

            TextField TextFieldPrivateKeyPass = new TextField(17, 4, 32, MainConfig.LoginServersSSH.Qstr("privateKeyPass"));
            TextFieldPrivateKeyPass.Secret = true;
            TextFieldPrivateKeyPass.Changed += new EventHandler(UpdatePrivateKeyPass);
            Button ButtonPKeyShow = new Button(51, 4, "Show");
            ButtonPKeyShow.Clicked = () => ButtonPKeyShow_Clicked(ButtonPKeyShow, TextFieldPrivateKeyPass);

            CheckBox CheckBoxWS = new CheckBox(1, 6, "ws");
            CheckBoxWS.Checked = MainConfig.LoginServersWSS.Qb("enabled");
            CheckBoxWS.Toggled += new EventHandler(UpdateWSEnabled);
            

            Label LabelWSPort = new Label(17, 6, "port: ");
            TextField TextFieldWSPort = new TextField(24, 6, 5, MainConfig.LoginServersWS.Qstr("port"));
            TextFieldWSPort.Changed += new EventHandler(UpdateWSPort);

            CheckBox CheckBoxWSS = new CheckBox(1, 7, "wss");
            CheckBoxWSS.Checked = MainConfig.LoginServersWSS.Qb("enabled");
            CheckBoxWSS.Toggled += new EventHandler(UpdateWSSEnabled);
            

            Label LabelWSSPort = new Label(17, 7, "port: ");
            TextField TextFieldWSSPort = new TextField(24, 7, 5, MainConfig.LoginServersWSS.Qstr("port"));
            TextFieldWSSPort.Changed += new EventHandler(UpdateWSSPort);

            FrameViewLoginServers.Add(CheckBoxTelnet, LabelTelnetPort, TextFieldTelnetPort, CheckBoxSSH, LabelSSHPort, TextFieldSSHPort, LabelPrivateKeyPath, TextFieldPrivateKeyPath, LabelPrivateKeyPass, ButtonPKeyShow, TextFieldPrivateKeyPass, CheckBoxWS, LabelWSPort, TextFieldWSPort, CheckBoxWSS, LabelWSSPort, TextFieldWSSPort);

            FrameView FrameViewLogging = new FrameView(new Rect(64, 0, 53, 10), "Logging");

            //new CheckBox (1, 0, "Remember me"),
            Label LabelFilename = new Label(2, 1, "Filename: ");
            TextField TextFieldFileame = new TextField(13, 1, 22, MainConfig.Logging.Qs("fileName"));
            TextFieldFileame.Changed += new EventHandler(UpdateLogFileame);
            Label LabelLogPath = new Label(2, 2, "Path: ");
            TextField TextFieldLogPath = new TextField(13, 2, 22, MainConfig.Paths.Qs("logs"));
            TextFieldLogPath.Changed += new EventHandler(UpdateLogPath);
            Label LabelLogLevel = new Label(39, 0, "Level: ");
            RadioGroup RadioGroupDebug = new RadioGroup(39, 2, new[] { "_Error", "_Warn", "_Info", "_Debug", "_Trace" });

            switch (MainConfig.Logging.Qs("level"))
            {
                case "error":
                    {
                        RadioGroupDebug.Selected = 0;
                        break;
                    }
                case "warn":
                    {
                        RadioGroupDebug.Selected = 1;
                        break;
                    }
                case "info":
                    {
                        RadioGroupDebug.Selected = 2;
                        break;
                    }
                case "debug":
                    {
                        RadioGroupDebug.Selected = 3;
                        break;
                    }
                case "trace":
                    {
                        RadioGroupDebug.Selected = 4;
                        break;
                    }
                default: //If missing or invalid - Set to info.
                    {
                        RadioGroupDebug.Selected = 2;
                        break;
                    }
            }

            RadioGroupDebug.SelectionChanged += (int x) => UpdateLogLevel(x);
            FrameViewLogging.Add(LabelFilename, TextFieldFileame, LabelLogPath, TextFieldLogPath, LabelLogLevel, RadioGroupDebug);
            win.Add(FrameViewLoginServers, FrameViewLogging);

            //Email

            FrameView FrameViewEmail = new FrameView(new Rect(64, 10, 53, 10), "Email");

            //new CheckBox (1, 0, "Remember me"),
            Label LabelEmailTransport = new Label(1, 0, "[Transport] ");
            Label LabelEmailFrom = new Label(8, 1, "From: ");
            TextField TextFieldEmailFrom = new TextField(15, 1, 25, MainConfig.Email.Qs("defaultFrom"));
            TextFieldEmailFrom.Changed += new EventHandler(UpdateEmailFrom);

            Label LabelEmailHost = new Label(8, 2, "Host: ");
            TextField TextFieldEmailHost = new TextField(15, 2, 25, MainConfig.EmailTransport.Qs("host"));
            TextFieldEmailHost.Changed += new EventHandler(UpdateEmailHost);
            Label LabelEmailPort = new Label(8, 3, "Port: ");
            TextField TextFieldEmailPort = new TextField(15, 3, 6, MainConfig.EmailTransport.Qstr("port"));
            TextFieldEmailPort.Changed += new EventHandler(UpdateEmailPort);
            CheckBox CheckBoxEmailSecure = new CheckBox(22, 3, "secure");
            CheckBoxEmailSecure.Checked = MainConfig.EmailTransport.Qb("secure");
            CheckBoxEmailSecure.Toggled += new EventHandler(UpdateWSSEnabled);

            Label LabelEmailAuth = new Label(1, 4, "[Auth] ");
            Label LabelEmailAuthUser = new Label(8, 5, "User: ");
            TextField TextFieldEmailAuthUser = new TextField(15, 5, 25, MainConfig.Paths.Qs("logs"));
            TextFieldEmailAuthUser.Changed += new EventHandler(UpdateEmailAuthUser);
            Label LabelEmailAuthPass = new Label(8, 6, "Pass: ");
            TextField TextFieldEmailAuthPass = new TextField(15, 6, 25, MainConfig.Paths.Qs("logs"));
            TextFieldEmailAuthPass.Secret = true;
            TextFieldEmailAuthPass.Changed += new EventHandler(UpdateEmailAuthPass);
            Button ButtonEKeyShow = new Button(41, 6, "Show");
            ButtonEKeyShow.Clicked = () => ButtonEKeyShow_Clicked(ButtonEKeyShow, TextFieldEmailAuthPass);

            FrameViewEmail.Add(LabelEmailTransport, LabelEmailAuth, LabelEmailFrom, TextFieldEmailFrom, LabelEmailHost, TextFieldEmailHost, LabelEmailPort, TextFieldEmailPort, CheckBoxEmailSecure, LabelEmailAuthUser, TextFieldEmailAuthUser, TextFieldEmailAuthUser, LabelEmailAuthPass, TextFieldEmailAuthPass, ButtonEKeyShow);
            win.Add(FrameViewEmail, Footer);
        }

        private static void UpdateLogPath(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.Paths["logs"] = TF.Text.ToString();
        }

        private static void UpdateLogFileame(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.Logging["fileName"] = TF.Text.ToString();
        }

        private static void UpdateWSPort(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;

            string PortString = TF.Text.ToString();
            string result = Regex.Replace(PortString, @"[^\d]", "");

            //  if (PortString != result) TF.Text = result; //Comment out for now, causes bug

            int ParsedPort = 8810; //default port

            if (Int32.TryParse(result, out ParsedPort))
            {
                MainConfig.LoginServersWS["port"] = ParsedPort;
            }
        }

        private static void UpdateWSSPort(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;

            string PortString = TF.Text.ToString();
            string result = Regex.Replace(PortString, @"[^\d]", "");

            //  if (PortString != result) TF.Text = result; //Comment out for now, causes bug

            int ParsedPort = 8811; //default port

            if (Int32.TryParse(result, out ParsedPort))
            {
                MainConfig.LoginServersWSS["port"] = ParsedPort;
            }
        }

        private static void UpdateEmailFrom(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.Email["defaultFrom"] = TF.Text.ToString();
        }

        private static void UpdateEmailHost(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.EmailTransport["host"] = TF.Text.ToString();
        }

        private static void UpdateEmailPort(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;

            string PortString = TF.Text.ToString();
            string result = Regex.Replace(PortString, @"[^\d]", "");

            //  if (PortString != result) TF.Text = result; //Comment out for now, causes bug

            int ParsedPort = 8810; //default port

            if (Int32.TryParse(result, out ParsedPort))
            {
                MainConfig.EmailTransport["port"] = ParsedPort;
            }
        }

        private static void UpdateEmailAuthPass(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.EmailAuth["pass"] = TF.Text.ToString();
        }

        private static void UpdateEmailAuthUser(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.EmailAuth["user"] = TF.Text.ToString();
        }

        private static void UpdatePrivateKeyPath(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.LoginServersSSH["privateKeyPem"] = TF.Text.ToString();
        }

        private static void UpdatePrivateKeyPass(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.LoginServersSSH["privateKeyPass"] = TF.Text.ToString();
        }

        private static void UpdatePreloginTheme(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.Theme["preLogin"] = TF.Text.ToString();
        }

        private static void UpdateDefaultTheme(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.Theme["default"] = TF.Text.ToString();
        }

        private static void UpdatePromptFile(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.General["promptFile"] = TF.Text.ToString();
        }

        private static void UpdateMenuFile(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.General["menuFile"] = TF.Text.ToString();
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

        private static void UpdateLoggingLevel(int Level)
        {
        }

        private static void UpdateSSHEnabled(object sender, EventArgs e)
        {
            CheckBox SSH = (CheckBox)sender;
            MainConfig.LoginServersSSH["enabled"] = SSH.Checked;
        }

        private static void UpdateTelnetEnabled(object sender, EventArgs e)
        {
            CheckBox Telnet = (CheckBox)sender;
            MainConfig.LoginServersTelnet["enabled"] = Telnet.Checked;
        }

        private static void UpdateWSEnabled(object sender, EventArgs e)
        {
            CheckBox WS = (CheckBox)sender;
            MainConfig.LoginServersWS["enabled"] = WS.Checked;
        }

        private static void UpdateWSSEnabled(object sender, EventArgs e)
        {
            CheckBox WSS = (CheckBox)sender;
            MainConfig.LoginServersWSS["enabled"] = WSS.Checked;
        }

        private static void UpdateSSHPort(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;

            string PortString = TF.Text.ToString();
            string result = Regex.Replace(PortString, @"[^\d]", "");

            //  if (PortString != result) TF.Text = result; //Comment out for now, causes bug

            int ParsedPort = 8889; //default port

            if (Int32.TryParse(result, out ParsedPort))
            {
                MainConfig.LoginServersSSH["port"] = ParsedPort;
            }
        }

        private static void UpdateTelnetPort(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;

            string PortString = TF.Text.ToString();
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

        private static void Save()
        {
            SaveJson(OpenedConfigFile.FullName);
        }

        private static void SaveAs()
        {
            SaveDialog SaveD = new SaveDialog("Save As", "Save as file");
            SaveD.DirectoryPath = Directory.GetCurrentDirectory(); //Use current path for now. Switch to default config

            Application.Run(SaveD);
            // SaveD.
            if (SaveD.FileName == null) return; //No file selected - return
            string Filename = SaveD.FileName.ToString();
            if (string.IsNullOrWhiteSpace(Filename))
            {
                return;
            }

            string FullSavePath = Path.Combine(SaveD.DirectoryPath.ToString(), Filename);
            SaveJson(FullSavePath);
        }

        private static void Open()
        {
            OpenDialog OpenD = new OpenDialog("Open", "Open a file");
            OpenD.DirectoryPath = OpenD.DirectoryPath = Directory.GetCurrentDirectory();
            OpenD.AllowsMultipleSelection = false;
            //System.Runtime.InteropServices.RuntimeInformation
            Application.Run(OpenD);

            if (OpenD.FilePaths.Count == 0) return; //No file selected - return
            OpenedConfigFile = new FileInfo(OpenD.FilePaths[0]);
            if (OpenedConfigFile.Exists)
            {
                MessageBox.Query(60, 7, "Selected File", string.Join(", ", OpenD.FilePaths), "Ok");
                ClearViews();
                LoadJson(OpenedConfigFile.FullName);
            }
            else
            {
                MessageBox.Query(60, 7, "File not found", "Please try again", "Ok");
            }
        }

        private static bool Quit()
        {
            var n = MessageBox.Query(50, 7, "Quit Demo", "Are you sure you want to quit this demo?", "Yes", "No");

            return n == 0;
        }

        private static void ClearViews()
        {
            if (win.Subviews[0].Subviews.Count > 0)
            {
                win.Subviews[0].RemoveAll();
            }
        }

        private static void Close()

        {
            ClearViews();

            win.Add(LabelNew, Footer);
        }

        private static void NewFile()
        {
            HjsonOptions ImportOptions = new HjsonOptions();
            ImportOptions.KeepWsc = true;
            TextReader TextReaderNew = new StringReader(Properties.Resources.DefaultConfigFile);

            MainConfig.Mainfile = HjsonValue.Load(TextReaderNew, ImportOptions);
            ClearViews();
            ProcessConfig();
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

                AddConfigMenu();
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