using System;
using System.IO;
using System.Numerics;
using System.Text.Json;
using System.Threading.Tasks;
using SimpleGltf.Enums;
using SimpleGltf.Extensions;

namespace SimpleGltf.Json.Extensions
{
    public static class GltfAssetExtensions
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            IgnoreNullValues = true
        };

        public static void SetDefaultScene(this GltfAsset gltfAsset, Scene scene)
        {
            gltfAsset.Scene = scene;
        }

        public static Accessor CreateAccessor(this GltfAsset gltfAsset, ComponentType componentType,
            AccessorType accessorType, bool normalized = false, string name = null, bool minMax = false)
        {
            return new(gltfAsset, componentType, accessorType, normalized, name, minMax);
        }

        public static Asset CreateAsset(this GltfAsset gltfAsset, string copyright = null)
        {
            return new(gltfAsset, copyright);
        }

        public static Buffer CreateBuffer(this GltfAsset gltfAsset, string name = null)
        {
            return new(gltfAsset, name);
        }

        public static BufferView CreateBufferView(this GltfAsset gltfAsset, Buffer buffer, BufferViewTarget target,
            string name = null)
        {
            return new(gltfAsset, buffer, target, name);
        }

        public static Image CreateImage(this GltfAsset gltfAsset, string uri, string name = null)
        {
            return new(gltfAsset, uri, name);
        }

        public static Image CreateImage(this GltfAsset gltfAsset, BufferView bufferView, MimeType mimeType,
            string name = null)
        {
            return new(gltfAsset, bufferView, mimeType, name);
        }

        public static Material CreateMaterial(this GltfAsset gltfAsset, Vector3? emissiveFactor = null,
            AlphaMode? alphaMode = null, float? alphaCutoff = null, bool? doubleSided = null, string name = null)
        {
            return new(gltfAsset, emissiveFactor, alphaMode, alphaCutoff, doubleSided, name);
        }

        public static Mesh CreateMesh(this GltfAsset gltfAsset, string name = null)
        {
            return new(gltfAsset, name);
        }

        public static Node CreateNode(this GltfAsset gltfAsset, string name = null)
        {
            return CreateNode(gltfAsset, Quaternion.Identity, Vector3.One, Vector3.Zero, name);
        }

        public static Node CreateNode(this GltfAsset gltfAsset, Quaternion rotation, Vector3 scale,
            Vector3 translation, string name = null)
        {
            return new(gltfAsset, rotation, scale, translation, name);
        }

        public static Sampler CreateSampler(this GltfAsset gltfAsset, ScaleFilter? magFilter = null,
            ScaleFilter? minFilter = null, WrappingMode wrapS = WrappingMode.Repeat,
            WrappingMode wrapT = WrappingMode.Repeat, string name = null)
        {
            return new(gltfAsset, magFilter, minFilter, wrapS, wrapT, name);
        }

        public static Scene CreateScene(this GltfAsset gltfAsset, string name = null, bool setDefault = true)
        {
            var scene = new Scene(gltfAsset, name);
            if (setDefault)
                gltfAsset.SetDefaultScene(scene);
            return scene;
        }

        public static Texture CreateTexture(this GltfAsset gltfAsset, Sampler sampler, Image image,
            string name = null)
        {
            return new(gltfAsset, sampler, image, name);
        }

        public static Task Save(this GltfAsset gltfAsset, string path)
        {
            return Path.GetExtension(path) switch
            {
                ".glb" => gltfAsset.SaveGltfBinary(path),
                ".gltf" => gltfAsset.SaveGltfEmbedded(path),
                _ => gltfAsset.SaveGltf(path)
            };
        }

        public static async Task SaveGltfBinary(this GltfAsset gltfAsset, string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await using var binaryWriter = new BinaryWriter(File.Create(filePath));
            binaryWriter.Write("glTF".ToMagic());
            binaryWriter.Write((uint) 2);
            binaryWriter.Seek(4, SeekOrigin.Current);

            await WriteChunk(binaryWriter, gltfAsset);

            var lastBuffer = gltfAsset.Buffers[^1];
            foreach (var buffer in gltfAsset.Buffers)
            {
                await using var stream = await buffer.GetStreamAsync();
                await WriteChunk(binaryWriter, "BIN", stream, buffer == lastBuffer);
            }

            binaryWriter.Seek(8, SeekOrigin.Begin);
            binaryWriter.Write((uint) binaryWriter.BaseStream.Length);
        }

        //TODO: merge the following two functions
        private static async Task WriteChunk(BinaryWriter binaryWriter, string magic, Stream data, bool last = false)
        {
            var headerStart = binaryWriter.BaseStream.Position;
            binaryWriter.Seek(4, SeekOrigin.Current);
            binaryWriter.Write(magic.ToMagic());
            var start = binaryWriter.BaseStream.Position;
            await data.CopyToAsync(binaryWriter.BaseStream);
            if (!last)
                binaryWriter.Seek((int) binaryWriter.BaseStream.Position.GetOffset(), SeekOrigin.Current);
            var end = binaryWriter.BaseStream.Position;
            var length = end - start;
            binaryWriter.Seek((int) headerStart, SeekOrigin.Begin);
            binaryWriter.Write((uint) length);
            binaryWriter.Seek((int) end, SeekOrigin.Begin);
        }

        private static async Task WriteChunk(BinaryWriter binaryWriter, GltfAsset gltfAsset, bool last = false)
        {
            var headerStart = binaryWriter.BaseStream.Position;
            binaryWriter.Seek(4, SeekOrigin.Current);
            binaryWriter.Write("JSON".ToMagic());
            var start = binaryWriter.BaseStream.Position;
            await JsonSerializer.SerializeAsync(binaryWriter.BaseStream, gltfAsset, JsonSerializerOptions);
            if (!last)
            {
                var offset = binaryWriter.BaseStream.Position.GetOffset();
                for (var i = 0; i < offset; i++)
                    binaryWriter.Write(' ');
            }

            var end = binaryWriter.BaseStream.Position;
            var length = end - start;
            binaryWriter.Seek((int) headerStart, SeekOrigin.Begin);
            binaryWriter.Write((uint) length);
            binaryWriter.Seek((int) end, SeekOrigin.Begin);
        }

        public static async Task SaveGltfEmbedded(this GltfAsset gltfAsset, string filePath)
        {
            throw new NotImplementedException();
        }

        public static async Task SaveGltf(this GltfAsset gltfAsset, string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}