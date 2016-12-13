namespace IsolatedIslandGame.Library.Items
{
    public abstract class DecorationFactory
    {
        public static DecorationFactory Instance { get; private set; }
        public static void Initial(DecorationFactory factory)
        {
            Instance = factory;
        }

        public abstract Decoration CreateDecoration(Material material, float positionX, float positionY, float positionZ, float rotationEulerAngleX, float rotationEulerAngleY, float rotationEulerAngleZ);
        public abstract void DeleteDecoration(int decorationID);
    }
}
