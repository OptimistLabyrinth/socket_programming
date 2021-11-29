using System.Drawing;
using System.Net.Sockets;

// // See https://aka.ms/new-console-template for more information
// Console.WriteLine("Client Program");

public class Client
{
  public static int Main(string[] args)
  {
    Client myClient = new Client();
    myClient.Run();
    return 0;
  }

  private const string terminate_string = "--term-end--";
  private const int BUFF_SIZE = 4096;

  public Client()
  {
  }

  public void Run()
  {
    string host = "127.0.0.1";
    int port = 8080;

    try
    {
      using (TcpClient tcpClient = new TcpClient(host, port))
      {
        using (NetworkStream stream = tcpClient.GetStream())
        {
          FileInfo fileInfo = new FileInfo("./4k_image.jpg");
          byte[] imageData = new byte[fileInfo.Length];

          Console.WriteLine(fileInfo.Length);

          using (FileStream fstream = fileInfo.OpenRead())
          {
            fstream.Read(imageData, 0, imageData.Length);
          }

          byte[] lengthData = BitConverter.GetBytes(imageData.Length);

          Console.WriteLine(lengthData.Length);

          stream.Write(lengthData, 0, lengthData.Length);
          stream.Write(imageData, 0, imageData.Length);
        }
      }
    }
    catch (System.Exception ex)
    {
      Console.WriteLine(ex);
    }
  }
}
