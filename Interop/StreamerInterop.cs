using System.Runtime.InteropServices;

namespace SampSharp.Streamer.Entities.Interop;

/// <summary>
/// P/Invoke-биндинги к C-exports в SampSharp.dll, которые через
/// <c>IStreamerComponent</c> IExtension форвардят вызовы в streamer.dll
/// (форк Incognito streamer plugin с open.mp x64 портом).
///
/// Если streamer.dll не загружен, все Streamer_* возвращают 0/false.
/// Состояние доступности запрашивается через <see cref="Streamer_IsAvailable"/>.
/// </summary>
internal static partial class StreamerInterop
{
    // Our own open.mp component — SampSharp.Streamer.dll — is loaded by the server from
    // env/components/. LoadLibrary finds already-loaded modules by short name without extension.
    private const string Lib = "SampSharp.Streamer";

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_IsAvailable();

    // ----- Objects ------------------------------------------------------------------

    [LibraryImport(Lib)]
    internal static partial int Streamer_Object_Create(int modelId,
        float posX, float posY, float posZ,
        float rotX, float rotY, float rotZ,
        int worldId, int interiorId, int playerId,
        float streamDistance, float drawDistance,
        int areaId, int priority);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_Destroy(int objectId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_IsValid(int objectId);

    [LibraryImport(Lib)]
    internal static partial int Streamer_Object_Move(int objectId,
        float targetX, float targetY, float targetZ, float speed,
        float rotX, float rotY, float rotZ);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_Stop(int objectId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_IsMoving(int objectId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static unsafe partial bool Streamer_Object_GetPos(int objectId, float* x, float* y, float* z);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_SetPos(int objectId, float x, float y, float z);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static unsafe partial bool Streamer_Object_GetRot(int objectId, float* x, float* y, float* z);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_SetRot(int objectId, float x, float y, float z);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_AttachToObject(int objectId, int parentId,
        float offX, float offY, float offZ, float rotX, float rotY, float rotZ,
        [MarshalAs(UnmanagedType.I1)] bool syncRotation);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_AttachToPlayer(int objectId, int playerId,
        float offX, float offY, float offZ, float rotX, float rotY, float rotZ);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_AttachToVehicle(int objectId, int vehicleId,
        float offX, float offY, float offZ, float rotX, float rotY, float rotZ);

    [LibraryImport(Lib, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_SetMaterial(int objectId, int materialIndex,
        int modelId, string txdName, string textureName, uint materialColor);

    [LibraryImport(Lib, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_SetMaterialText(int objectId, int materialIndex,
        string text, int materialSize, string fontFace, int fontSize,
        [MarshalAs(UnmanagedType.I1)] bool bold, uint fontColor, uint backColor, int alignment);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Object_Edit(int playerId, int objectId);

    // ----- Pickups ------------------------------------------------------------------

    [LibraryImport(Lib)]
    internal static partial int Streamer_Pickup_Create(int modelId, int type,
        float posX, float posY, float posZ,
        int worldId, int interiorId, int playerId,
        float streamDistance, int areaId, int priority);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Pickup_Destroy(int pickupId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Pickup_IsValid(int pickupId);

    // ----- 3D Text Labels -----------------------------------------------------------

    [LibraryImport(Lib, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int Streamer_TextLabel_Create(string text, uint color,
        float posX, float posY, float posZ, float drawDistance,
        int attachedPlayer, int attachedVehicle,
        [MarshalAs(UnmanagedType.I1)] bool testLos,
        int worldId, int interiorId, int playerId,
        float streamDistance, int areaId, int priority);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_TextLabel_Destroy(int labelId);

    [LibraryImport(Lib, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_TextLabel_Update(int labelId, uint color, string text);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_TextLabel_IsValid(int labelId);

    // ----- Map Icons ----------------------------------------------------------------

    [LibraryImport(Lib)]
    internal static partial int Streamer_MapIcon_Create(float posX, float posY, float posZ,
        int type, uint color,
        int worldId, int interiorId, int playerId,
        float streamDistance, int style, int areaId, int priority);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_MapIcon_Destroy(int iconId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_MapIcon_IsValid(int iconId);

    // ----- Checkpoints --------------------------------------------------------------

    [LibraryImport(Lib)]
    internal static partial int Streamer_Checkpoint_Create(float posX, float posY, float posZ, float size,
        int worldId, int interiorId, int playerId,
        float streamDistance, int areaId, int priority);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Checkpoint_Destroy(int cpId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Checkpoint_IsValid(int cpId);

    // ----- Actors -------------------------------------------------------------------

    [LibraryImport(Lib)]
    internal static partial int Streamer_Actor_Create(int modelId,
        float posX, float posY, float posZ, float rotation,
        [MarshalAs(UnmanagedType.I1)] bool invulnerable, float health, float streamDistance,
        int worldId, int interiorId, int playerId, int areaId, int priority);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_Destroy(int actorId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_IsValid(int actorId);

    [LibraryImport(Lib, StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_ApplyAnimation(int actorId, string animLib, string animName,
        float delta,
        [MarshalAs(UnmanagedType.I1)] bool loop,
        [MarshalAs(UnmanagedType.I1)] bool lockX,
        [MarshalAs(UnmanagedType.I1)] bool lockY,
        [MarshalAs(UnmanagedType.I1)] bool freeze, int timeMs);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_ClearAnimations(int actorId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static unsafe partial bool Streamer_Actor_GetPos(int actorId, float* x, float* y, float* z);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_SetPos(int actorId, float x, float y, float z);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static unsafe partial bool Streamer_Actor_GetFacingAngle(int actorId, float* angle);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_SetFacingAngle(int actorId, float angle);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static unsafe partial bool Streamer_Actor_GetHealth(int actorId, float* health);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_SetHealth(int actorId, float health);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_IsInvulnerable(int actorId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_SetInvulnerable(int actorId,
        [MarshalAs(UnmanagedType.I1)] bool invulnerable);

    [LibraryImport(Lib)]
    internal static partial int Streamer_Actor_GetVirtualWorld(int actorId);

    [LibraryImport(Lib)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool Streamer_Actor_SetVirtualWorld(int actorId, int worldId);

    // ----- Event callback registration ---------------------------------------------

    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_PickUpDynamicPickup(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_EnterDynamicCP(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_LeaveDynamicCP(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_EnterDynamicRaceCP(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_LeaveDynamicRaceCP(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_EnterDynamicArea(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_LeaveDynamicArea(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicObjectMoved(delegate* unmanaged[Cdecl]<int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicObjectStreamIn(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicObjectStreamOut(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicPickupStreamIn(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicPickupStreamOut(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicTextLabelStreamIn(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicTextLabelStreamOut(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicCheckpointStreamIn(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicCheckpointStreamOut(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicMapIconStreamIn(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_DynamicMapIconStreamOut(delegate* unmanaged[Cdecl]<int, int, void> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_PlayerEditDynamicObject(delegate* unmanaged[Cdecl]<int, int, int, float, float, float, float, float, float, int> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_PlayerSelectDynamicObject(delegate* unmanaged[Cdecl]<int, int, int, float, float, float, int> fn);
    [LibraryImport(Lib)] internal static unsafe partial void Streamer_SetCallback_PlayerShootDynamicObject(delegate* unmanaged[Cdecl]<int, int, int, float, float, float, int> fn);
}
