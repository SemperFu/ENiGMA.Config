using NStack;
using System;
using Terminal.Gui;

namespace ENiGMAConfig
{
    partial class Program
    {
        private static void UpdateBBSName(TextField BBSName, ustring OldText)
        {
            MainConfig.General["boardName"] = BBSName.Text.ToString();
        }

        private static void UpdateMenuFile(TextField MenuFile, ustring OldText)
        {
            MainConfig.General["menuFile"] = MenuFile.Text.ToString();
        }

        private static void UpdatePromptFile(TextField PromptFile, ustring OldText)
        {
            MainConfig.General["promptFile"] = PromptFile.Text.ToString();
        }

        private static void UpdateDefaultTheme(TextField DefaultTheme, ustring OldText)
        {
            MainConfig.Theme["default"] = DefaultTheme.Text.ToString();
        }

        private static void UpdatePreloginTheme(TextField LoginTheme, ustring OldText)
        {
            MainConfig.Theme["preLogin"] = LoginTheme.Text.ToString();
        }

        private static void UpdateTelnetEnabled(bool TelnetChecked)
        {
            MainConfig.LoginServersTelnet["enabled"] = TelnetChecked;
        }

        private static void UpdateSSHEnabled(bool SSHChecked)
        {
            MainConfig.LoginServersSSH["enabled"] = SSHChecked;
        }

        private static void UpdateWSEnabled(bool WSChecked)
        {
            MainConfig.LoginServersWS["enabled"] = WSChecked;
        }

        private static void UpdateWSSEnabled(bool WSSChecked)
        {
            MainConfig.LoginServersWSS["enabled"] = WSSChecked;
        }

        private static void UpdatePrivateKeyPath(TextField PrivateKeyPath, ustring OldText)
        {
            MainConfig.LoginServersSSH["privateKeyPem"] = PrivateKeyPath.Text.ToString();
        }

        private static void UpdatePrivateKeyPass(TextField PrivateKeyPass, ustring OldText)
        {
            MainConfig.LoginServersSSH["privateKeyPass"] = PrivateKeyPass.Text.ToString();
        }

        private static void UpdateSSHPort(TextField SSHPort, ustring OldText)
        {
            if (SharedFunctions.PortCheck(SSHPort.Text))
            {
                var cp = SSHPort.CursorPosition;
                SSHPort.Text = OldText;
                SSHPort.CursorPosition = Math.Min(cp, SSHPort.Text.RuneCount);
            }

            int DefaultPort = 8889; //default port

            if (Int32.TryParse(SSHPort.Text.ToString(), out int ParsedPort))
            {
                if (ParsedPort > 65535) SSHPort.Text = "65535";
                MainConfig.LoginServersSSH["port"] = ParsedPort;
            }
            else
            {
                MainConfig.LoginServersSSH["port"] = DefaultPort;
            }
        }

        private static void UpdateTelnetPort(TextField TelnetPort, ustring OldText)
        {
            // Don't allow more than 4 digits and Match Digits only
            if (SharedFunctions.PortCheck(TelnetPort.Text))
            {
                var cp = TelnetPort.CursorPosition;
                TelnetPort.Text = OldText;
                TelnetPort.CursorPosition = Math.Min(cp, TelnetPort.Text.RuneCount);
            }

            int DefaultPort = 8888; //default port

            if (Int32.TryParse(TelnetPort.Text.ToString(), out int ParsedPort))
            {
                if (ParsedPort > 65535)
                {
                    TelnetPort.Text = "65535";
                    ParsedPort = 65535;
                }

                MainConfig.LoginServersTelnet["port"] = ParsedPort;
            }
            else
            {
                MainConfig.LoginServersTelnet["port"] = DefaultPort;
            }
        }

