using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using IsolatedIslandGame.Database;
using IsolatedIslandGame.Database.MySQL;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.CommunicationInfrastructure;
using IsolatedIslandGame.Library.Items;
using IsolatedIslandGame.Library.Landmarks;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol.Communication;
using IsolatedIslandGame.Server.Configuration;
using log4net.Config;
using Photon.SocketServer;
using System;
using System.IO;

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
            Scheduler.Inital(TimeSpan.FromSeconds(1));
            SetupServices();
            SetupManagers();
            
            
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
        private void SetupManagers()
        {
            UserFactory.Initial();
            SystemManager.Initial(new ServerSystemManager());
            PlayerFactory.Initial();
            ItemManager.Initial(new ItemFactory());
            InventoryItemInfoFactory.Initial(new Items.ServerInventoryItemInfoFactory());
            DecorationFactory.Initial(new Items.ServerDecorationFactory());
            BlueprintManager.Initial(new BlueprintFactory());
            VesselManager.Initial(new ServerVesselManager());
            QuestRecordFactory.Initial(new Quests.ServerQuestRecordFactory());
            QuestManager.Initial(new QuestFactory());
            LandmarkManager.Initial(new LandmarkFactory());
            FriendManager.InitialManager();
            PlayerInformationManager.Initial(new ServerPlayerInformationManager());
            TransactionManager.Initial();
            IslandManager.Initial();
        }
    }
}
