using IsolatedIslandGame.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedIslandGame.Library
{
    public class ClientFunctionCheckTable
    {
        public static ClientFunctionCheckTable Instance { get; private set; }
        public static void Initial(Dictionary<int, bool> functionCheckTable)
        {
            Instance = new ClientFunctionCheckTable();
            Instance.functionCheckTable = new Dictionary<ClientFunctionCode, bool>();
            foreach(var pair in functionCheckTable)
            {
                Instance.functionCheckTable.Add((ClientFunctionCode)pair.Key, pair.Value);
            }
            LogService.Info("Initial ClientFunctionCheckTable Successful");
        }

        private Dictionary<ClientFunctionCode, bool> functionCheckTable = new Dictionary<ClientFunctionCode, bool>();
        public Dictionary<int, bool> FunctionCheckTableCopy { get { return functionCheckTable.ToDictionary(entry => (int)entry.Key,entry => entry.Value); ; } }
        public bool IsFunctionOpened(ClientFunctionCode functionCode)
        {
            return functionCheckTable.ContainsKey(functionCode) && functionCheckTable[functionCode];
        }
    }
}
