using Hexa.NET.ImGui;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;
using System.Text;

namespace EasyImGui;

public struct FontEx {
    public string Name;
    public float Size;
    public ImFontPtr FontPtr;
}

public struct Texture2D {
    public string Name;
    public Vector2 Size;
    public int Channels;
    public uint Texture;
}

/// Fonts
public unsafe static partial class EasyResourceMan {
    private static readonly List<FontEx> _fonts = [];
    public static IEnumerable<FontEx> GetFonts() => _fonts;

    /// <summary>
    /// Load font from byte[] and add it to fonts.
    /// </summary>
    public static unsafe void LoadFontEx(ReadOnlySpan<byte> data, int size, string name, nint glyphRanges = 0, bool merge = false) {
        var io = ImGui.GetIO();
        var cfg = ImGui.ImFontConfig() with {
            MergeMode = merge,
            OversampleH = 1,
            OversampleV = 1,
            FontData = ImGui.MemAlloc((uint)data.Length),
            FontDataSize = data.Length,
            GlyphRanges = glyphRanges != 0 ? (uint*)glyphRanges : io.Fonts.GetGlyphRangesDefault(),
            SizePixels = size,
            FontDataOwnedByAtlas = true
        };
        var nameData = new Span<byte>(Encoding.UTF8.GetBytes($"{name}, {size}px"));
        nameData.CopyTo(cfg.Name);
        data.CopyTo(new Span<byte>(cfg.FontData, data.Length));

        var font = io.Fonts.AddFont(cfg);
        _fonts.Add(new FontEx {
            Name = name,
            Size = size,
            FontPtr = font
        });
        io.Fonts.Build(); // build here because glyph ranges need to be allocated when calling Build or GetTexDataAsRGBA32
        cfg.Destroy();
    }

    /// <summary>
    /// Load font from byte[] and add it to fonts.
    /// </summary>
    /// <param name="data">Font Data</param>
    /// <param name="name">Font Name</param>
    /// <param name="sizes">Sizes to load</param>
    public static unsafe void LoadFontFromResx(ReadOnlySpan<byte> data, string name, params int[] sizes) {
        foreach(var i in sizes) {
            LoadFontEx(data, i, name);
        }
    }

    /// <summary>
    /// Get font by name and size.
    /// </summary>
    /// <param name="name">Font name</param>
    /// <param name="size">Font size</param>
    /// <returns>Font ptr. If size is 0 then first font with name is returned.</returns>
    public static ImFontPtr GetFont(string name, int size = 0) {
        if(size == 0) return _fonts.FirstOrDefault(font => font.Name == name).FontPtr;
        return _fonts.FirstOrDefault(font => font.Name == name && font.Size == size).FontPtr;
    }

}

/// Textures
public unsafe static partial class EasyResourceMan {

    private static readonly List<Texture2D> _textures = [];
    public static IEnumerable<Texture2D> GetTextures() => _textures;

    /// <summary>
    /// Create texture from byte*
    /// </summary>
    /// <param name="imageData">Image bytes</param>
    /// <param name="width">Image width</param>
    /// <param name="height">Image height</param>
    public static unsafe uint CreateTexture2D(byte* imageData, int width, int height, 
        PixelInternalFormat iFormat = PixelInternalFormat.Rgba,
        PixelFormat format = PixelFormat.Rgba,
        PixelType pixelType = PixelType.UnsignedByte) {
        GL.GenTextures(1, out uint texture);
        GL.BindTexture(TextureTarget.Texture2D, texture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, iFormat, width, height, 0, format, pixelType, (IntPtr)imageData);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        return texture;
    }

    /// <summary>
    /// Create texture from bytes. These should be preprocessed with something like StbImage for raw bytes.
    /// </summary>
    /// <param name="bytes">Image bytes</param>
    /// <param name="width">Image width</param>
    /// <param name="height">Image height</param>
    /// <param name="name">Image name</param>
    /// <param name="channels">Image channels, default = 4</param>
    public static Texture2D CreateTexture2D(ReadOnlySpan<byte> bytes, int width, int height, string name, int channels = 4) {
        fixed(byte* bytePtr = bytes) {
            var tex = CreateTexture2D(bytePtr, width, height);
            var texture = new Texture2D {
                Name = name,
                Size = new Vector2(width, height),
                Channels = channels,
                Texture = tex
            };
            _textures.Add(texture);
        }

        return GetTexture(name);
    }

    public static Texture2D GetTexture(string name) => _textures.FirstOrDefault(t => t.Name ==  name);

}
