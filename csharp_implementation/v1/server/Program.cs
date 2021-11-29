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
  private const int BUFF_SIZE = 10;
  public Server()
  {
  }

  public void Run()
  {
    IPAddress host = IPAddress.Parse("127.0.0.1");
    int port = 8080;

    TcpListener tcpListener = new TcpListener(host, port);
    tcpListener.Start();

    while (true)
    {
      try
      {
        using (TcpClient tcpClient = tcpListener.AcceptTcpClient())
        {
          string result = String.Empty;

          using (NetworkStream stream = tcpClient.GetStream())
          {
            while (true)
            {
              byte[] readData = new byte[BUFF_SIZE];

              int readBytes = stream.Read(readData, 0, readData.Length);
              string readString = System.Text.Encoding.UTF8.GetString(readData, 0, readBytes);

              Console.WriteLine($"    {readString}");

              if (readString.Contains(terminate_string))
              {
                readString = readString.Substring(0, readString.IndexOf(terminate_string));
                result += readString;
                break;
              }

              result += readString;
              if (result.Contains(terminate_string) || readString.Length < BUFF_SIZE)
              {
                break;
              }
            }
            result = result.Substring(0, result.IndexOf(terminate_string));

            Console.WriteLine($"from client: {result}");

            string sendString = $"received {result.Length} bytes from client" + terminate_string;
            byte[] sendData = System.Text.Encoding.UTF8.GetBytes(sendString);

            stream.Write(sendData, 0, sendData.Length);
          }
        }
      }
      catch (System.Exception ex)
      {
        Console.WriteLine(ex);
      }
    }
  }
}
