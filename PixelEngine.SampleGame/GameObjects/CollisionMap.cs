public enum CollisionType
{
    None=0,
    Solid
}

public class CollisionMap : ArrayDataGrid<CollisionType>
{
    public CollisionMap(int width, int height) : base(width, height)
    {
    }
}