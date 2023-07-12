using System.Net.Sockets;

namespace Protocol;

public class Message
{
    public byte[] body;

    public Session session = null;
    public Header header = null;


    //Construtor Inbound Messages
    public Message(Session _session,byte[] _header)
    {
        session = _session;
        header = new Header();
        header.RecvHeader(_header, _session);
        body = new byte[header.msgSize];
        Recv();
        
    }

    //Construtor Outbound Messages de resposta
    public Message(Message _message)
    {
            header = _message.header;
            session = _message.session;
            body = new byte[_message.body.Length];
            body = _message.body;
            header.msgSize = (ushort)body.Length;
            string cmd = System.Text.Encoding.ASCII.GetString(body);
            header.isResponse();
            Send();

           // header.msgSize =(ushort)  _message.message.Length;
           // header.cookHeader();
        
       
    }

    //Construtor Outbound Messages, para ser trabalhado com inputs de utilizador.
    public Message (Session _session)
    {   
        header = new Header();
        session = _session;
    }



    public void Recv()
    {
            body = session.Recv(body, (ushort) body.Length);
            string content = System.Text.Encoding.ASCII.GetString(body);
            session.ParseCommand(this);
    }

    public Message Reply(byte[] _body)
    {
       body = new byte[_body.Length];
       body = _body;
       return (new Message(this));
    }

    public byte[] cookHeader()
    {
        
            header.msgIdOrigin = header.msgID;
            header.msgID = session.GenNewID();
            var passHeader = header.cookHeader();
            return passHeader;
    }

    /*public byte[] cookExtHeader()
    {}*/
    public byte[] cookMsg(string msg)
    {  
        byte[] cookedMsg = System.Text.Encoding.ASCII.GetBytes(msg);
        return cookedMsg;
    }

    public void Send()
    {
        
        byte[] header =  cookHeader();
        session.Send(header, (ushort)header.Length);
        //se o extended header tiver ligado no bitmask manda tbm o ext header.
        if((header[1] & (1 << 1)) != 0)
        {
            byte[] extHeader = this.header.isExtended();
            session.Send(extHeader ,(ushort)extHeader.Length);
        }
        session.Send(body,(ushort) body.Length);
    }
}