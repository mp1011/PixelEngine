namespace PixelEngine.Tests.Services;

[TestFixture]
class GensVramImporterTests
{
    [Test]
    public void CanImportVram()
    {
        var bytes = File.ReadAllBytes($@"{TestContext.CurrentContext.TestDirectory}/Fixtures/samplevram.ram");
        var tiles = GensVramImporter.Import(bytes);

        Assert.That(tiles[0,0], Is.EqualTo(0));
        Assert.That(tiles[1, 0], Is.EqualTo(0));
        Assert.That(tiles[2, 0], Is.EqualTo(11));
        Assert.That(tiles[3, 0], Is.EqualTo(12));

        Assert.That(tiles[4, 1], Is.EqualTo(0));
        Assert.That(tiles[5, 1], Is.EqualTo(1));
        Assert.That(tiles[6, 1], Is.EqualTo(9));
        Assert.That(tiles[7, 1], Is.EqualTo(7));

        Assert.That(tiles[0, 3], Is.EqualTo(0));
        Assert.That(tiles[1, 3], Is.EqualTo(0));
        Assert.That(tiles[2, 3], Is.EqualTo(13));
        Assert.That(tiles[3, 3], Is.EqualTo(0));

        Assert.That(tiles[0, 7], Is.EqualTo(0));
        Assert.That(tiles[1, 7], Is.EqualTo(0));
        Assert.That(tiles[2, 7], Is.EqualTo(11));
        Assert.That(tiles[3, 7], Is.EqualTo(12));
        Assert.That(tiles[4, 7], Is.EqualTo(0));
        Assert.That(tiles[5, 7], Is.EqualTo(0));
        Assert.That(tiles[6, 7], Is.EqualTo(11));
        Assert.That(tiles[7, 7], Is.EqualTo(12));
    }
}
