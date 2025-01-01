public class WaterWaver
{
    private ScrollingLayer _layer;
    private int _yStart, _yEnd;
    private double _animationValue = 0;

    public WaterWaver(ScrollingLayer layer, int yStart, int yEnd)
    {
        _yStart = yStart;
        _yEnd = yEnd;
        _layer = layer;
    }

    public void Update()
    {
        _animationValue += 0.05;

        int scrollOffset = _layer.VScrollTable.Values[0];

        double relY = 0;
        for(int y = _yStart; y <= _yEnd; y++)
        {
            _layer.HScrollTable.Values[(y - scrollOffset).NMod(_layer.HScrollTable.Values.Length)] += (short)(4 * Math.Sin(y + _animationValue));

            relY += 0.001;
        }
    }
}

