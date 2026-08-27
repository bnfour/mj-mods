using System.IO;
using MelonLoader;
using UnityEngine;

namespace Bnfour.MoeJigsawMods.CustomBackground.Utilities;

/// <summary>
/// Manages the custom images used within the mod.
/// </summary>
/// <remarks>
/// Only ready to use Sprites are available outside this class. It encapsulates
/// all the technical stuff.
/// </remarks>
internal class SpriteProvider
{
    // this is of an utmost importance, or custom images would be rendered at 1% the size
    // took me way too long to find out
    private const float PixelsPerUnit = 1f;

    private const string FallbackBackgroundName = "Bnfour.MoeJigsawMods.CustomBackground.Resources.bg-placeholder.png";
    private readonly string CustomImagePath = Path.Combine(Application.dataPath, "bg.png");

    private readonly byte[] _rawBg;

    // TODO create at runtime, probably store as System.Drawing's images internally
    private readonly byte[] _rawBn;
    private readonly byte[] _rawBp;

    // backing fields for lazy loading

    private Sprite _background;
    private Sprite _buttonNormal;
    private Sprite _buttonPressed;

    internal Sprite Background => _background ??= LoadSprite(_rawBg, false);
    internal Sprite ButtonNormal => _buttonNormal ??= LoadSprite(_rawBn);
    internal Sprite ButtonPressed => _buttonPressed ??= LoadSprite(_rawBp);

    internal SpriteProvider()
    {
        EnsureImageExists();
        _rawBg = File.ReadAllBytes(CustomImagePath);

        // TODO instead of loading at runtime, create from loaded image
        // code is temporary, so just copypasted from another method xdd
        using (var stream = GetType().Assembly.GetManifestResourceStream("Bnfour.MoeJigsawMods.CustomBackground.Resources.normal.png"))
        using (MemoryStream ms = new())
        {
            // imagine using .NET Framework 3.5 in current year
            // 64k is a completely arbitrary buffer size
            var buffer = new byte[64 * 1024];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            _rawBn = ms.ToArray();
        }
        using (var stream = GetType().Assembly.GetManifestResourceStream("Bnfour.MoeJigsawMods.CustomBackground.Resources.pressed.png"))
        using (MemoryStream ms = new())
        {
            // imagine using .NET Framework 3.5 in current year
            // 64k is a completely arbitrary buffer size
            var buffer = new byte[64 * 1024];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            _rawBp = ms.ToArray();
        }
    }

    private void EnsureImageExists()
    {
        if (!File.Exists(CustomImagePath))
        {
            // TODO the logger is not available at the time this is called,
            // store an error flag or something
            // Melon<CustomBackgroundMod>.Logger.Warning($"Custom image not found, writing default fallback to {CustomImagePath}\nReplace it with your own image.");

            using (var stream = GetType().Assembly.GetManifestResourceStream(FallbackBackgroundName))
            using (MemoryStream ms = new())
            {
                // imagine using .NET Framework 3.5 in current year
                // 64k is a completely arbitrary buffer size
                var buffer = new byte[64 * 1024];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                File.WriteAllBytes(CustomImagePath, ms.ToArray());
            }
        }
    }

    private Sprite LoadSprite(byte[] data, bool isPivotCentered = true)
    {
        var texture = new Texture2D(1, 1, TextureFormat.RGB24, false);
        ImageConversion.LoadImage(texture, data);

        // TODO this warns on loading button images, but those will not be loaded this way later
        if (!(texture.width == 1920 && texture.height == 1080))
        {
            Melon<CustomBackgroundMod>.Logger.Warning("1920x1080 images work best for backgrounds.");
        }

        Vector2 pivot = isPivotCentered ? new(0.5f, 0.5f) : new(0, 0);
        return Sprite.Create(texture, new(0, 0, texture.width, texture.height), pivot, PixelsPerUnit);
    }
}
