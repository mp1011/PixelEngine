namespace PixelEngine.Tests.Models;

[TestFixture]
class DataGridTests
{
    [Test]
    public void CanSetAndReadGridValues()
    {
        var grid = new BitArrayDataGrid(8, 8, new NBitArray(4,64));

        grid[6, 7] = 14;
        Assert.That(grid[6,7], Is.EqualTo(14));
    }
}
