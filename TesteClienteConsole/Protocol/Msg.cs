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
        Recv();
        
    }

    //Construtor Outbound Messages de resposta
    public Message(Message _message)
    {
           header = new Header();
           session = _message.session;
           body = _message.body;
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
            body = new byte[header.msgSize];
            body = session.Recv(body, header.msgSize);
            string content = System.Text.Encoding.ASCII.GetString(body);
            Console.WriteLine("Incoming message: "+ content);
    }

      public Message Reply(byte[] _body)
    {
       body = new byte[_body.Length];
       body = _body;
       return (new Message(this));
    }

    public byte[] cookHeader(Message _message)
    {

            this.header.msgIdOrigin = _message.header.msgID;
            this.header.msgID = _message.session.GenNewID();
            header.msgSize = _message.header.msgSize;
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
        byte[] header =  cookHeader(this);
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