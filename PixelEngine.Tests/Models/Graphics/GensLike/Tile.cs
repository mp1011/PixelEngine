namespace PixelEngine.Tests.Models.Graphics.GensLike;

[TestFixture]
class TileTest
{
    [Test]
    public void CanCreateTileWithProperties()
    {
        var tile = new Tile(index: 88, priority: true, flipV: true, flipH:false, paletteIndex: PaletteIndex.P3);
        Assert.That(tile.Index, Is.EqualTo(88));
        Assert.That(tile.FlipH, Is.False);
        Assert.That(tile.FlipV, Is.True);
        Assert.That(tile.Priority, Is.True);
        Assert.That(tile.PaletteIndex, Is.EqualTo(PaletteIndex.P3));
    }
}
