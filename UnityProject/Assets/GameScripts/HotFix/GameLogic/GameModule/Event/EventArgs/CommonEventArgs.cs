public class CommonEventArgs : GameEventArgs
{
    private int m_EventId;
    private object m_UserData;

    public CommonEventArgs(EventId eventId, object userData = null)
    {
        m_EventId = (int)eventId;
        m_UserData = userData;
    }
    
    public CommonEventArgs(int eventId, object userData = null)
    {
        m_EventId = eventId;
        m_UserData = userData;
    }

    public override int Id
    {
        get { return m_EventId; }
    }

    public object UserData
    {
        get { return m_UserData; }
        set { m_UserData = value; }
    }

    public object UserData1
    {
        get;
        set;
    }

    public override void Clear()
    {
        m_EventId = 0;
        m_UserData = null;
    }
}
