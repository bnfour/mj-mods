using System.IO;

using MelonLoader;
using UnityEngine;

namespace Bnfour.MoeJigsawMods.CustomBackground.Utilities;

/// <summary>
/// Manages the custom images used within the mod.
/// </summary>
/// <remarks>
/// Only ready to use Sprites are available outside this class. It encapsulates
/// all the horrors^W technical stuff.
/// </remarks>
internal class SpriteProvider
{
    // this is of an utmost importance, or custom images would be rendered at 1% the size
    // took me way too long to find out
    private const float PixelsPerUnit = 1f;

    private const int ThumbSize = 44;
    private const int ThumbWidth = 78;

    private const string StreamPathPrefix = "Bnfour.MoeJigsawMods.CustomBackground.Resources.";
    private readonly string CustomImagePath = Path.Combine(Application.dataPath, "bg.png");

    private readonly byte[] _rawBg;

    // used to warn/notify the user the default image can be replaced
    private readonly bool _wasPlaceholderUsed;

    // backing fields for lazy loading

    private Sprite _background;
    private Sprite _buttonNormal;
    private Sprite _buttonPressed;

    internal Sprite Background => _background ??= LoadBackground(_rawBg);
    internal Sprite ButtonNormal => _buttonNormal ??= CreateButtonSprite("normal");
    internal Sprite ButtonPressed => _buttonPressed ??= CreateButtonSprite("pressed");

    internal SpriteProvider()
    {
        // TODO the naming is meh
        _wasPlaceholderUsed = EnsureImageExists();
        _rawBg = File.ReadAllBytes(CustomImagePath);
    }

    internal void WarnIfNeeded()
    {
        if (_wasPlaceholderUsed)
        {
            Melon<CustomBackgroundMod>.Logger.Warning($"Custom image not found, writing default fallback to {CustomImagePath}. Replace it with your own image.");
        }
    }

    // returns whether the default was written
    private bool EnsureImageExists()
    {
        if (!File.Exists(CustomImagePath))
        {
            File.WriteAllBytes(CustomImagePath, LoadRawPngFromAssembly("bg-placeholder.png"));
            return true;
        }
        return false;
    }

    private byte[] LoadRawPngFromAssembly(string name)
    {
        using (var stream = GetType().Assembly.GetManifestResourceStream(StreamPathPrefix + name))
        using (MemoryStream ms = new())
        {
            // imagine using .NET Framework 3.5 in current year
            var buffer = new byte[16 * 1024];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            return ms.ToArray();
        }
    }

    private Sprite LoadBackground(byte[] data)
    {
        var texture = new Texture2D(1, 1, TextureFormat.RGB24, false);
        ImageConversion.LoadImage(texture, data);

        if (!(texture.width == 1920 && texture.height == 1080))
        {
            Melon<CustomBackgroundMod>.Logger.Warning("1920x1080 images work best for backgrounds.");
        }
        // note the non-center pivot
        return Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(0, 0), PixelsPerUnit);
    }

    private Sprite CreateButtonSprite(string stateName)
    {
        var bg = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        ImageConversion.LoadImage(bg, LoadRawPngFromAssembly($"{stateName}-backdrop.png"));

        var mask = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        ImageConversion.LoadImage(mask, LoadRawPngFromAssembly($"{stateName}-mask.png"));

        Blend(bg, PrepareThumbnail(Background.texture, mask));

        return Sprite.Create(bg, new(0, 0, bg.width, bg.height), new(0.5f, 0.5f), PixelsPerUnit);
    }

    // scale down, apply alpha mask
    private Texture2D PrepareThumbnail(Texture2D source, Texture2D mask)
    {
        var prev = RenderTexture.active;

        var target = new RenderTexture(ThumbWidth, ThumbSize, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(source, target);

        var result = new Texture2D(mask.width, mask.height, TextureFormat.ARGB32, false);
        result.ReadPixels(new((ThumbWidth - ThumbSize) / 2, 0, ThumbSize, ThumbSize),
        // no idea why, but this dest coords make thumb perfect between the two states
            mask.width - ThumbSize, 0, false);

        var alphaSource = mask.GetPixels();
        var colorSource = result.GetPixels();

        var masked = new Color[alphaSource.Length];
        for (int i = 0; i < masked.Length; i++)
        {
            masked[i] = new(colorSource[i].r, colorSource[i].g, colorSource[i].b, alphaSource[i].a);
        }
        result.SetPixels(masked);

        result.Apply(false, false);

        RenderTexture.active = prev;
        target.Release();
        UnityEngine.Object.Destroy(target);

        return result;
    }

    // works in-place on bg!
    private void Blend(Texture2D bg, Texture2D fg)
    {
        var bgPixels = bg.GetPixels();
        var fgPixels = fg.GetPixels();

        var pixels = new Color[bgPixels.Length];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Blend(bgPixels[i], fgPixels[i]);
        }

        bg.SetPixels(pixels);
        bg.Apply(false, false);
    }

    // just alpha blending, nothing to see here
    private Color Blend(Color bg, Color fg)
    {
        var inverseA = 1 - fg.a;
        var outputA = bg.a + fg.a * inverseA;

        if (outputA < 0.01)
        {
            return new(0, 0, 0, 0);
        }

        var r = (fg.r * fg.a + bg.r * bg.a * inverseA) / outputA;
        var g = (fg.g * fg.a + bg.g * bg.a * inverseA) / outputA;
        var b = (fg.b * fg.a + bg.b * bg.a * inverseA) / outputA;

        return new(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), Mathf.Clamp01(outputA));
    }
}
