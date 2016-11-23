using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using IsolatedIslandGame.Database;
using IsolatedIslandGame.Database.MySQL;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Server.Configuration;
using log4net.Config;
using Photon.SocketServer;
using System.IO;

namespace IsolatedIslandGame.Server.PhotonServerEnvironment
{
    public class Application : ApplicationBase
    {
        private static Application instance;
        public static Application ServerInstance { get { return instance; } }
        public static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public SystemConfiguration SystemConfiguration { get; private set; }

        protected override void Setup()
        {
            instance = this;

            SetupLog();
            SetupConfiguration();
            SetupServices();
            SetupFactories();
            
            Log.Info("PhotonServer Setup Successiful.......");
        }

        protected override void TearDown()
        {
            DatabaseService.Dispose();
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new Peer(initRequest);
        }

        protected void SetupLog()
        {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(ApplicationPath, "log");
            FileInfo file = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }
            LogService.InitialService(Log.Info, Log.InfoFormat, Log.Error, Log.ErrorFormat, Log.Fatal, Log.FatalFormat);
        }
        protected void SetupConfiguration()
        {
            SystemConfiguration = SystemConfiguration.Load(Path.Combine(ApplicationPath, "config", "system.config"));
        }
        protected void SetupServices()
        {
            FacebookService.InitialService();
            DatabaseService.Initial(new MySQLDatabaseService());
            if (DatabaseService.Connect(SystemConfiguration.DatabaseHostname, SystemConfiguration.DatabaseUsername, SystemConfiguration.DatabasePassword, SystemConfiguration.Database))
            {
                Log.Info("Database Setup Successiful.......");
            }
        }
        protected void SetupFactories()
        {
            UserFactory.InitialFactory();
            PlayerFactory.InitialFactory();
        }
    }
}
