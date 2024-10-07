public interface IRenderStrategy
{
    void Initialize(GraphicsDevice device, Specs specs);
    void OnFrameUpdate();
    void Draw(GraphicsDevice device, SpriteBatch spriteBatch);
}

public class CanvasRenderStrategy : IRenderStrategy
{
    private Texture2D _canvas;
    private readonly RenderService<VertexPositionColor> _renderService;

    public CanvasRenderStrategy(RenderService<VertexPositionColor> renderService)
    {
        _renderService = renderService;
    }

    public void Draw(GraphicsDevice device, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_canvas, Vector2.Zero, XnaColor.White);
        spriteBatch.End();
    }

    public void Initialize(GraphicsDevice device, Specs specs)
    {
        _canvas = new Texture2D(device, specs.ScreenWidth, specs.ScreenHeight);
    }

    public void OnFrameUpdate()
    {
        throw new System.NotImplementedException();
        //_canvas.SetData(_renderService.ColorData.ToArray());
    }
}

public class PixelTextureRenderStrategy : IRenderStrategy
{
    private readonly RenderService<VertexPositionColor> _renderService;
    private Specs _specs;
    private Texture2D _pixel;
    public PixelTextureRenderStrategy(RenderService<VertexPositionColor> renderService)
    {
        _renderService = renderService;
    }

    public void Draw(GraphicsDevice device, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        for (int y = 0; y < _specs.ScreenHeight; y++)
        {
            for (int x = 0; x < _specs.ScreenWidth; x++)
            {
                spriteBatch.Draw(_pixel, new Vector2(x, y), _renderService.ColorData[x, y].Color);
            }
        }
        spriteBatch.End();
    }

    public void Initialize(GraphicsDevice device, Specs specs)
    {
        _specs = specs;
        _pixel = new Texture2D(device, 1, 1);
        _pixel.SetData(new XnaColor[] { XnaColor.White });
    }

    public void OnFrameUpdate()
    {
    }
}

public class PrimitivesRenderStrategy : IRenderStrategy
{
    private BasicEffect _basicEffect;
    private readonly RenderService<VertexPositionColor> _renderService;

    public PrimitivesRenderStrategy(RenderService<VertexPositionColor> renderService)
    {
        _renderService = renderService;
    }
        
    public void Draw(GraphicsDevice device, SpriteBatch spriteBatch)
    {
        var vertices = _renderService.ColorData.ToArray();
        _basicEffect.CurrentTechnique.Passes[0].Apply();
        device.DrawUserPrimitives(PrimitiveType.PointList, vertices, 0, vertices.Length);
    }

    public void Initialize(GraphicsDevice device, Specs specs)
    {       
        _basicEffect = new BasicEffect(device)
        {
            VertexColorEnabled = true, 
            Projection = Matrix.CreateOrthographicOffCenter(0, 
                specs.ScreenWidth,
                specs.ScreenHeight,
                0,
                0,
                1)
        };
    }

    public void OnFrameUpdate()
    {
    }
}