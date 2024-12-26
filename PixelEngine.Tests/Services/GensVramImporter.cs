namespace PixelEngine.Tests.Services;

[TestFixture]
class GensVramImporterTests
{
    [Test]
    public void CanImportVram()
    {
        var bytes = File.ReadAllBytes($@"{TestContext.CurrentContext.TestDirectory}/Fixtures/samplevram.ram");
        var tiles = new PatternTable(Specs.GensLike);
        tiles.SetData(bytes);

        Assert.That(tiles[0], Is.EqualTo(0));
        Assert.That(tiles[1], Is.EqualTo(0));
        Assert.That(tiles[2], Is.EqualTo(11));
        Assert.That(tiles[3], Is.EqualTo(12));

        Assert.That(tiles[4], Is.EqualTo(0));
        Assert.That(tiles[5], Is.EqualTo(1));
        Assert.That(tiles[6], Is.EqualTo(9));
        Assert.That(tiles[7], Is.EqualTo(7));

        Assert.That(tiles[12], Is.EqualTo(0));
        Assert.That(tiles[13], Is.EqualTo(0));
        Assert.That(tiles[14], Is.EqualTo(13));
        Assert.That(tiles[15], Is.EqualTo(0));
    }
}
