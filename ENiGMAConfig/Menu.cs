using Hjson;
using NStack;
using System.IO;
using Terminal.Gui;

namespace ENiGMAConfig
{
    partial class Program
    {
        private static ConfigHJSON MainConfig = new ConfigHJSON();
        private static FileInfo OpenedConfigFile;
        private static Toplevel top = Application.Top;
        private static string currentversion = "ENiGMA½ v0.0.9-alpha";
        private static string MainWindowsName = currentversion+ " Config Editor";

        private static Window win = new Window(new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), MainWindowsName);
        //private static Window win = new Window(MainWindowsName)
        //{
        //    X = 0,
        //    Y = 1,
        //    Width = Dim.Fill(),
        //    Height = Dim.Fill()
        //};

        private static Label LabelNew = new Label(2, 2, "Open or Create new file ");

        private static Label Footer = new Label(3, 24, "Press F9 (on Unix, ESC+9) to activate the menu");

        private static void StartGUI()
        {
            Application.Init();

            // Creates the top-level window to show

            top.Add(win);
            top = Application.Top;

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

            Application.Run(top);
        }

        private static void About()
        {
            MessageBox.Query(60, 5, "About", "Basic config creation and editing for ENiGMA½ v0.0.9-alpha", "Ok");
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
            OpenedConfigFile = new FileInfo(FullSavePath);
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
            OpenedConfigFile = null;
            ClearViews();
            ProcessConfig();
        }

        private static void Save()
        {
            if (OpenedConfigFile == null)
            {
                //File was new  - Use saveas instead
                SaveAs();
            }
            else
            {
                //File not new, save.
                SaveJson(OpenedConfigFile.FullName);
            }
        }

