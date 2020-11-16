using Hjson;
using NStack;
using System.IO;
using Terminal.Gui;

namespace ENiGMAConfig
{
    partial class Program
    {
        
        private static Toplevel top = Application.Top;
        private static string currentversion = "ENiGMA½ v0.0.9-alpha";
        private static string MainWindowsName = currentversion + " Config Editor";

        private static Window win = new(new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), MainWindowsName);
        //private static Window win = new Window(MainWindowsName)
        //{
        //    X = 0,
        //    Y = 1,
        //    Width = Dim.Fill(),
        //    Height = Dim.Fill()
        //};

        private static Label LabelNew = new(2, 2, "Open or Create new file ");

        private static Label Footer = new(3, 24, "Press F9 (on Unix, ESC+9) to activate the menu");

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
            SaveDialog SaveD = new("Save As", "Save as file");
            SaveD.DirectoryPath = Directory.GetCurrentDirectory(); //Use current path for now. Switch to default config
            SaveD.AllowedFileTypes = new string[] { "hjson" };
            SaveD.NameFieldLabel = "File: ";
            if (MainConfig.OpenedConfigFile is not null)
            {
                SaveD.FilePath = MainConfig.OpenedConfigFile.Name; //Default filename prompt
            }
            else
            {
                SaveD.FilePath = "config.hjson"; //Default filename prompt
            }
           
            Application.Run(SaveD);
            // SaveD.
            if (SaveD.FileName == null) return; //No file selected - return
            string Filename = SaveD.FileName.ToString();
            if (string.IsNullOrWhiteSpace(Filename))
            {
                return;
            }

