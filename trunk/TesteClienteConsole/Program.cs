using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Protocol;

class MyTcpClient
{
    public static void Main()
    {
        NetworkStream stream=null;
        int extensions = 0;
        string msg;
        Int32 msgID;
        byte bmask;
        char sign;
        msg = "messageToSend";
        Int16 msgSize = (Int16)msg.Length;
        sign ='X';
        msgID = 1;
        bmask = 0b01000000;

        // Convert char to byte
        byte signByte = (byte)sign;


        // Combine all bytes into one byte array
        byte[] header =new byte[8+extensions];
        //assinatura
        header[0] = signByte;
        //bitmask
        header[1] = bmask;
        //tamanho da mensagem a mandar
        header[2] = (byte)(msgSize >> 8);
        header[3] = (byte)(msgSize & 255);
        //Id da mensagem
        header[4] = (byte) (msgID >> 24);
        header[5] = (byte)((msgID >> 16) & 0xFF);
        header[6] = (byte)((msgID >> 8) & 0xFF);
        header[7] = (byte) (msgID & 0xFF);

        //str=Console.ReadLine();
            try
            {
            while(true)
                {
                
                Console.WriteLine( "Sending \"" + msg + "\" to server..." );
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 13130;

                // Prefer a using declaration to ensure the instance is Disposed later.
                TcpClient client = new TcpClient("127.0.0.1", port);

                // |x|y| | | | | | |
                // &8

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

                Console.WriteLine(data.Length);

                //data[0] = Convert.ToByte('x');
                
                // Get a client stream for reading and writing.
                
                stream = client.GetStream();
                

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: "+ System.Text.Encoding.ASCII.GetString(data, 0, data.Length) + " (length): " + data.Length );

                // Receive the server response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                //Console.ReadKey();
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine( "Received: "+ responseData );

                //stream.Dispose();
                // Explicit close is not necessary since TcpClient.Dispose() will be
                // called automatically.
                //stream.Close();
                //client.Close();
                //client.Dispose();
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