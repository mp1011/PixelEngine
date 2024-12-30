public class CollisionManager
{
    private readonly Specs _specs;
    private readonly CoordinateTranslator _coordinateTranslator;
    private readonly CollisionMap _collisionMap;

    public CollisionManager(CollisionMap collisionMap, ScrollingLayer layer, Specs specs)
    {
        _coordinateTranslator = new CoordinateTranslator(layer);
        _coordinateTranslator.ScrollSeamCorner = new Point(0, 0);
        _collisionMap = collisionMap;
        _specs = specs;
    }

    public CollisionResponse CheckBlockCollision(MovingSprite movingSprite, SpriteCollider collider)
    {
        var response = new CollisionResponse();

        var worldPosition = _coordinateTranslator.SpriteToWorld(movingSprite.Sprite);
        var topLeftTile = worldPosition / _specs.TileSize;
        var bottomRightTile = worldPosition.Add(movingSprite.PixelWidth, movingSprite.PixelHeight) / _specs.TileSize;

        var verticalHitbox = collider.GetVerticalHitbox(worldPosition);
        var horizontalHitbox = collider.GetHorizontalHitbox(worldPosition);

        _collisionMap.ForEach(
            from: topLeftTile,
            to: bottomRightTile,
            (x, y) =>
            {
                if (_collisionMap[x, y] == CollisionType.Solid)
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
                            var newPos = _coordinateTranslator.WorldToSprite(worldPosition);
                            movingSprite.RealY = newPos.Y;

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
                            var newPos = _coordinateTranslator.WorldToSprite(worldPosition);
                            movingSprite.RealX = newPos.X;

                            verticalHitbox = collider.GetVerticalHitbox(worldPosition);
                            horizontalHitbox = collider.GetHorizontalHitbox(worldPosition);
                        }
                    }
                }
            });



        return response;
    }
}

