
using SFML.Graphics;

class CollidersOverlay : IDisplayOverlay
{
    private SFML.Graphics.Color _hColor, _vColor;
    
    public CollidersOverlay()
    {
        _hColor = new SFML.Graphics.Color(255, 0, 0, 150);
        _vColor = new SFML.Graphics.Color(0, 0, 255, 150);
    }

    public void Draw(RenderWindow window, float xScale, float yScale)
    {
        foreach(var colliderWatch in Debug.WatchedColliders)
        {
            var horizontalHitbox = colliderWatch.Collider.GetHorizontalHitbox(colliderWatch.Sprite.WorldLocation);
            var verticalHitbox = colliderWatch.Collider.GetVerticalHitbox(colliderWatch.Sprite.WorldLocation);

            var hScreenPos = Debug.CoordinateTranslator.WorldToScreen(new Point(horizontalHitbox.X, horizontalHitbox.Y));
            var vScreenPos = Debug.CoordinateTranslator.WorldToScreen(new Point(verticalHitbox.X, verticalHitbox.Y));

            var shapeH = new RectangleShape(new Vector2f(horizontalHitbox.Width * xScale, horizontalHitbox.Height * yScale));
            shapeH.Position = new Vector2f(hScreenPos.X * xScale, hScreenPos.Y * yScale);
            shapeH.FillColor = _hColor;
            window.Draw(shapeH);

            var shapeV = new RectangleShape(new Vector2f(verticalHitbox.Width * xScale, verticalHitbox.Height * yScale));
            shapeV.Position = new Vector2f(vScreenPos.X * xScale, vScreenPos.Y * yScale);
            shapeV.FillColor = _vColor;
            window.Draw(shapeV);
        }
    }
}