        private static void UpdateWSPort(TextField WSPort, ustring OldText)
        {
            // Don't allow more than 4 digits and Match Digits only
            if (SharedFunctions.PortCheck(WSPort.Text))
            {
                var cp = WSPort.CursorPosition;
                WSPort.Text = OldText;
                WSPort.CursorPosition = Math.Min(cp, WSPort.Text.RuneCount);
            }

            int DefaultPort = 8810; //default port

            if (Int32.TryParse(WSPort.Text.ToString(), out int ParsedPort))
            {
                if (ParsedPort > 65535) WSPort.Text = "65535";
                MainConfig.LoginServersWS["port"] = ParsedPort;
            }
            else
            {
                MainConfig.LoginServersWS["port"] = DefaultPort;
            }
        }

        private static void UpdateWSSPort(TextField WSSPort, ustring OldText)
        {
            // Don't allow more than 4 digits and Match Digits only
            if (SharedFunctions.PortCheck(WSSPort.Text))
            {
                var cp = WSSPort.CursorPosition;
                WSSPort.Text = OldText;
                WSSPort.CursorPosition = Math.Min(cp, WSSPort.Text.RuneCount);
            }

            int DefaultPort = 8811; //default port

            if (Int32.TryParse(WSSPort.Text.ToString(), out int ParsedPort))
            {
                if (ParsedPort > 65535) WSSPort.Text = "65535";
                MainConfig.LoginServersWSS["port"] = ParsedPort;
            }
            else
            {
                MainConfig.LoginServersWSS["port"] = DefaultPort;
            }
        }

        private static void ButtonEKeyShow_Clicked(Button ButtonEKeyShow, TextField TextFieldEmailAuthPass)
        {
            if (TextFieldEmailAuthPass.Secret)
            {
                TextFieldEmailAuthPass.Secret = false;
                ButtonEKeyShow.Text = "Hide";
            }
            else
            {
                TextFieldEmailAuthPass.Secret = true;
                ButtonEKeyShow.Text = "Show";
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

        private static void UpdateLogFileame(TextField LogFileame, ustring OldText)
        {
            MainConfig.Logging["fileName"] = LogFileame.Text.ToString();
        }

        private static void UpdateLogPath(TextField LogPath, ustring OldText)
        {
            MainConfig.Paths["logs"] = LogPath.Text.ToString();
        }

        private static void UpdateLogLevel(RadioGroup.SelectedItemChangedArgs SelectedItemChangedArgs)
        {
            // "_Error", "_Warn", "_Info", "_Debug", "_Trace"
            MainConfig.Logging["level"] = SelectedItemChangedArgs.SelectedItem switch
            {
                0 => "error",
                1 => "warn",
                2 => "info",
                3 => "debug",
                4 => "trace",
                //If missing or invalid - Set to info.
                _ => "info",
            };
        }

        private static void UpdateEmailFrom(TextField EmailFrom, ustring OldText)
        {
            MainConfig.Email["defaultFrom"] = EmailFrom.Text.ToString();
        }

        private static void UpdateEmailHost(TextField EmailHost, ustring OldText)
        {
            MainConfig.EmailTransport["host"] = EmailHost.Text.ToString();
        }

        private static void UpdateEmailPort(TextField EmailPort, ustring OldText)
        {
            // Don't allow more than 4 digits and Match Digits only
            if (SharedFunctions.PortCheck(EmailPort.Text))
            {
                var cp = EmailPort.CursorPosition;
                EmailPort.Text = OldText;
                EmailPort.CursorPosition = Math.Min(cp, EmailPort.Text.RuneCount);
            }

            int DefaultPort = 8810; //default port

            if (Int32.TryParse(EmailPort.Text.ToString(), out int ParsedPort))
            {
                if (ParsedPort > 65535) EmailPort.Text = "65535";
                MainConfig.EmailTransport["port"] = ParsedPort;
            }
            else
            {
                MainConfig.EmailTransport["port"] = DefaultPort;
            }
        }

        private static void UpdateEmailAuthPass(TextField EmailAuthPass, ustring OldText)
        {
            MainConfig.EmailAuth["pass"] = EmailAuthPass.Text.ToString();
        }

        private static void UpdateEmailAuthUser(TextField EmailAuthUser, ustring OldText)
        {
            MainConfig.EmailAuth["user"] = EmailAuthUser.Text.ToString();
        }
    }
}