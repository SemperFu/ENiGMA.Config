using Newtonsoft.Json;
using System.Collections.Generic;

namespace ENiGMAConfig
{
  
    public class EnigmaConfigSchema
    {
        [JsonProperty("general")]
        public General General { get; set; }

        [JsonProperty("paths")]
        public Paths Paths { get; set; }

        [JsonProperty("logging")]
        public Logging Logging { get; set; }

        [JsonProperty("theme")]
        public Theme Theme { get; set; }

        [JsonProperty("loginServers")]
        public LoginServers LoginServers { get; set; }

        [JsonProperty("contentServers")]
        public ContentServers ContentServers { get; set; }

        [JsonProperty("email")]
        public Email Email { get; set; }

        [JsonProperty("messageConferences")]
        public MessageConferences MessageConferences { get; set; }

        [JsonProperty("scannerTossers")]
        public ScannerTossers ScannerTossers { get; set; }

        [JsonProperty("fileBase")]
        public FileBase FileBase { get; set; }

        [JsonProperty("users")]
        public Users Users { get; set; }

        [JsonProperty("archives")]
        public Archives Archives { get; set; }

        [JsonProperty("fileTransferProtocols")]
        public FileTransferProtocols FileTransferProtocols { get; set; }

        [JsonProperty("statLog")]
        public StatLog StatLog { get; set; }
    }

    public partial class Archives
    {
        [JsonProperty("archivers")]
        public FileTransferProtocols Archivers { get; set; }
    }

    public partial class FileTransferProtocols
    {
    }

    public partial class ContentServers
    {
        [JsonProperty("web")]
        public Web Web { get; set; }

        [JsonProperty("gopher")]
        public Gopher Gopher { get; set; }
    }

    public partial class Gopher
    {
        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("publicHostname")]
        public string PublicHostname { get; set; }

        [JsonProperty("publicPort")]
        public long PublicPort { get; set; }

