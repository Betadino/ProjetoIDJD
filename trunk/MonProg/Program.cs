using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Protocol;

class MyTcpListener
{  

    private static void Main()
    {
        

        TcpListener server = null;
        
        try
        {
            // Set the TcpListener on port 13130.
            Int32 port = 13130;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            /* 1 byte assinatura = X;
            1 byte para bit masking:
              1a verificação = reply?*/


            //thread counter - using in names.
            //int threadNumber;         
            
            

            // Enter the listening loop.
            while (true)
            {
                Console.Write("Waiting for a connection... ");
                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.

                /*isto é feito na criação da thread??*/
                TcpClient client = server.AcceptTcpClient();

                //Create a new thread each time a new socket is open
                Thread t = new Thread(HandleThread);
                t.Start(client);

                Console.WriteLine("Connected!");
                Console.WriteLine("New thread Created!");
                //threadNumber++;
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            server.Stop();
        }

        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }

    static void HandleThread(object o)
    {
        var session = new Session((TcpClient) o);
        session.handleMessages();
    }
}
