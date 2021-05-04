using System;
using System.Collections.Generic;
using SimpleGltf.Enums;

namespace SimpleGltf.Json
{
    public class Sampler
    {
        private readonly GltfAsset _gltfAsset;

        private Sampler(GltfAsset gltfAsset)
        {
        }

        internal Sampler(GltfAsset gltfAsset, ScaleFilter? magFilter, ScaleFilter? minFilter, WrappingMode wrapS,
            WrappingMode wrapT, string name) : this(gltfAsset)
        {
            if (magFilter != ScaleFilter.Linear && magFilter != ScaleFilter.Nearest)
                throw new ArgumentOutOfRangeException(nameof(magFilter), magFilter,
                    "Only Linear and Nearest ScaleFilter are allowed for magFilter!");
            _gltfAsset = gltfAsset;
            _gltfAsset.Samplers ??= new List<Sampler>();
            _gltfAsset.Samplers.Add(this);
            MagFilter = magFilter;
            MinFilter = minFilter;
            WrapS = wrapS == WrappingMode.Repeat ? wrapS : null;
            WrapT = wrapT == WrappingMode.Repeat ? wrapT : null;
            Name = name;
        }

        public ScaleFilter? MagFilter { get; }

        public ScaleFilter? MinFilter { get; }

        public WrappingMode? WrapS { get; }

        public WrappingMode? WrapT { get; }

        public string Name { get; }
    }
}