using System.Collections;

namespace World.PathFinder
{
    public class PathfinderUtil
    {
        public static ArrayList smoothPathToStraight(ArrayList path,WorldZoneMapData mapData) {
            ArrayList newPath = new ArrayList();
            int lastPointIndex = 0;
            newPath.Add(path[lastPointIndex]);
            int nowCheckPointIndex = 1;
            for (; nowCheckPointIndex < path.Count; nowCheckPointIndex++) {
                int[] lastPoint = (int[])path[lastPointIndex];
                int[] nowCheckPoint = (int[])path[nowCheckPointIndex];
                if (mapData.CouldStraightGoByQr(lastPoint, nowCheckPoint)) {
                    continue;
                }
                newPath.Add(path[nowCheckPointIndex - 1]);
                lastPointIndex = nowCheckPointIndex - 1;
            }
            newPath.Add(path[nowCheckPointIndex - 1]);
            return newPath;
        }
    }
}
