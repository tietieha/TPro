using UnityEngine;

namespace GEngine.MapEditor
{
    public class AreaScatterSetting
    {
        public bool isCenterZone;
        public int expandCount;

        public int width;
        public int height;
        public int level;
        public int count;
        public int postype;
        public string name;

        public Rect rect;
    }

    public class MovetypeScatterSetting
    {
        public string name;
        public int count;
        public int minStep;
        public int maxStep;
        public int affectRound;
    }
}