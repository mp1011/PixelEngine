namespace PixelEngine.Tests.Services;

[TestFixture]
class BitStreamReaderTests
{
    [Test]
    public void CanReadBitsFromByteArray()
    {
        var bytes = new byte[] { 31, 11 };

        var reader = new BitStreamReader(bytes);
        Assert.That(reader.ReadNextBits(3), Is.EqualTo(7));
        Assert.That(reader.ReadNextBits(3), Is.EqualTo(3));
        Assert.That(reader.ReadNextBits(3), Is.EqualTo(4));
        Assert.That(reader.ReadNextBits(3), Is.EqualTo(5));

    }
}