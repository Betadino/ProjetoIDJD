using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class MyTcpListener
{
  public static void Main()
  {
    TcpListener server = null;
    try
    {
      // Set the TcpListener on port 13130.
      Int32 port = 13130;
      IPAddress localAddr = IPAddress.Parse("192.168.215.28");

      // TcpListener server = new TcpListener(port);
      server = new TcpListener(localAddr, port);

      // Start listening for client requests.
      server.Start();

      // Buffer for reading data
      Byte[] bytes = new Byte[256];
      String data = null;
      //thread counter - using in names.
      int threadNumber = 0;

      // Enter the listening loop.
      while(true)
      {
        Console.Write("Waiting for a connection... ");
        // Perform a blocking call to accept requests.
        // You could also use server.AcceptSocket() here.

        /*isto é feito na criação da thread??*/
        //var client  = server.AcceptTcpClient();


        //Create a new thread each time a new socket is open
        Thread t = new Thread(client = server.AcceptSocket);
        t.Start();
        data = null;

        Console.WriteLine("Connected!");
        Console.WriteLine("New thread Created!");
        threadNumber++;


        
        // Get a stream object for reading and writing
        NetworkStream stream = client.GetStream();

        int i;

        // Loop to receive all the data sent by the client.
        while((i = stream.Read(bytes, 0, bytes.Length))!=0)
        {
          // Translate data bytes to a ASCII string.
          data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
          Console.WriteLine("Received: {0}", data);

          // Process the data sent by the client.
          data = data.ToUpper();

          byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

          // Send back a response.
          stream.Write(msg, 0, msg.Length);
          Console.WriteLine("Sent: {0}", data);
        }
      }
    }
    catch(SocketException e)
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
}