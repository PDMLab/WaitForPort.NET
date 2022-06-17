using System;
using System.Net.Http;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;
using static WaitForPort.Ports;

namespace WaitForPortTests;

public class HttpTests
{
  [Fact]
  public async Task ShouldThrowOnTimeout()
  {
    var exception = await Assert.ThrowsAsync<ApplicationException>(
      () => WaitForHttp(new Uri("http://localhost"), HttpMethod.Get, 5000)
    );

    Assert.Equal(
      "Timeout waiting for url http://localhost/",
      exception.Message
    );
  }

  [Fact]
  public async Task ShouldWaitForHttp()
  {
    var service = new TestcontainersBuilder<TestcontainersContainer>()
      .WithImage("nginx:1.22")
      .WithPortBinding(
        80,
        80
      )
      .Build();

    await service.StartAsync();
    await WaitForHttp(new Uri("http://localhost"), HttpMethod.Get, 5000);
    await service.StopAsync();
  }
}
