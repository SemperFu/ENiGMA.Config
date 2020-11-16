using Hjson;
using System.IO;

namespace ENiGMAConfig
{
    public class ConfigHJSON
    {
        public JsonValue Mainfile; //For Main Config
        public EnigmaConfigSchema Deserialized;
        public FileInfo OpenedConfigFile;
        public JsonObject MainObjects; //Convert to Objects
        public JsonObject General;  // For General Config settings
        public JsonObject Paths;
        public JsonObject Logging;
        public JsonObject Theme;
        public JsonObject LoginServers;
        public JsonObject LoginServersTelnet;
        public JsonObject LoginServersSSH;
        public JsonObject LoginServersWebSocket;
        public JsonObject LoginServersWS;
        public JsonObject LoginServersWSS;
        public JsonObject Email;
        public JsonObject EmailTransport;
        public JsonObject EmailAuth;
        public JsonObject contentServers;
        public JsonObject messageConferences;
        public JsonObject messageNetworks;
        public JsonObject fileBase;
        public JsonObject scannerTossers;
        public JsonObject archivers;

        public ConfigHJSON()
        {
            OpenedConfigFile = null;
            Deserialized = new();
        }
    }
}