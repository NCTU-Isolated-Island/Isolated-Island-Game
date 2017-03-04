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
                    return "拉普達";
                case OceanType.Type2:
                    return "北冰洋";
                case OceanType.Type3:
                    return "阿波羅";
                case OceanType.Type4:
                    return "梅索布達米亞";
                case OceanType.Type5:
                    return "亞特蘭提斯";
                case OceanType.Type6:
                    return "奧林匹斯";
                case OceanType.Type7:
                    return "所羅門";
                default:
                    return "未定義的";
            }
        }
    }
}
