namespace IsolatedIslandGame.Protocol
{
    public static class GroupNameMapping
    {
        public static string GetGroupName(GroupType groupType)
        {
            switch (groupType)
            {
                case GroupType.No:
                    return "無";
                case GroupType.Animal:
                    return "信仰";
                case GroupType.Businessman:
                    return "科技";
                case GroupType.Farmer:
                    return "自然";
                default:
                    return "未定義的";
            }
        }
    }
}
