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
  private const int BUFF_SIZE = 10;

  public Client()
  {
  }

  public void Run()
  {
    string host = "127.0.0.1";
    int port = 8080;

    while (true)
    {
      try
      {
        Console.WriteLine("Enter message to send server: ");
        string? message = Console.ReadLine();
        if (message == null || message == "exit" || message == "quit")
        {
          break;
        }
        if (message.Length == 0)
        {
          continue;
        }
        message += terminate_string;

        using (TcpClient tcpClient = new TcpClient(host, port))
        {
          byte[] sendData = System.Text.Encoding.UTF8.GetBytes(message);

          using (NetworkStream stream = tcpClient.GetStream())
          {
            stream.Write(sendData, 0, sendData.Length);

            string result = String.Empty;

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

            Console.WriteLine($"Received from server: {result}");
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
