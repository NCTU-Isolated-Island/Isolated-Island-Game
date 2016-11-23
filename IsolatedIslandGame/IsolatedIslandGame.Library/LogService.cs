namespace IsolatedIslandGame.Library
{
    public class LogService
    {
        private static LogService instance;
        public static LogHandler Info { get { return instance.infoMethod; } }
        public static LogFormatHandler InfoFormat { get { return instance.infoFormatMethod; } }
        public static LogHandler Error { get { return instance.errorMethod; } }
        public static LogFormatHandler ErrorFormat { get { return instance.errorFormatMethod; } }
        public static LogHandler Fatal { get { return instance.fatalMethod; } }
        public static LogFormatHandler FatalFormat { get { return instance.fatalFormatMethod; } }

        public static void InitialService(LogHandler infoMethod, LogFormatHandler infoFormatMethod, LogHandler errorMethod, LogFormatHandler errorFormatMethod, LogHandler fatalMethod, LogFormatHandler fatalFormatMethod)
        {
            instance = new LogService();
            instance.infoMethod = infoMethod;
            instance.infoFormatMethod = infoFormatMethod;
            instance.errorMethod = errorMethod;
            instance.errorFormatMethod = errorFormatMethod;
            instance.fatalMethod = fatalMethod;
            instance.fatalFormatMethod = fatalFormatMethod;
        }

        public delegate void LogHandler(object message);
        public delegate void LogFormatHandler(string format, params object[] args);

        private LogHandler infoMethod;
        private LogFormatHandler infoFormatMethod;
        private LogHandler errorMethod;
        private LogFormatHandler errorFormatMethod;
        private LogHandler fatalMethod;
        private LogFormatHandler fatalFormatMethod;
    }
}
