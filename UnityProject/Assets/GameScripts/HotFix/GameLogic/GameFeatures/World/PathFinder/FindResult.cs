namespace World.PathFinder
{
    public enum FindResult : int
    {
        DO_NOT_FIND = -1,
        SUCCESS = 1,
        TIME_OUT = 2,
        NODE_TOO_LONG = 3,
        POS_ERROR = 4,
        BLOCKING = 5,
    }
}