            string FullSavePath = Path.Combine(SaveD.DirectoryPath.ToString(), Filename);
            MainConfig.OpenedConfigFile = new FileInfo(FullSavePath);
            SaveJson(FullSavePath);
        }

        private static void Open()
        {
            OpenDialog OpenD = new("Open", "Open a file");
            OpenD.DirectoryPath = OpenD.DirectoryPath = Directory.GetCurrentDirectory();
            OpenD.AllowsMultipleSelection = false;
            OpenD.AllowedFileTypes = new string[] { "hjson", "json" };
            OpenD.NameFieldLabel = "File: ";
            //OpenD.FilePath = MainConfig.FileName; //Default filename prompt
            //System.Runtime.InteropServices.RuntimeInformation
            Application.Run(OpenD);

            if (OpenD.FilePaths.Count == 0) return; //No file selected - return
            MainConfig.OpenedConfigFile = new FileInfo(OpenD.FilePaths[0]);
            if (MainConfig.OpenedConfigFile.Exists)
            {
                MessageBox.Query(60, 7, "Selected File", string.Join(", ", OpenD.FilePaths), "Ok");
                ClearViews();
                LoadJson(MainConfig.OpenedConfigFile.FullName);
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
            HjsonOptions ImportOptions = new() { KeepWsc = true };
            TextReader TextReaderNew = new StringReader(Properties.Resources.DefaultConfigFile);

            MainConfig.Mainfile = HjsonValue.Load(TextReaderNew, ImportOptions);
            MainConfig.OpenedConfigFile = null;
            ClearViews();
            ProcessConfig();
        }

        private static void Save()
        {
            if (MainConfig.OpenedConfigFile == null)
            {
                //File was new  - Use saveas instead
                SaveAs();
            }
            else
            {
                //File not new, save.
                SaveJson(MainConfig.OpenedConfigFile.FullName);
            }
        }

        private static void AddConfigMenu()
        {
            //win.Remove(LabelNew);
            // Add some controls
            //Label LabelBoardName = new Label("boardName: ") { X = 3, Y = 2 };
            Label LabelBoardName = new(3, 2, "BBS Name:");
            TextField TextfieldBoardName = new(19, 2, 30, MainConfig.General.Qs("boardName").Replace("½", "")); //Crashes textfield. Bug already submitted
                                                                                                                //  TextfieldBoardName.Changed += new EventHandler(UpdateBBSName);
            TextfieldBoardName.TextChanged += (e) => UpdateBBSName(TextfieldBoardName, e);

            Label LabelMenuFile = new(3, 4, "Menu File:");
            TextField TextFieldMenuFile = new(19, 4, 30, MainConfig.General.Qs("menuFile"));
            TextFieldMenuFile.TextChanged += (e) => UpdateMenuFile(TextFieldMenuFile, e);

            Label LabelPromptFile = new(3, 5, "Prompt File:");
            TextField TextFieldPromptFile = new(19, 5, 30, MainConfig.General.Qs("promptFile"));
            TextFieldPromptFile.TextChanged += (e) => UpdatePromptFile(TextFieldPromptFile, e);

            Label LabelDefaultTheme = new(3, 7, "Default Theme:");
            TextField TextFieldDefaultTheme = new(19, 7, 30, MainConfig.Theme.Qs("default"));
            TextFieldDefaultTheme.TextChanged += (e) => UpdateDefaultTheme(TextFieldDefaultTheme, e);

            Label LabelPreLoginTheme = new(3, 8, "Prelogin Theme: ");
            TextField TextFieldPreLoginTheme = new(19, 8, 30, MainConfig.Theme.Qs("preLogin"));
            TextFieldPreLoginTheme.TextChanged += (e) => UpdatePreloginTheme(TextFieldPreLoginTheme, e);

            win.Add(LabelBoardName, TextfieldBoardName, LabelMenuFile, TextFieldMenuFile, LabelPromptFile, TextFieldPromptFile, LabelDefaultTheme, TextFieldDefaultTheme, LabelPreLoginTheme, TextFieldPreLoginTheme);

            FrameView FrameViewLoginServers = new(new Rect(1, 10, 62, 10), "Login Servers");

            CheckBox CheckBoxTelnet = new(1, 0, "telnet");
            CheckBoxTelnet.Checked = MainConfig.LoginServersTelnet.Qb("enabled");
            CheckBoxTelnet.Toggled += UpdateTelnetEnabled;

            Label LabelTelnetPort = new(17, 0, "port: ");
            TextField TextFieldTelnetPort = new(24, 0, 6, MainConfig.LoginServersTelnet.Qstr("port"));
            TextFieldTelnetPort.TextChanged += (e) => UpdateTelnetPort(TextFieldTelnetPort, e);

            CheckBox CheckBoxSSH = new(1, 1, "ssh");
            CheckBoxSSH.Checked = MainConfig.LoginServersSSH.Qb("enabled");
            CheckBoxSSH.Toggled += UpdateSSHEnabled;

            Label LabelSSHPort = new(17, 1, "port: ");
            TextField TextFieldSSHPort = new(24, 1, 6, MainConfig.LoginServersSSH.Qstr("port"));
            TextFieldSSHPort.TextChanged += (e) => UpdateSSHPort(TextFieldSSHPort, e);

            Label LabelPrivateKeyPath = new(1, 3, "privateKeyPem: ");
            TextField TextFieldPrivateKeyPath = new(17, 3, 42, MainConfig.LoginServersSSH.Qstr("privateKeyPem"));
            TextFieldPrivateKeyPath.TextChanged += (e) => UpdatePrivateKeyPath(TextFieldPrivateKeyPath, e);

            Label LabelPrivateKeyPass = new(1, 4, "privateKeyPass: ");

            TextField TextFieldPrivateKeyPass = new(17, 4, 32, MainConfig.LoginServersSSH.Qstr("privateKeyPass"));
            TextFieldPrivateKeyPass.Secret = true;
            TextFieldPrivateKeyPass.TextChanged += (e) => UpdatePrivateKeyPass(TextFieldPrivateKeyPass, e);

            Button ButtonPKeyShow = new(51, 4, "Show");

            ButtonPKeyShow.Clicked += () => ButtonPKeyShow_Clicked(ButtonPKeyShow, TextFieldPrivateKeyPass);

            CheckBox CheckBoxWS = new(1, 6, "ws");
            CheckBoxWS.Checked = MainConfig.LoginServersWSS.Qb("enabled");
            CheckBoxWS.Toggled += UpdateWSEnabled;

            Label LabelWSPort = new(17, 6, "port: ");
            TextField TextFieldWSPort = new(24, 6, 6, MainConfig.LoginServersWS.Qstr("port"));
            TextFieldWSPort.TextChanged += (e) => UpdateWSPort(TextFieldWSPort, e);

            CheckBox CheckBoxWSS = new(1, 7, "wss");
            CheckBoxWSS.Checked = MainConfig.LoginServersWSS.Qb("enabled");
            CheckBoxWSS.Toggled += UpdateWSSEnabled;

            Label LabelWSSPort = new(17, 7, "port: ");
            TextField TextFieldWSSPort = new(24, 7, 6, MainConfig.LoginServersWSS.Qstr("port"));
            TextFieldWSSPort.TextChanged += (e) => UpdateWSSPort(TextFieldWSSPort, e);

            FrameViewLoginServers.Add(CheckBoxTelnet, LabelTelnetPort, TextFieldTelnetPort, CheckBoxSSH, LabelSSHPort, TextFieldSSHPort, LabelPrivateKeyPath, TextFieldPrivateKeyPath, LabelPrivateKeyPass, ButtonPKeyShow, TextFieldPrivateKeyPass, CheckBoxWS, LabelWSPort, TextFieldWSPort, CheckBoxWSS, LabelWSSPort, TextFieldWSSPort);

            FrameView FrameViewLogging = new(new Rect(64, 0, 53, 10), "Logging");

            //new CheckBox (1, 0, "Remember me"),
            Label LabelFilename = new(2, 1, "Filename: ");
            TextField TextFieldFileName = new(13, 1, 22, MainConfig.Logging.Qs("fileName"));
            TextFieldFileName.TextChanged += (e) => UpdateLogFileame(TextFieldFileName, e);
            Label LabelLogPath = new(2, 2, "Path: ");
            TextField TextFieldLogPath = new(13, 2, 22, MainConfig.Paths.Qs("logs"));
            TextFieldLogPath.TextChanged += (e) => UpdateLogPath(TextFieldLogPath, e);
            Label LabelLogLevel = new(39, 0, "Level: ");
            RadioGroup RadioGroupDebug = new(39, 2, new ustring[] { "_Error", "_Warn", "_Info", "_Debug", "_Trace" });

            RadioGroupDebug.SelectedItem = MainConfig.Logging.Qs("level") switch
            {
                "error" => 0,
                "warn" => 1,
                "info" => 2,
                "debug" => 3,
                "trace" => 4,
                //If missing or invalid - Set to info(2).
                _ => 2,
            };

            RadioGroupDebug.SelectedItemChanged += UpdateLogLevel;

            FrameViewLogging.Add(LabelFilename, TextFieldFileName, LabelLogPath, TextFieldLogPath, LabelLogLevel, RadioGroupDebug);
            win.Add(FrameViewLoginServers, FrameViewLogging);

            //Email

            FrameView FrameViewEmail = new(new Rect(64, 10, 53, 10), "Email");

            //new CheckBox (1, 0, "Remember me"),
            Label LabelEmailTransport = new(1, 0, "[Transport] ");
            Label LabelEmailFrom = new(8, 1, "From: ");
            TextField TextFieldEmailFrom = new(15, 1, 25, MainConfig.Email.Qs("defaultFrom"));
            TextFieldEmailFrom.TextChanged += (e) => UpdateEmailFrom(TextFieldEmailFrom, e);

            Label LabelEmailHost = new(8, 2, "Host: ");
            TextField TextFieldEmailHost = new(15, 2, 25, MainConfig.EmailTransport.Qs("host"));
            TextFieldEmailHost.TextChanged += (e) => UpdateEmailHost(TextFieldEmailHost, e);
            Label LabelEmailPort = new(8, 3, "Port: ");
            TextField TextFieldEmailPort = new(15, 3, 6, MainConfig.EmailTransport.Qstr("port"));
            TextFieldEmailPort.TextChanged += (e) => UpdateEmailPort(TextFieldEmailPort, e);
            CheckBox CheckBoxEmailSecure = new(22, 3, "secure");
            CheckBoxEmailSecure.Checked = MainConfig.EmailTransport.Qb("secure");
            CheckBoxEmailSecure.Toggled += UpdateWSSEnabled;

            Label LabelEmailAuth = new(1, 4, "[Auth] ");
            Label LabelEmailAuthUser = new(8, 5, "User: ");
            TextField TextFieldEmailAuthUser = new(15, 5, 25, MainConfig.EmailAuth.Qs("user"));
            TextFieldEmailAuthUser.TextChanged += (e) => UpdateEmailAuthUser(TextFieldEmailAuthUser, e);
            Label LabelEmailAuthPass = new(8, 6, "Pass: ");
            TextField TextFieldEmailAuthPass = new(15, 6, 25, MainConfig.EmailAuth.Qs("pass"));
            TextFieldEmailAuthPass.Secret = true;
            TextFieldEmailAuthPass.TextChanged += (e) => UpdateEmailAuthPass(TextFieldEmailAuthPass, e);
            Button ButtonEKeyShow = new(41, 6, "Show");
            ButtonEKeyShow.Clicked += () => ButtonEKeyShow_Clicked(ButtonEKeyShow, TextFieldEmailAuthPass);

            FrameViewEmail.Add(LabelEmailTransport, LabelEmailAuth, LabelEmailFrom, TextFieldEmailFrom, LabelEmailHost, TextFieldEmailHost, LabelEmailPort, TextFieldEmailPort, CheckBoxEmailSecure, LabelEmailAuthUser, TextFieldEmailAuthUser, TextFieldEmailAuthUser, LabelEmailAuthPass, TextFieldEmailAuthPass, ButtonEKeyShow);
            win.Add(FrameViewEmail, Footer);
        }
    }
}