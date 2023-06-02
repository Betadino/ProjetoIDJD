namespace Protocol;

public class Msg
{
    private Session session;
    private Header header;
    string msg;
    Int16 msgSize;

    public Header Header { get => header; set => header = value; }
    public Session Session { get => session; set => session = value; }

    public Msg(Session _session)
    {
        Session = _session;
        Header = new Header();

        
    }

    public Msg(Msg messge)
    {

    }

    public void Send()
    {      
        
        
    }

    public void Receive()
    {
        
    }




}