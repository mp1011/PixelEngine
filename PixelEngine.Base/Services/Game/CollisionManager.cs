public abstract class CollisionManager
{
    private readonly Specs _specs;
    private readonly CoordinateTranslator _coordinateTranslator;
    protected readonly LevelTileMap _collisionMap;

    public CollisionManager(LevelTileMap collisionMap, CoordinateTranslator coordinateTranslator, Specs specs)
    {
        _coordinateTranslator = coordinateTranslator;
        _coordinateTranslator.ScrollSeamCorner = new Point(0, 0);
        _collisionMap = collisionMap;
        _specs = specs;
    }

    public CollisionResponse CheckBlockCollision(MovingSprite movingSprite, SpriteCollider collider)
    {
        var response = new CollisionResponse();

        var worldPosition = movingSprite.WorldLocation;
        var topLeftTile = worldPosition / _specs.TileSize;
        var bottomRightTile = worldPosition.Add(movingSprite.PixelWidth, movingSprite.PixelHeight) / _specs.TileSize;

        var verticalHitbox = collider.GetVerticalHitbox(worldPosition);
        var horizontalHitbox = collider.GetHorizontalHitbox(worldPosition);

        _collisionMap.Tiles.ForEach(
            from: topLeftTile,
            to: bottomRightTile,
            (x, y) =>
            {
                if (IsTileCollidable(x,y))
                {
                    var tileHitbox = new Rectangle(x * _specs.TileSize, y * _specs.TileSize, _specs.TileSize, _specs.TileSize);
                    int xCorrection=0, yCorrection=0;

                    if(tileHitbox.Intersects(verticalHitbox))
                    {
                        int bottomOverlap = verticalHitbox.Bottom - tileHitbox.Top;
                        int topOverlap = tileHitbox.Bottom - verticalHitbox.Top;

                        if(bottomOverlap > 0 && bottomOverlap < topOverlap)
                        {
                            yCorrection = -bottomOverlap;
                        }
                        else if(topOverlap > 0 && topOverlap < bottomOverlap)
                        {
                            yCorrection = topOverlap;
                        }

                        if(yCorrection != 0)
                        {
                            movingSprite.VerticalMotion.Speed = 0;
                            movingSprite.VerticalMotion.Target = 0;

                            worldPosition = worldPosition.Add(0, yCorrection);
                            movingSprite.WorldY = worldPosition.Y;

                            verticalHitbox = collider.GetVerticalHitbox(worldPosition);
                            horizontalHitbox = collider.GetHorizontalHitbox(worldPosition);
                        }

                        if (verticalHitbox.Bottom == tileHitbox.Top)
                            response.IsOnGround = true;
                    }

                    if (tileHitbox.Intersects(horizontalHitbox))
                    {
                        int rightOverlap = horizontalHitbox.Right - tileHitbox.Left;
                        int leftOverlap = tileHitbox.Right - horizontalHitbox.Left;

                        if (rightOverlap > 0 && rightOverlap < leftOverlap)
                        {
                            xCorrection = -rightOverlap;
                        }
                        else if (leftOverlap > 0 && leftOverlap < rightOverlap)
                        {
                            xCorrection = leftOverlap;
                        }

                        if (xCorrection != 0)
                        {
                            movingSprite.HorizontalMotion.Speed = 0;
                            movingSprite.HorizontalMotion.Target = 0;

                            worldPosition = worldPosition.Add(xCorrection, 0);
                            movingSprite.WorldX = worldPosition.X;

                            verticalHitbox = collider.GetVerticalHitbox(worldPosition);
                            horizontalHitbox = collider.GetHorizontalHitbox(worldPosition);
                        }
                    }
                }
            });



        return response;
    }

    protected abstract bool IsTileCollidable(int x, int y);
}

