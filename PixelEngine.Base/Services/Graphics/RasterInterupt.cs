public interface IRasterInterupt
{
    void OnHBlank(Layer layer, int renderY);
}

class TestRasterInterupt : IRasterInterupt
{
    private double _value = 0;

    public void OnHBlank(Layer layer, int renderY)
    {
        var v = _value + (renderY*10.0) / 180.0;
        layer.Scroll.X = (int)((renderY/2) + Math.Sin(v)*8);

        if (renderY == 0)
        {
            _value += 0.1;
            if (_value > Math.PI * 2)
            {
                _value -= Math.PI * 2;
            }
        }
    }
}
