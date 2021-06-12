using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SimpleGltf.Enums;

namespace SimpleGltf.Json
{
    public class Sampler : IIndexable
    {
        internal Sampler(GltfAsset gltfAsset)
        {
            gltfAsset.Samplers ??= new List<Sampler>();
            Index = gltfAsset.Samplers.Count;
            gltfAsset.Samplers.Add(this);
        }
        
        [JsonIgnore] public int Index { get; }

        public WrappingMode? WrapS { get; init; }

        public WrappingMode? WrapT { get; init; }
    }
}