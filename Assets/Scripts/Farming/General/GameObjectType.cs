namespace VitsehLand.Scripts.Farming.General
{
    public static class GameObjectType
    {
        public enum FilteredType
        {
            Crop = 0,
            FarmingProduct = 1,
            NaturalResource = 2,
            Power = 3,
            Water = 4,
            Fertilizer = 5,
        }

        public enum FeaturedType
        {
            None = -1, // Like water
            Product = 1, // GameObject from crafting
            Normal = 2 // GameObject from farming, gathering, rewarding
        }
    }
}