using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GEngine.MapEditor
{
    public enum InteractionType
    {
        Chest = 1,
        Npc,
        Army,
        Enemy,
        Res,
        Lamb,
        Ore,
        Gold,
        Cristal,
        Other,
    }

    public static class InteractionTypeExtensions
    {
        public static InteractionType ToNumericValue(string name)
        {
            if (string.IsNullOrEmpty(name))
                return InteractionType.Npc;
            if (Enum.TryParse(name, out InteractionType result))
                return result;
            return InteractionType.Npc;
        }
    }

    public class InteractionPoint
    {
        /// <summary>
        /// Event Id
        /// </summary>
        public int id;
        /// <summary>
        /// Excel中配置的id
        /// </summary>
        public int index;
        public Vector2Int position;
        public Vector3 rotation;
        public InteractionType type = InteractionType.Npc;
        /// <summary>
        /// 注释信息
        /// </summary>
        public string comments = string.Empty;
        public GameObject prefab;
        public string prefabPath;
        public Vector3 scale = Vector3.one;
        public int mapId = 0;
        public List<Vector2Int> BlockPoints = new List<Vector2Int>();

        public bool IsBlockPosition(int x, int y)
        {
            foreach (var point in BlockPoints)
            {
                if (point.x == x && point.y == y)
                {
                    return true;
                }
            }

            return false;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(position.x);
            bw.Write(position.y);
            bw.Write(rotation.x);
            bw.Write(rotation.y);
            bw.Write(rotation.z);
            bw.Write((int)type);
            bw.Write(prefabPath);
            bw.Write(scale.x);
            bw.Write(scale.y);
            bw.Write(scale.z);
        }
    }
}