using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using IsolatedIslandGame.Database;
using IsolatedIslandGame.Database.MySQL;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Protocol.Communication;
using IsolatedIslandGame.Server.Configuration;
using IsolatedIslandGame.Server.Items;
using log4net.Config;
using Photon.SocketServer;
using System.IO;
using System;

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
            SetupConfiguration();
            SetupServices();
            SetupFactories();
            SetupManagers();
            Scheduler.Inital(TimeSpan.FromSeconds(1));
            
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

        private void SetupLog()
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
        private void SetupConfiguration()
        {
            SystemConfiguration config;
            if(SystemConfiguration.Load(Path.Combine(ApplicationPath, "config", "system.config"), out config))
            {
                SystemConfiguration.InitialConfiguration(config);
            }
            else
            {
                LogService.Fatal("Load SystemConfiguration Fail");
            }
            Photon.SocketServer.Protocol.TryRegisterCustomType(typeof(Blueprint.ElementInfo), (byte)SerializationTypeCode.BlueprintElementInfo, SerializationHelper.Serialize<Blueprint.ElementInfo>, SerializationHelper.Deserialize<Blueprint.ElementInfo>);
        }
        private void SetupServices()
        {
            FacebookService.InitialService();
            DatabaseService.Initial(new MySQL_DatabaseService());
            if (DatabaseService.Connect(
                    hostName: SystemConfiguration.Instance.DatabaseHostname, 
                    userName: SystemConfiguration.Instance.DatabaseUsername, 
                    password: SystemConfiguration.Instance.DatabasePassword, 
                    database: SystemConfiguration.Instance.Database))
            {
                Log.Info("Database Setup Successiful.......");
            }
        }
        private void SetupFactories()
        {
            UserFactory.Initial();
            PlayerFactory.Initial();
            ItemManager.Initial(new ItemFactory());
            InventoryItemInfoFactory.Initial(new ServerInventoryItemInfoFactory());
            DecorationFactory.Initial(new ServerDecorationFactory());
            BlueprintManager.Initial(new BlueprintFactory());
        }
        private void SetupManagers()
        {
            SystemManager.Initial(new ServerSystemManager());
            VesselManager.Initial(new ServerVesselManager());
            FriendManager.InitialManager();
            PlayerInformationManager.Initial(new ServerPlayerInformationManager());
            TransactionManager.Initial();
            IslandManager.Initial();
        }
    }
}
