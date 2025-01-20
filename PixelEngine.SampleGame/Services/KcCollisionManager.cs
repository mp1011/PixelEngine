class KcCollisionManager : CollisionManager
{
    private readonly TileProperties _tileProperties;

    public KcCollisionManager(LevelTileMap collisionMap, CoordinateTranslator coordinateTranslator, TileProperties tileProperties, Specs specs) 
        : base(collisionMap, coordinateTranslator, specs)
    {
        _tileProperties = tileProperties;
    }

    protected override bool IsTileCollidable(int x, int y)
    {
        return _tileProperties[_collisionMap.Tiles[x, y].Index] > TileType.Empty;
    }
}

