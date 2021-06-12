using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SimpleGltf.Json
{
    public class Mesh : IIndexable
    {
        internal Mesh(GltfAsset gltfAsset)
        {
            gltfAsset.Meshes ??= new List<Mesh>();
            Index = gltfAsset.Meshes.Count;
            Primitives = new List<Primitive>();
            gltfAsset.Meshes.Add(this);
        }
        
        [JsonIgnore] public int Index { get; }

        public IList<Primitive> Primitives { get; }
    }
}