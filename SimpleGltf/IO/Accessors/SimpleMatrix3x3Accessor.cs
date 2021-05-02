using SimpleGltf.Converters;
using SimpleGltf.Enums;
using SimpleGltf.Extensions;
using SimpleGltf.Json;

namespace SimpleGltf.IO.Accessors
{
    public class SimpleMatrix3x3Accessor<T> : SimpleAccessor
    {
        internal SimpleMatrix3x3Accessor(BufferView bufferView, bool minMax = false, bool? normalized = null) : base(
            bufferView,
            AccessorComponentTypeConverter.Convert(typeof(T)), AccessorType.Matrix3x3, minMax, normalized)
        {
            var componentSize = AccessorComponentTypeConverter.GetSize(AccessorComponentType);
            Size = 3 * componentSize;
            Size = Size.Offset();
            Size += 3 * componentSize;
            Size = Size.Offset();
            Size += 3 * componentSize;
            Size = Size.Offset();
        }

        public void Write(
            T m11, T m12, T m13,
            T m21, T m22, T m23,
            T m31, T m32, T m33)
        {
            EnsureOffset();
            Accessor.Write((dynamic) m11);
            Accessor.Write((dynamic) m21);
            Accessor.Write((dynamic) m31);

            EnsureOffset();
            Accessor.Write((dynamic) m12);
            Accessor.Write((dynamic) m22);
            Accessor.Write((dynamic) m32);

            EnsureOffset();
            Accessor.Write((dynamic) m13);
            Accessor.Write((dynamic) m23);
            Accessor.Write((dynamic) m33);

            Accessor.Count++;
            Accessor.NextComponent();
        }
    }
}