public static class GameObjectType
{
    public enum FilteredType
    {
        Crop = 0,
        NaturePlant = 1,
        Fertilizer = 2,
        NaturalResource = 3,
        Power = 4
    }

    public enum FeaturedType
    {
        None = -1, // Like water
        Product = 1, // GameObject from crafting
        Normal = 2 // GameObject from farming, gathering, rewarding
    }
}
