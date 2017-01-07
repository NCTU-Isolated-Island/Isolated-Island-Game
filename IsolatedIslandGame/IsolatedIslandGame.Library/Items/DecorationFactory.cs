namespace IsolatedIslandGame.Library.Items
{
    public abstract class DecorationFactory
    {
        public static DecorationFactory Instance { get; private set; }
        public static void Initial(DecorationFactory factory)
        {
            Instance = factory;
        }

        public abstract bool CreateDecoration(int vesselID, Material material, float positionX, float positionY, float positionZ, float rotationEulerAngleX, float rotationEulerAngleY, float rotationEulerAngleZ, out Decoration decoration);
        public abstract void DeleteDecoration(int decorationID);
    }
}
