using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Protocol
{
    public class Session
    {
        ushort sessionMsgID;
        NetworkStream stream;
        TcpClient client;

        //string de teste
        string teste = "isto é um teste";

        private Byte[] headerArr { get; set; }

        public Session(TcpClient _client)
        {
            client = _client;
            stream = client.GetStream();
            headerArr = new Byte[8];
            
        }

        public void HandleMessages()
        {
            
            //recebe o header do stream
            Recv(headerArr, (ushort)headerArr.Length);
            
            if (headerArr[0] != 88) // verificar assinatura
            {
                client.Close();
            }
            else
            {
                Message msg = new Message(this, headerArr);

                msg.Recv();

            }

        }

        public ushort GenNewID()
        {
           return ++ sessionMsgID;
        }

        public byte[] Recv(byte[] buffer, ushort size)
        {
            //trata  o header.
            int numBytesToRead = size;
            int numBytesRead = 0;
            while (numBytesToRead>0)
            {   
            
                int readThisCycle = stream.Read(buffer, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (0 == readThisCycle)
                break;
                numBytesRead += readThisCycle;
                numBytesToRead -= readThisCycle;
            }
            /*if(buffer.Length>0)
            {

            string content = System.Text.Encoding.ASCII.GetString(buffer);
            Console.WriteLine("Debug: "+ content);
            }*/

            return buffer;
        }

         public void Send(byte [] block, ushort blockSize)
        {
            //chama o send do session
              //recebe o header do stream
                ushort numBytesWritten = 0;
                stream.Write(block, numBytesWritten, blockSize); 
        }


    }


}