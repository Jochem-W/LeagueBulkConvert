using System;
using System.Text.Json.Serialization;
using SimpleGltf.Enums;
using SimpleGltf.Json.Converters;

namespace SimpleGltf.Json
{
    public class AnimationSampler
    {
        internal AnimationSampler(Animation animation, Accessor input, Accessor output)
        {
            if (input.Type != AccessorType.Scalar || input.ComponentType != ComponentType.Float)
                throw new ArgumentException("Input has to be a scalar accessor with floats!", nameof(input));
            animation.Samplers.Add(this);
            Input = input;
            Output = output;
        }

        [JsonIgnore] public Accessor Input { get; }

        [JsonPropertyName("input")] public int InputReference => Input.GltfAsset.Accessors.IndexOf(Input);

        [JsonConverter(typeof(InterpolationAlgorithmConverter))]
        public InterpolationAlgorithm Interpolation { get; set; }

        [JsonIgnore] public Accessor Output { get; }

        [JsonPropertyName("output")] public int OutputReference => Output.GltfAsset.Accessors.IndexOf(Output);
    }
}