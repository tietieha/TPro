// ******************************************************************
//                .-"""-.
//               / .===. \
//               \/ 6 6 \/
//               ( \___/ )
//     ______ooo__\_____/_____________
//    / @author     Leon             /
//   / @Modified   2023-11-11 14:15 /
//  /_____________________ooo______/
//                |_ | _|
//                /-'Y'-\
//               (__/ \__)
// ******************************************************************

using UnityEngine;

#if UNITY_EDITOR
namespace GEngine.MapEditor
{
    public static class MapEditorEvent
    {
        public static MapEditorEvents.Event TestEvent = new("TestEvent");

        public static MapEditorEvents.Event<MapToolOP, MapToolOP> MapEditorToolChanged = new("MapEditorToolChanged");

        public static MapEditorEvents.Event<int> LevelLabelUpdateEvent      = new("LevelLabelUpdateEvent");
        public static MapEditorEvents.Event      LevelLabelUpdateAllEvent   = new("LevelLabelUpdateAllEvent");
        public static MapEditorEvents.Event<int> BusinessNameUpdateEvent    = new("BusinessNameUpdateEvent");
        public static MapEditorEvents.Event      BusinessNameUpdateAllEvent = new("BusinessNameUpdateAllEvent");
        public static MapEditorEvents.Event<int> InteractionRemoveEvent     = new("InteractionRemoveEvent");
        public static MapEditorEvents.Event<int> InteractionAddEvent        = new("InteractionAddEvent");
        public static MapEditorEvents.Event<InteractionUpdateParam> InteractionUpdateEvent     = new("InteractionUpdateEvent");
        public static MapEditorEvents.Event<InteractionPoint> InteractionEditorUpdateEvent = new("InteractionEditorUpdateEvent");
        public static MapEditorEvents.Event<int> InteractionFocusEvent      = new("InteractionFocusEvent");
        public static MapEditorEvents.Event      InteractionReloadEvent     = new("InteractionReloadEvent");
        public static MapEditorEvents.Event      InteractionSyncEvent       = new("InteractionSyncEvent");
        public class InteractionUpdateParam
        {
            public int Index;
            public Vector3 StartPosition;
        }
    }
}

#endif