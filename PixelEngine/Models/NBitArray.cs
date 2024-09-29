using System;

namespace PixelEngine.Models
{
    public class NBitArray
    {
        private const int BitsPerInteger = 64;

        private readonly ulong _maskValue;
        private readonly int _valuesPerInteger;
        private readonly int _bitsPerValue;

        private ulong[] _data;

        public NBitArray(int bitsPerValue, int length)
        {
            _bitsPerValue = bitsPerValue;
            _valuesPerInteger = BitsPerInteger / bitsPerValue;
            _data = new ulong[length / _valuesPerInteger];
            _maskValue = (ulong)(Math.Pow(2, bitsPerValue) - 1);
        }

        public void SetData(byte[] data)
        {
            for(int dataIndex=0; dataIndex < data.Length/8; dataIndex++)
            {
                _data[dataIndex] = BitConverter.ToUInt64(data, dataIndex * 8);
            }
        }

        public byte this[int index] 
        {
            get
            {
                int arrayIndex = index / _valuesPerInteger;
                int integerIndex = index % _valuesPerInteger;

                var mask = _maskValue << (integerIndex * _bitsPerValue);
                return (byte)((_data[arrayIndex] & mask) >> (integerIndex * _bitsPerValue));
            }

            set
            {
                int arrayIndex = index / _valuesPerInteger;
                int integerIndex = index % _valuesPerInteger;

                var mask = ~(_maskValue << (integerIndex * _bitsPerValue));
                _data[arrayIndex] = _data[arrayIndex] & mask;

                ulong newValue = (ulong)((ulong)value << (integerIndex * _bitsPerValue));
                _data[arrayIndex] = (ulong)(_data[arrayIndex] | newValue);
            }
        }
    }
}
