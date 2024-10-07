namespace PixelEngine.Tests.Models.Graphics.GensLike;

[TestFixture]
class TileTest
{
    [Test]
    public void CanCreateTileWithProperties()
    {
        var tile = new Tile(13, 98, TileFlags.FlipX | TileFlags.Priority, PaletteIndex.P3);
        Assert.That(tile.X, Is.EqualTo(13));
        Assert.That(tile.Y, Is.EqualTo(98));
        Assert.That(tile.Flags, Is.EqualTo(TileFlags.FlipX | TileFlags.Priority));
        Assert.That(tile.PaletteIndex, Is.EqualTo(PaletteIndex.P3));

    }
}
