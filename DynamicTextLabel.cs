using System.Numerics;
using SampSharp.Entities.SAMP;
using SampSharp.Streamer.Entities.Interop;

namespace SampSharp.Streamer.Entities;

/// <summary>Streamer-managed 3D text label (PAWN-native: CreateDynamic3DTextLabel).</summary>
public sealed class DynamicTextLabel : DynamicEntity
{
    private string _text;
    private Color _color;

    internal DynamicTextLabel(int id, string text, Color color, Vector3 position, float drawDistance)
    {
        Id = id;
        _text = text;
        _color = color;
        Position = position;
        DrawDistance = drawDistance;
    }

    public Vector3 Position { get; }
    public float DrawDistance { get; }

    public string Text
    {
        get => _text;
        set
        {
            _text = value ?? string.Empty;
            StreamerInterop.Streamer_TextLabel_Update(Id, ToArgb(_color), _text);
        }
    }

    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            StreamerInterop.Streamer_TextLabel_Update(Id, ToArgb(_color), _text);
        }
    }

    /// <summary>Обновляет цвет и текст одним вызовом (один round-trip в streamer).</summary>
    public bool Update(Color color, string text)
    {
        _color = color;
        _text = text ?? string.Empty;
        return StreamerInterop.Streamer_TextLabel_Update(Id, ToArgb(color), _text);
    }

    public override bool IsAlive => StreamerInterop.Streamer_TextLabel_IsValid(Id);
    public override void DestroyEntity() => StreamerInterop.Streamer_TextLabel_Destroy(Id);

    private static uint ToArgb(Color c) => ((uint)c.A << 24) | ((uint)c.R << 16) | ((uint)c.G << 8) | c.B;
}