        private static void AddConfigMenu()
        {
            //win.Remove(LabelNew);
            // Add some controls
            //Label LabelBoardName = new Label("boardName: ") { X = 3, Y = 2 };
            Label LabelBoardName = new Label(3, 2, "BBS Name:");
            TextField TextfieldBoardName = new TextField(19, 2, 30, MainConfig.General.Qs("boardName").Replace("½", "")); //Crashes textfield. Bug already submitted
                                                                                                                          //  TextfieldBoardName.Changed += new EventHandler(UpdateBBSName);
            TextfieldBoardName.TextChanged += UpdateBBSName;

            Label LabelMenuFile = new Label(3, 4, "Menu File:");
            TextField TextFieldMenuFile = new TextField(19, 4, 30, MainConfig.General.Qs("menuFile"));
            TextFieldMenuFile.TextChanged += UpdateMenuFile;

            Label LabelPromptFile = new Label(3, 5, "Prompt File:");
            TextField TextFieldPromptFile = new TextField(19, 5, 30, MainConfig.General.Qs("promptFile"));
            TextFieldPromptFile.TextChanged += UpdatePromptFile;

            Label LabelDefaultTheme = new Label(3, 7, "Default Theme:");
            TextField TextFieldDefaultTheme = new TextField(19, 7, 30, MainConfig.Theme.Qs("default"));
            TextFieldDefaultTheme.TextChanged += UpdateDefaultTheme;

            Label LabelPreLoginTheme = new Label(3, 8, "Prelogin Theme: ");
            TextField TextFieldPreLoginTheme = new TextField(19, 8, 30, MainConfig.Theme.Qs("preLogin"));
            TextFieldPreLoginTheme.TextChanged += UpdatePreloginTheme;

            win.Add(LabelBoardName, TextfieldBoardName, LabelMenuFile, TextFieldMenuFile, LabelPromptFile, TextFieldPromptFile, LabelDefaultTheme, TextFieldDefaultTheme, LabelPreLoginTheme, TextFieldPreLoginTheme);

            FrameView FrameViewLoginServers = new FrameView(new Rect(1, 10, 62, 10), "Login Servers");

            CheckBox CheckBoxTelnet = new CheckBox(1, 0, "telnet");
            CheckBoxTelnet.Checked = MainConfig.LoginServersTelnet.Qb("enabled");
            CheckBoxTelnet.Toggled += UpdateTelnetEnabled;

            Label LabelTelnetPort = new Label(17, 0, "port: ");
            TextField TextFieldTelnetPort = new TextField(24, 0, 5, MainConfig.LoginServersTelnet.Qstr("port"));
            TextFieldTelnetPort.TextChanged += UpdateTelnetPort;

            CheckBox CheckBoxSSH = new CheckBox(1, 1, "ssh");
            CheckBoxSSH.Checked = MainConfig.LoginServersSSH.Qb("enabled");
            CheckBoxSSH.Toggled += UpdateSSHEnabled;

            Label LabelSSHPort = new Label(17, 1, "port: ");
            TextField TextFieldSSHPort = new TextField(24, 1, 5, MainConfig.LoginServersSSH.Qstr("port"));
            TextFieldSSHPort.TextChanged += UpdateSSHPort;

            Label LabelPrivateKeyPath = new Label(1, 3, "privateKeyPem: ");
            TextField TextFieldPrivateKeyPath = new TextField(17, 3, 42, MainConfig.LoginServersSSH.Qstr("privateKeyPem"));
            TextFieldPrivateKeyPath.TextChanged += UpdatePrivateKeyPath;

            Label LabelPrivateKeyPass = new Label(1, 4, "privateKeyPass: ");

            TextField TextFieldPrivateKeyPass = new TextField(17, 4, 32, MainConfig.LoginServersSSH.Qstr("privateKeyPass"));
            TextFieldPrivateKeyPass.Secret = true;
            TextFieldPrivateKeyPass.TextChanged += UpdatePrivateKeyPass;

            Button ButtonPKeyShow = new Button(51, 4, "Show");

            ButtonPKeyShow.Clicked += () => ButtonPKeyShow_Clicked(ButtonPKeyShow, TextFieldPrivateKeyPass);

            CheckBox CheckBoxWS = new CheckBox(1, 6, "ws");
            CheckBoxWS.Checked = MainConfig.LoginServersWSS.Qb("enabled");
            CheckBoxWS.Toggled += UpdateWSEnabled;

            Label LabelWSPort = new Label(17, 6, "port: ");
            TextField TextFieldWSPort = new TextField(24, 6, 5, MainConfig.LoginServersWS.Qstr("port"));
            TextFieldWSPort.TextChanged += UpdateWSPort;

            CheckBox CheckBoxWSS = new CheckBox(1, 7, "wss");
            CheckBoxWSS.Checked = MainConfig.LoginServersWSS.Qb("enabled");
            CheckBoxWSS.Toggled += UpdateWSSEnabled;

            Label LabelWSSPort = new Label(17, 7, "port: ");
            TextField TextFieldWSSPort = new TextField(24, 7, 5, MainConfig.LoginServersWSS.Qstr("port"));
            TextFieldWSSPort.TextChanged += UpdateWSSPort;

            FrameViewLoginServers.Add(CheckBoxTelnet, LabelTelnetPort, TextFieldTelnetPort, CheckBoxSSH, LabelSSHPort, TextFieldSSHPort, LabelPrivateKeyPath, TextFieldPrivateKeyPath, LabelPrivateKeyPass, ButtonPKeyShow, TextFieldPrivateKeyPass, CheckBoxWS, LabelWSPort, TextFieldWSPort, CheckBoxWSS, LabelWSSPort, TextFieldWSSPort);

            FrameView FrameViewLogging = new FrameView(new Rect(64, 0, 53, 10), "Logging");

            //new CheckBox (1, 0, "Remember me"),
            Label LabelFilename = new Label(2, 1, "Filename: ");
            TextField TextFieldFileName = new TextField(13, 1, 22, MainConfig.Logging.Qs("fileName"));
            TextFieldFileName.TextChanged += UpdateLogFileame;
            Label LabelLogPath = new Label(2, 2, "Path: ");
            TextField TextFieldLogPath = new TextField(13, 2, 22, MainConfig.Paths.Qs("logs"));
            TextFieldLogPath.TextChanged += UpdateLogPath;
            Label LabelLogLevel = new Label(39, 0, "Level: ");
            RadioGroup RadioGroupDebug = new RadioGroup(39, 2, new ustring[] { "_Error", "_Warn", "_Info", "_Debug", "_Trace" });

            switch (MainConfig.Logging.Qs("level"))
            {
                case "error":
                    {
                        RadioGroupDebug.SelectedItem = 0;
                        break;
                    }
                case "warn":
                    {
                        RadioGroupDebug.SelectedItem = 1;
                        break;
                    }
                case "info":
                    {
                        RadioGroupDebug.SelectedItem = 2;
                        break;
                    }
                case "debug":
                    {
                        RadioGroupDebug.SelectedItem = 3;
                        break;
                    }
                case "trace":
                    {
                        RadioGroupDebug.SelectedItem = 4;
                        break;
                    }
                default: //If missing or invalid - Set to info.
                    {
                        RadioGroupDebug.SelectedItem = 2;
                        break;
                    }
            }

            //RadioGroupDebug.SelectionChanged += (int x) => UpdateLogLevel(x);
            RadioGroupDebug.SelectedItemChanged += UpdateLogLevel;

            FrameViewLogging.Add(LabelFilename, TextFieldFileName, LabelLogPath, TextFieldLogPath, LabelLogLevel, RadioGroupDebug);
            win.Add(FrameViewLoginServers, FrameViewLogging);

            //Email

            FrameView FrameViewEmail = new FrameView(new Rect(64, 10, 53, 10), "Email");

            //new CheckBox (1, 0, "Remember me"),
            Label LabelEmailTransport = new Label(1, 0, "[Transport] ");
            Label LabelEmailFrom = new Label(8, 1, "From: ");
            TextField TextFieldEmailFrom = new TextField(15, 1, 25, MainConfig.Email.Qs("defaultFrom"));
            TextFieldEmailFrom.TextChanged += UpdateEmailFrom;

            Label LabelEmailHost = new Label(8, 2, "Host: ");
            TextField TextFieldEmailHost = new TextField(15, 2, 25, MainConfig.EmailTransport.Qs("host"));
            TextFieldEmailHost.TextChanged += UpdateEmailHost;
            Label LabelEmailPort = new Label(8, 3, "Port: ");
            TextField TextFieldEmailPort = new TextField(15, 3, 6, MainConfig.EmailTransport.Qstr("port"));
            TextFieldEmailPort.TextChanged += UpdateEmailPort;
            CheckBox CheckBoxEmailSecure = new CheckBox(22, 3, "secure");
            CheckBoxEmailSecure.Checked = MainConfig.EmailTransport.Qb("secure");
            CheckBoxEmailSecure.Toggled += UpdateWSSEnabled;

            Label LabelEmailAuth = new Label(1, 4, "[Auth] ");
            Label LabelEmailAuthUser = new Label(8, 5, "User: ");
            TextField TextFieldEmailAuthUser = new TextField(15, 5, 25, MainConfig.Paths.Qs("logs"));
            TextFieldEmailAuthUser.TextChanged += UpdateEmailAuthUser;
            Label LabelEmailAuthPass = new Label(8, 6, "Pass: ");
            TextField TextFieldEmailAuthPass = new TextField(15, 6, 25, MainConfig.Paths.Qs("logs"));
            TextFieldEmailAuthPass.Secret = true;
            TextFieldEmailAuthPass.TextChanged += UpdateEmailAuthPass;
            Button ButtonEKeyShow = new Button(41, 6, "Show");
            ButtonEKeyShow.Clicked += () => ButtonEKeyShow_Clicked(ButtonEKeyShow, TextFieldEmailAuthPass);

            FrameViewEmail.Add(LabelEmailTransport, LabelEmailAuth, LabelEmailFrom, TextFieldEmailFrom, LabelEmailHost, TextFieldEmailHost, LabelEmailPort, TextFieldEmailPort, CheckBoxEmailSecure, LabelEmailAuthUser, TextFieldEmailAuthUser, TextFieldEmailAuthUser, LabelEmailAuthPass, TextFieldEmailAuthPass, ButtonEKeyShow);
            win.Add(FrameViewEmail, Footer);
        }
    }
}