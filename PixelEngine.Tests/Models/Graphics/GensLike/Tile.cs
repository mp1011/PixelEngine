namespace PixelEngine.Tests.Models.Graphics.GensLike;

[TestFixture]
class TileTest
{
    [Test]
    public void CanCreateTileWithProperties()
    {
        var tile = new Tile(777, TileFlags.FlipX | TileFlags.Priority, PaletteIndex.P3);
        Assert.That(tile.Index, Is.EqualTo(777));
        Assert.That(tile.Flags, Is.EqualTo(TileFlags.FlipX | TileFlags.Priority));
        Assert.That(tile.PaletteIndex, Is.EqualTo(PaletteIndex.P3));

    }
}
