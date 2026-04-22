using System.Numerics;
using SampSharp.Entities.SAMP;
using SampSharp.Streamer.Entities.Interop;

namespace SampSharp.Streamer.Entities;

/// <summary>Streamer-managed world object (PAWN-native: CreateDynamicObject).</summary>
public sealed class DynamicObject : DynamicEntity
{
    internal DynamicObject(int id, int modelId, Vector3 position, Vector3 rotation)
    {
        Id = id;
        ModelId = modelId;
        _position = position;
        _rotation = rotation;
    }

    public int ModelId { get; }

    private Vector3 _position;
    private Vector3 _rotation;

    public Vector3 Position
    {
        get
        {
            unsafe
            {
                float x = 0, y = 0, z = 0;
                if (StreamerInterop.Streamer_Object_GetPos(Id, &x, &y, &z))
                    _position = new Vector3(x, y, z);
            }
            return _position;
        }
        set
        {
            _position = value;
            StreamerInterop.Streamer_Object_SetPos(Id, value.X, value.Y, value.Z);
        }
    }

    public Vector3 Rotation
    {
        get
        {
            unsafe
            {
                float x = 0, y = 0, z = 0;
                if (StreamerInterop.Streamer_Object_GetRot(Id, &x, &y, &z))
                    _rotation = new Vector3(x, y, z);
            }
            return _rotation;
        }
        set
        {
            _rotation = value;
            StreamerInterop.Streamer_Object_SetRot(Id, value.X, value.Y, value.Z);
        }
    }

    public override bool IsAlive => StreamerInterop.Streamer_Object_IsValid(Id);

    public bool IsMoving => StreamerInterop.Streamer_Object_IsMoving(Id);

    public override void DestroyEntity() => StreamerInterop.Streamer_Object_Destroy(Id);

    /// <summary>
    /// Starts moving the object towards the given target. Returns the move
    /// duration in milliseconds, or 0 if the object is attached or speed is 0.
    /// </summary>
    public int Move(Vector3 target, float speed, Vector3 rotation = default)
        => StreamerInterop.Streamer_Object_Move(Id, target.X, target.Y, target.Z, speed,
            rotation.X, rotation.Y, rotation.Z);

    public bool Stop() => StreamerInterop.Streamer_Object_Stop(Id);

    /// <summary>Opens the per-player edit mode for this dynamic object on the given player.</summary>
    public bool Edit(Player player)
        => player != null && StreamerInterop.Streamer_Object_Edit(player.Id, Id);

    public bool AttachTo(DynamicObject parent, Vector3 offset, Vector3 rotation, bool syncRotation = true)
        => StreamerInterop.Streamer_Object_AttachToObject(Id, parent?.Id ?? -1,
            offset.X, offset.Y, offset.Z, rotation.X, rotation.Y, rotation.Z, syncRotation);

    public bool AttachTo(Player player, Vector3 offset, Vector3 rotation)
        => player != null
           && StreamerInterop.Streamer_Object_AttachToPlayer(Id, player.Id,
               offset.X, offset.Y, offset.Z, rotation.X, rotation.Y, rotation.Z);

    public bool AttachTo(Vehicle vehicle, Vector3 offset, Vector3 rotation)
        => vehicle != null
           && StreamerInterop.Streamer_Object_AttachToVehicle(Id, vehicle.Id,
               offset.X, offset.Y, offset.Z, rotation.X, rotation.Y, rotation.Z);

    public bool SetMaterial(int materialIndex, int modelId, string txdName, string textureName, uint materialColor = 0)
        => StreamerInterop.Streamer_Object_SetMaterial(Id, materialIndex, modelId,
            txdName ?? string.Empty, textureName ?? string.Empty, materialColor);

    public bool SetMaterial(int materialIndex, int modelId, string txdName, string textureName, Color materialColor)
        => SetMaterial(materialIndex, modelId, txdName, textureName, ToRgba(materialColor));

    public unsafe bool SetMaterialText(int materialIndex, string text,
        int materialSize = 0, string fontFace = "", int fontSize = 24, bool bold = true,
        uint fontColor = 0xFFFFFFFFu, uint backColor = 0, int alignment = 1)
    {
        var enc = SampSharp.OpenMp.Core.Std.StringViewMarshaller.Encoding;
        var s = text ?? string.Empty;
        var byteCount = enc.GetByteCount(s);
        var buffer = new byte[byteCount + 1];
        enc.GetBytes(s, buffer);
        buffer[byteCount] = 0;
        fixed (byte* ptr = buffer)
        {
            return StreamerInterop.Streamer_Object_SetMaterialText(Id, materialIndex,
                ptr, materialSize, fontFace ?? string.Empty, fontSize, bold,
                fontColor, backColor, alignment);
        }
    }

    public bool SetMaterialText(int materialIndex, string text,
        int materialSize, string fontFace, int fontSize, bool bold,
        Color fontColor, Color backColor, int alignment = 1)
        => SetMaterialText(materialIndex, text, materialSize, fontFace, fontSize, bold,
            ToRgba(fontColor), ToRgba(backColor), alignment);

    private static uint ToRgba(Color c) => ((uint)c.R << 24) | ((uint)c.G << 16) | ((uint)c.B << 8) | c.A;
}
