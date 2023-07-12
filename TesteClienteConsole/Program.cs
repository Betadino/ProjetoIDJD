using System.Net.Sockets;
using Protocol;


class MyTcpClient
{
    public static void Main()
    {
        string msg = "";
      
            try
            {
                Int32 port = 13130;
                TcpClient client = new TcpClient("127.0.0.1", port);
                Session session = new Session(client);
                Console.WriteLine("Connected!!");

            while(true)
                {
                Console.WriteLine("Insert command:");
                msg = Console.ReadLine();
                Message messageSent = new Message(session);
                messageSent.body = messageSent.cookMsg(msg);
                messageSent.header.msgSize = (ushort)messageSent.body.Length;
                //Console.WriteLine(messageSent.header.msgSize);
                messageSent.Send();
                session.HandleMessages();

                }
            }
            catch (ArgumentNullException err)
            {
                Console.WriteLine( "ArgumentNullException: "+ err );
            }
            catch (SocketException err)
            {
                Console.WriteLine( "SocketException: " + err );
            }
            catch (Exception err)
            {
                Console.WriteLine( "Erro----> " + err );
            }
        
    }
} 