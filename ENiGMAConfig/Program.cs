using Hjson;
using System;
using System.IO;
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
            Label LabelBoardName = new Label(3, 2, "BBS Name");
            TextField TextfieldBoardName = new TextField(14, 2, 30, MainConfig.General.Qs("boardName").Replace("½", "")); //Crashes textfield. Bug already submitted
            TextfieldBoardName.Changed += new EventHandler(UpdateBBSName);

            Label LabelMenuFile = new Label(3, 4, "menuFile: ");
            TextField TextFieldMenuFile = new TextField(14, 4, 30, MainConfig.General.Qs("menuFile"));
            Label LabelPromptFile = new Label(3, 5, "promptFile: ");
            TextField TextFieldPromptFile = new TextField(14, 5, 30, MainConfig.General.Qs("promptFile"));
            win.Add(LabelBoardName, TextfieldBoardName, LabelMenuFile, TextFieldMenuFile, LabelPromptFile, TextFieldPromptFile);

            FrameView FrameViewLoginServers = new FrameView(new Rect(1, 10, 63, 10), "Login Servers");

            CheckBox CheckBoxTelnet = new CheckBox(1, 0, "telnet");
            CheckBoxTelnet.Checked = MainConfig.Telnet.Qb("enabled");
            CheckBoxTelnet.Toggled += new EventHandler(UpdateTelnetEnabled);
            Label LabelTelnetPort = new Label(17, 0, "port: ");
            TextField TextFieldTelnetPort = new TextField(24, 0, 5, MainConfig.Telnet.Qstr("port"));
            TextFieldTelnetPort.Changed += new EventHandler(UpdateTelnetPort);

            CheckBox CheckBoxSSH = new CheckBox(1, 1, "ssh");
            CheckBoxSSH.Checked = MainConfig.SSH.Qb("enabled");
            CheckBoxSSH.Toggled += new EventHandler(UpdateSSHEnabled);

            Label LabelSSHPort = new Label(17, 1, "port: ");
            TextField TextFieldSSHPort = new TextField(24, 1, 5, MainConfig.SSH.Qstr("port"));
            TextFieldSSHPort.Changed += new EventHandler(UpdateSSHPort);

            Label LabelPrivateKeyPath = new Label(1, 3, "privateKeyPem: ");
            TextField TextFieldPrivateKeyPath = new TextField(17, 3, 43, MainConfig.SSH.Qstr("privateKeyPem"));

            Label LabelPrivateKeyPass = new Label(1, 4, "privateKeyPass: ");

            TextField TextFieldPrivateKeyPass = new TextField(17, 4, 43, MainConfig.SSH.Qstr("privateKeyPass"));
            TextFieldPrivateKeyPass.Secret = true;

            CheckBox CheckBoxWS = new CheckBox(1, 6, "ws");
            CheckBoxWS.Checked = MainConfig.WSS.Qb("enabled");
            CheckBoxWS.Toggled += new EventHandler(UpdateWSEnabled);

            Label LabelWSPort = new Label(17, 6, "port: ");
            TextField TextFieldWSPort = new TextField(24, 6, 5, MainConfig.WS.Qstr("port"));

            CheckBox CheckBoxWSS = new CheckBox(1, 7, "wss");
            CheckBoxWSS.Checked = MainConfig.WSS.Qb("enabled");
            CheckBoxWSS.Toggled += new EventHandler(UpdateWSSEnabled);

            Label LabelWSSPort = new Label(17, 7, "port: ");
            TextField TextFieldWSSPort = new TextField(24, 7, 5, MainConfig.WSS.Qstr("port"));

            FrameViewLoginServers.Add(CheckBoxTelnet, LabelTelnetPort, TextFieldTelnetPort, CheckBoxSSH, LabelSSHPort, TextFieldSSHPort, LabelPrivateKeyPath, TextFieldPrivateKeyPath, LabelPrivateKeyPass, TextFieldPrivateKeyPass, CheckBoxWS, LabelWSPort, TextFieldWSPort, CheckBoxWSS, LabelWSSPort, TextFieldWSSPort);

            FrameView FrameViewLogging = new FrameView(new Rect(63, 0, 54, 10), "Logging");

            //new CheckBox (1, 0, "Remember me"),
            Label LabelFilename = new Label(2, 1, "Filename: ");
            TextField TextFieldFileame = new TextField(13, 1, 22, MainConfig.Logging.Qs("fileName"));
            Label LabelLogPath = new Label(2, 2, "Path: ");
            TextField TextFieldLogPath = new TextField(13, 2, 22, MainConfig.Paths.Qs("logs"));
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

            win.Add(FrameViewLoginServers, FrameViewLogging, Footer);
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
            MainConfig.SSH["enabled"] = SSH.Checked;
        }

        private static void UpdateTelnetEnabled(object sender, EventArgs e)
        {
            CheckBox Telnet = (CheckBox)sender;
            MainConfig.Telnet["enabled"] = Telnet.Checked;
        }

        private static void UpdateWSEnabled(object sender, EventArgs e)
        {
            CheckBox WS = (CheckBox)sender;
            MainConfig.WS["enabled"] = WS.Checked;
        }

        private static void UpdateWSSEnabled(object sender, EventArgs e)
        {
            CheckBox WSS = (CheckBox)sender;
            MainConfig.WSS["enabled"] = WSS.Checked;
        }

        private static void UpdateSSHPort(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.SSH["port"] = TF.Text.ToString();
        }

        private static void UpdateTelnetPort(object sender, EventArgs e)
        {
            TextField TF = (TextField)sender;
            MainConfig.Telnet["port"] = TF.Text.ToString();
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
            string Filename = SaveD.FileName.ToString();
            if (string.IsNullOrWhiteSpace(Filename))
            {
                return;
            }
            string SaveDir = "";
            if (SaveD.DirectoryPath.EndsWith("\\"))
            {
                SaveDir = SaveD.DirectoryPath.ToString();
            }
            else
            {
                SaveDir = SaveD.DirectoryPath.ToString() + "\\";
            }
            string FullSavePath = SaveDir + Filename;
            SaveJson(FullSavePath);
        }

        private static void Open()
        {
            OpenDialog OpenD = new OpenDialog("Open", "Open a file");
            OpenD.DirectoryPath = OpenD.DirectoryPath = Directory.GetCurrentDirectory();
            OpenD.AllowsMultipleSelection = false;

            Application.Run(OpenD);
            OpenedConfigFile = new FileInfo(OpenD.DirectoryPath.ToString() + OpenD.FilePath.ToString());
            MessageBox.Query(60, 7, "Selected File", string.Join(", ", OpenD.FilePaths), "ok");

            // string FilePath = @"C:\Users\jasin\source\repos\FileTest\config.hjson";
            ClearViews();
            LoadJson(OpenedConfigFile.FullName);
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
            ProcessConfig();
        }

        private static void ProcessConfig()
        {
            win.Remove(LabelNew);
            MainConfig.MainObjects = MainConfig.Mainfile.Qo();
            MainConfig.General = MainConfig.MainObjects.Qo("general");
            MainConfig.Paths = MainConfig.MainObjects.Qo("paths");
            MainConfig.Logging = MainConfig.MainObjects.Qo("logging").Qo("rotatingFile");
            MainConfig.Theme = MainConfig.MainObjects.Qo("theme");
            MainConfig.LoginServers = MainConfig.MainObjects.Qo("loginServers");
            MainConfig.Telnet = MainConfig.LoginServers.Qo("telnet");
            MainConfig.SSH = MainConfig.LoginServers.Qo("ssh");
            MainConfig.WebSocket = MainConfig.LoginServers.Qo("webSocket");
            MainConfig.WS = MainConfig.WebSocket.Qo("ws");
            MainConfig.WSS = MainConfig.WebSocket.Qo("wss");
            MainConfig.Email = MainConfig.MainObjects.Qo("email");
            MainConfig.contentServers = MainConfig.MainObjects.Qo("contentServers");
            MainConfig.messageConferences = MainConfig.MainObjects.Qo("messageConferences");
            MainConfig.messageNetworks = MainConfig.MainObjects.Qo("messageNetworks");
            MainConfig.fileBase = MainConfig.MainObjects.Qo("fileBase");
            MainConfig.scannerTossers = MainConfig.MainObjects.Qo("scannerTossers");
            MainConfig.archivers = MainConfig.MainObjects.Qo("archives");

            AddConfigMenu();
        }

        private static void SaveJson(string FilePath)
        {
            HjsonOptions FileOptions = new HjsonOptions();
            FileOptions.KeepWsc = true;

            HjsonValue.Save(MainConfig.Mainfile, FilePath, FileOptions);
        }
    }
}