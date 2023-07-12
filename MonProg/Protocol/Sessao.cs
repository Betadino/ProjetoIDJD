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
            // Linhas de debug
            //Console.WriteLine("HEX debug: ", Convert.ToHexString(buffer, 0, size));
            //string content = System.Text.Encoding.ASCII.GetString(buffer);
            //Console.WriteLine("Incoming message: "+ content);

            return buffer;
        }

        public void Send(byte [] block, ushort blockSize)
        {
            //chama o send do session
              //recebe o header do stream
                ushort numBytesWritten = 0;
                stream.Write(block, numBytesWritten, blockSize); 
        }

        public void ParseCommand(Message _msg)
        {
            string cmd = System.Text.Encoding.ASCII.GetString(_msg.body);
            cmd = cmd.ToUpper();
            string cpu="cpu: 80%";
            string ram= "ram: 69%";
            string notValid = cmd + " was not a valid command";
            //switch case respostas de comandos.
            switch(cmd) 
                {
                  case "CPU":
                    byte[] msgcpu = _msg.cookMsg(cpu);
                    _msg.Reply(msgcpu);
                    break;
                  case "RAM":
                    byte[] msgram = _msg.cookMsg(ram);
                    _msg.Reply(msgram);
                    break;
                  default:
                    byte[] msgvalid = _msg.cookMsg(notValid);
                    _msg.Reply(msgvalid);
                    break;
                }
        }

    }


}