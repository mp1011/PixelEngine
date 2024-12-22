public interface IRasterInterupt
{
    void OnHBlank(Layer layer, int renderY);
}

public class TestRasterInterupt : IRasterInterupt
{
    private RenderService _renderService;
    Color[] _baseColors;

    public TestRasterInterupt(RenderService renderService)
    {
        _renderService = renderService;
    }

    public void OnHBlank(Layer layer, int renderY)
    {
        var palette = _renderService.Palette(0);
        if (_baseColors == null)
        {
            _baseColors = Enumerable.Range(0, palette.Length)
                .Select(p => palette[p])
                .ToArray();
        }

        for (int i = 0; i < palette.Length; i++)
            AdjustColor(i, renderY, palette);       
    }

    private void AdjustColor(int index, int renderY, Palette palette)
    {
        var newColor = new Color(
           (_baseColors[index].R - (renderY / 1)).Clamp(0, 255),
           (_baseColors[index].G - (renderY / 1)).Clamp(0, 255),
           (_baseColors[index].B - (renderY / 1)).Clamp(0, 255));

        palette.SetColor(index, newColor);
    }
}
