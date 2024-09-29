

namespace PixelEngine.Tests.Models
{
    [TestFixture]
    class NBitArrayTests
    {
        [Test]
        public void CanSetValuesTo4BitArray()
        {
            var array = new NBitArray(4, 100);

            for(int i = 0; i < 50;i++)
                array[i] = (byte)(i % 16);

            for (int i = 0; i < 50; i++)
                Assert.That(array[i], Is.EqualTo((byte)(i % 16)));
        }
    }
}
