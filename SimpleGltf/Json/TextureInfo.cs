namespace SimpleGltf.Json
{
    public class TextureInfo
    {
        private readonly Texture _texture;

        internal TextureInfo(Texture texture, int texCoord)
        {
            _texture = texture;
            TexCoord = texCoord;
        }

        public int TexCoord { get; }
    }
}