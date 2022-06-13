using System.Net;
using System.Net.Sockets;

namespace WaitForPort;

public class Ports
{
  public static async Task WaitForTcpPort(
    int port,
    int timeout = int.MaxValue
  )
  {
    if (port <= 0)
    {
      throw new ArgumentException($"{nameof(timeout)} must be greater than zero.");
    }
    
    if (timeout < 0)
    {
      throw new ArgumentException($"{nameof(timeout)} must be greater or equal to zero.");
    }
    WaitForPort(port, timeout);
  }

  private static void WaitForPort(
    int port,
    long timeoutMilliseconds
  )
  {
    var endpoint = new IPEndPoint(
      IPAddress.Parse("127.0.0.1"),
      port
    );
    using (var socket = new Socket(
             SocketType.Stream,
             ProtocolType.Tcp
           ))
    {
      var millisecondsToWait = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + timeoutMilliseconds;
      while (true)
        try
        {
          socket.Connect(endpoint);
          break;
        }
        catch (Exception ex)
        {
          Thread.Sleep(1000);
          var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

          if (now >= millisecondsToWait)
            throw new ApplicationException(
              $"Timeout waiting for endpoint {endpoint.Address}:{endpoint.Port}",
              ex
            );
        }
    }
  }
}