        [JsonProperty("bannerFile")]
        public string BannerFile { get; set; }
    }

    public partial class Web
    {
        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("staticRoot")]
        public string StaticRoot { get; set; }

        [JsonProperty("resetPassword")]
        public ResetPassword ResetPassword { get; set; }

        [JsonProperty("http")]
        public Http Http { get; set; }

        [JsonProperty("https")]
        public Https Https { get; set; }
    }

    public partial class Http
    {
        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

    public partial class Https
    {
        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("certPem")]
        public string CertPem { get; set; }

        [JsonProperty("keyPem")]
        public string KeyPem { get; set; }
    }

    public partial class ResetPassword
    {
        [JsonProperty("resetPassEmailText")]
        public string ResetPassEmailText { get; set; }

        [JsonProperty("resetPassEmailHtml")]
        public string ResetPassEmailHtml { get; set; }

        [JsonProperty("resetPageTemplate")]
        public string ResetPageTemplate { get; set; }
    }

    public partial class Email
    {
        [JsonProperty("defaultFrom")]
        public string DefaultFrom { get; set; }

        [JsonProperty("transport")]
        public Transport Transport { get; set; }
    }

    public partial class Transport
    {
        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("secure")]
        public bool Secure { get; set; }

        [JsonProperty("auth")]
        public Auth Auth { get; set; }
    }

    public partial class Auth
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("pass")]
        public string Pass { get; set; }
    }

    public partial class FileBase
    {
        [JsonProperty("areaStoragePrefix")]
        public string AreaStoragePrefix { get; set; }

        [JsonProperty("storageTags")]
        public FileTransferProtocols StorageTags { get; set; }

        [JsonProperty("areas")]
        public FileTransferProtocols Areas { get; set; }
    }

    public partial class General
    {
        [JsonProperty("boardName")]
        public string BoardName { get; set; }

        [JsonProperty("menuFile")]
        public string MenuFile { get; set; }

        [JsonProperty("promptFile")]
        public string PromptFile { get; set; }
    }

    public partial class Logging
    {
        [JsonProperty("rotatingFile")]
        public RotatingFile RotatingFile { get; set; }
    }

    public partial class RotatingFile
    {
        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }
    }

    public partial class LoginServers
    {
        [JsonProperty("telnet")]
        public Telnet Telnet { get; set; }

        [JsonProperty("ssh")]
        public Ssh Ssh { get; set; }

        [JsonProperty("webSocket")]
        public WebSocket WebSocket { get; set; }
    }

    public partial class Ssh
    {
        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("privateKeyPass")]
        public string PrivateKeyPass { get; set; }

        [JsonProperty("algorithms")]
        public Algorithms Algorithms { get; set; }

        [JsonProperty("privateKeyPem")]
        public string PrivateKeyPem { get; set; }

        [JsonProperty("firstMenu")]
        public string FirstMenu { get; set; }

        [JsonProperty("firstMenuNewUser")]
        public string FirstMenuNewUser { get; set; }
    }

    public partial class Algorithms
    {
        [JsonProperty("kex")]
        public string[] Kex { get; set; }

        [JsonProperty("cipher")]
        public string[] Cipher { get; set; }

        [JsonProperty("hmac")]
        public string[] Hmac { get; set; }

        [JsonProperty("compress")]
        public string[] Compress { get; set; }
    }

    public partial class Telnet
    {
        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("firstMenu")]
        public string FirstMenu { get; set; }
    }

    public partial class WebSocket
    {
        [JsonProperty("proxied")]
        public bool Proxied { get; set; }

        [JsonProperty("ws")]
        public Http Ws { get; set; }

        [JsonProperty("wss")]
        public Https Wss { get; set; }
    }

    public partial class MessageConferences
    {
        [JsonProperty("another_sample_conf")]
        public AnotherSampleConf AnotherSampleConf { get; set; }

        [JsonProperty("local")]
        public Local Local { get; set; }
    }

    public partial class AnotherSampleConf
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("areas")]
        public AnotherSampleConfAreas Areas { get; set; }
    }

    public partial class AnotherSampleConfAreas
    {
        [JsonProperty("another_sample_area")]
        public AnotherSampleArea AnotherSampleArea { get; set; }
    }

    public partial class AnotherSampleArea
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("sort")]
        public long Sort { get; set; }

        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Default { get; set; }
    }

    public partial class Local
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("sort")]
        public long Sort { get; set; }

        [JsonProperty("default")]
        public bool Default { get; set; }

        [JsonProperty("areas")]
        public LocalAreas Areas { get; set; }
    }

    public partial class LocalAreas
    {
        [JsonProperty("general")]
        public AnotherSampleArea General { get; set; }
    }

    public partial class Paths
    {
        [JsonProperty("logs")]
        public string Logs { get; set; }
    }

    public partial class ScannerTossers
    {
        [JsonProperty("ftn_bso")]
        public FileTransferProtocols FtnBso { get; set; }
    }

    public partial class StatLog
    {
        [JsonProperty("systemEvents")]
        public SystemEvents SystemEvents { get; set; }
    }

    public partial class SystemEvents
    {
        [JsonProperty("loginHistoryMax")]
        public long LoginHistoryMax { get; set; }
    }

    public partial class Theme
    {
        [JsonProperty("default")]
        public string Default { get; set; }

        [JsonProperty("preLogin")]
        public string PreLogin { get; set; }

        [JsonProperty("passwordChar")]
        public string PasswordChar { get; set; }

        [JsonProperty("dateFormat")]
        public DateEFormat DateFormat { get; set; }

        [JsonProperty("timeFormat")]
        public TimeFormat TimeFormat { get; set; }

        [JsonProperty("dateTimeFormat")]
        public DateEFormat DateTimeFormat { get; set; }
    }

    public partial class DateEFormat
    {
        [JsonProperty("short")]
        public string Short { get; set; }

        [JsonProperty("long")]
        public string Long { get; set; }
    }

    public partial class TimeFormat
    {
        [JsonProperty("short")]
        public string Short { get; set; }
    }

    public partial class Users
    {
        [JsonProperty("requireActivation")]
        public bool RequireActivation { get; set; }

        [JsonProperty("preAuthIdleLogoutSeconds")]
        public long PreAuthIdleLogoutSeconds { get; set; }

        [JsonProperty("idleLogoutSeconds")]
        public long IdleLogoutSeconds { get; set; }

        [JsonProperty("newUserNames")]
        public string[] NewUserNames { get; set; }
    }
}