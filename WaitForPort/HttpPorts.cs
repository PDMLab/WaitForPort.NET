namespace WaitForPort;

public static partial class Ports
{
  public static async Task WaitForHttp(Uri url, HttpMethod method, int timeoutMilliSeconds = int.MaxValue)
  {
    var millisecondsToWait = (DateTimeOffset.Now.Ticks / TimeSpan.TicksPerMillisecond) + timeoutMilliSeconds;
    var client = new HttpClient();
    while (true)
    {
      try
      {
        var response = await client.SendAsync(new HttpRequestMessage(method, url));
        if (!response.IsSuccessStatusCode)
        {
          throw new ApplicationException();
        }

        break;
      }
      catch (Exception ex)
      {
        Thread.Sleep(1000);
        var now = DateTimeOffset.Now.Ticks / TimeSpan.TicksPerMillisecond;

        if (now >= millisecondsToWait)
          throw new ApplicationException(
            $"Timeout waiting for url {url}",
            ex
          );
      }
    }
  }
}
