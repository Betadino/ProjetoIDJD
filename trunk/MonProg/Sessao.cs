using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Protocol
{
    public class Session
    {
        NetworkStream stream;
        TcpClient client;

        public Session(TcpClient _client)
        {
            client = _client;
            stream = client.GetStream();
        }

        public void handleMessages()
        {
            // Get a stream object for reading and writing
            
            byte[] header= new byte[8];
            byte[] payload = null;

            
            while(true)
            {
             //byte[] doisDispose= new byte[8];
        
            
            //string dados = "hey";
            //data = null;
            
            //int numBytesToRead = (int)stream.Length; - perguntar ao professor

            int numBytesToRead = header.Length;

            int numBytesRead = 0;
            // Loop to receive all the data sent by the client.
            while (numBytesToRead>0)
            {   
               
                int readThisCycle = stream.Read(header, numBytesRead, numBytesToRead);
                
                // Break when the end of the file is reached.
                if (0 == readThisCycle)
                break;

                numBytesRead += readThisCycle;
                numBytesToRead -= readThisCycle;


            }
                //Console.WriteLine("teste:" + header[0]);
                    //if(header[0] != 'X')
                      //  {   
                          //  Console.WriteLine("X");
                        //}
                

                numBytesToRead = header.Length;

                numBytesRead = 0;
                // Translate data bytes to a ASCII string.
                
                string data = System.Text.Encoding.ASCII.GetString(header, numBytesRead,numBytesToRead);

                Console.WriteLine("Received: {0}", data);
               // String maiusculas, converte string para bytes.
                string msg = "Received.";
                //data = data.ToUpper();
                msg = msg.ToUpper();
                byte[] bmsg = System.Text.Encoding.ASCII.GetBytes(msg);

                // Send back a response.
                stream.Write(bmsg, 0, bmsg.Length);
                Console.WriteLine("Sent: {0}", msg);
            }

        }
    }
}