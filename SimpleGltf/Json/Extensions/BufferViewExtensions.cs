using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SimpleGltf.Extensions;

namespace SimpleGltf.Json.Extensions
{
    public static class BufferViewExtensions
    {
        public static IEnumerable<Accessor> GetAccessors(this BufferView bufferView)
        {
            return bufferView.Buffer.GltfAsset.Accessors.Where(accessor => accessor.BufferView == bufferView);
        }

        public static int? GetByteOffset(this BufferView bufferView)
        {
            var take = true;
            var byteOffset = bufferView.Buffer.GetBufferViews().TakeWhile(bV =>
            {
                if (bV == bufferView)
                    take = false;
                return take;
            }).GetLength();
            if (byteOffset == 0)
                return null;
            return byteOffset + byteOffset.GetOffset();
        }

        public static int GetLength(this IEnumerable<BufferView> bufferViews)
        {
            var length = 0;
            foreach (var bufferView in bufferViews)
            {
                length += length.GetOffset();
                length += bufferView.ByteLength;
            }

            return length;
        }

        public static async Task<Stream> GetStreamAsync(this BufferView bufferView)
        {
            if (bufferView.PngStream != null)
            {
                var pngStream = new MemoryStream();
                bufferView.PngStream.Seek(0, SeekOrigin.Begin);
                await bufferView.PngStream.CopyToAsync(pngStream);
                pngStream.Seek(0, SeekOrigin.Begin);
                return pngStream;
            }

            var stream = new MemoryStream();
            var stride = bufferView.ByteStride;
            var accessors = bufferView.GetAccessors().ToList();
            accessors.SeekToBegin();
            if (stride == null)
            {
                foreach (var accessor in accessors)
                    await accessor.BinaryWriter.BaseStream.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }

            var totalLength = accessors.GetLength();
            while (stream.Length != totalLength)
                foreach (var accessor in accessors)
                {
                    var memory = new byte[accessor.ComponentSize];
                    await accessor.BinaryWriter.BaseStream.ReadAsync(memory);
                    await stream.WriteAsync(memory);
                }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}