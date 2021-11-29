using System.Net;
using System.Net.Sockets;

// // See https://aka.ms/new-console-template for more information
// Console.WriteLine("Server Program");

public class Server
{
  public static int Main(string[] args)
  {
    Server myServer = new Server();
    myServer.Run();
    return 0;
  }

  private const string terminate_string = "--term-end--";
  private const int BUFF_SIZE = 4096;
  public Server()
  {
  }

  public void Run()
  {
    IPAddress host = IPAddress.Parse("127.0.0.1");
    int port = 8080;

    TcpListener tcpListener = new TcpListener(host, port);
    tcpListener.Start();

    try
    {
      using (TcpClient tcpClient = tcpListener.AcceptTcpClient())
      {
        using (NetworkStream stream = tcpClient.GetStream())
        {
          byte[] lengthData = new byte[sizeof(int)];
          stream.Read(lengthData, 0, lengthData.Length);

          byte[] imageData = new byte[BitConverter.ToInt32(lengthData, 0)];
          stream.Read(imageData, 0, imageData.Length);

          File.WriteAllBytes("./4k_image_copy.jpg", imageData);
        }
      }
    }
    catch (System.Exception ex)
    {
      Console.WriteLine(ex);
    }
  }
}
