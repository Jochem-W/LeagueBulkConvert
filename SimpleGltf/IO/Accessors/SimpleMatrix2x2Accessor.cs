using SimpleGltf.Converters;
using SimpleGltf.Enums;
using SimpleGltf.Extensions;
using SimpleGltf.Json;

namespace SimpleGltf.IO.Accessors
{
    public class SimpleMatrix2x2Accessor<T> : SimpleAccessor
    {
        internal SimpleMatrix2x2Accessor(BufferView bufferView, bool minMax = false, bool? normalized = null) : base(
            bufferView,
            AccessorComponentTypeConverter.Convert(typeof(T)), AccessorType.Matrix2x2, minMax, normalized)
        {
            var componentSize = AccessorComponentTypeConverter.GetSize(AccessorComponentType);
            Size = 2 * componentSize;
            Size = Size.Offset();
            Size += 2 * componentSize;
            Size = Size.Offset();
        }

        public void Write(T m11, T m12, T m21, T m22)
        {
            EnsureOffset();
            Accessor.Write((dynamic) m11);
            Accessor.Write((dynamic) m21);

            EnsureOffset();
            Accessor.Write((dynamic) m12);
            Accessor.Write((dynamic) m22);

            Accessor.Count++;
            Accessor.NextComponent();
        }
    }
}