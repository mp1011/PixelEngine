public record SpriteCollider(Rectangle HorizontalCollider, Rectangle VerticalCollider)
{
    public Rectangle GetHorizontalHitbox(Point worldPosition) => 
        new Rectangle(worldPosition.X + HorizontalCollider.X,
            worldPosition.Y + HorizontalCollider.Y,
            HorizontalCollider.Width, 
            HorizontalCollider.Height);

    public Rectangle GetVerticalHitbox(Point worldPosition) =>
       new Rectangle(worldPosition.X + VerticalCollider.X,
           worldPosition.Y + VerticalCollider.Y,
           VerticalCollider.Width,
           VerticalCollider.Height);
}