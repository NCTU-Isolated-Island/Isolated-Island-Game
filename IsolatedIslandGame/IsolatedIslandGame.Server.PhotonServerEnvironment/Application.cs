using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using System.IO;
using IsolatedIslandGame.Library;

namespace IsolatedIslandGame.Server.PhotonServerEnvironment
{
    public class Application : ApplicationBase
    {
        private static Application instance;
        public static Application ServerInstance { get { return instance; } }
        public static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        protected override void Setup()
        {
            instance = this;

            SetupLog();
            SetupServices();
            SetupFactories();
            
            Log.Info("PhotonServer Setup Successiful.......");
        }

        protected override void TearDown()
        {

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
        }
        protected void SetupServices()
        {
            LogService.InitialService(Log.Info, Log.InfoFormat);
            FacebookService.InitialService();
        }
        protected void SetupFactories()
        {
            UserFactory.InitialFactory();
            PlayerFactory.InitialFactory();
        }
    }
}
