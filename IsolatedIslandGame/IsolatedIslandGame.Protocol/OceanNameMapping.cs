namespace IsolatedIslandGame.Protocol
{
    public static class OceanNameMapping
    {
        public static string GetOceanName(OceanType oceanType)
        {
            switch(oceanType)
            {
                case OceanType.Unknown:
                    return "未知的";
                case OceanType.Type1:
                    return "第1";
                case OceanType.Type2:
                    return "第2";
                case OceanType.Type3:
                    return "第3";
                case OceanType.Type4:
                    return "第4";
                case OceanType.Type5:
                    return "第5";
                case OceanType.Type6:
                    return "第6";
                case OceanType.Type7:
                    return "第7";
                default:
                    return "未定義的";
            }
        }
    }
}
