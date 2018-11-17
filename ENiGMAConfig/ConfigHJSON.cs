using Hjson;

namespace ENiGMAConfig
{
    internal class ConfigHJSON
    {
        public JsonValue Mainfile; //For Main Config
        public JsonObject MainObjects; //Convert to Objects
        public JsonObject General;  // For General Config settings
        public JsonObject Paths;
        public JsonObject Logging;
        public JsonObject Theme;
        public JsonObject LoginServers;
        public JsonObject Telnet;
        public JsonObject SSH;
        public JsonObject WebSocket;
        public JsonObject WS;
        public JsonObject WSS;
        public JsonObject Email;
        public JsonObject contentServers;
        public JsonObject messageConferences;
        public JsonObject messageNetworks;
        public JsonObject fileBase;
        public JsonObject scannerTossers;
        public JsonObject archivers;
    }
}