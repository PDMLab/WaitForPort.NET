using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;
using static WaitForPort.Ports;

namespace WaitForPortTests;

public class TcpPortTests
{
  [Fact]
  public async Task ShouldWaitForTcpPort()
  {
    var service = new TestcontainersBuilder<TestcontainersContainer>()
      .WithImage("postgres:14")
      .WithEnvironment(
        "POSTGRES_PASSWORD",
        "p455w02d"
      )
      .WithPortBinding(
        5432,
        5432
      )
      .Build();

    await service.StartAsync();
    WaitForTcpPort(
      5432,
      10000
    );
    await service.StopAsync();
  }

  [Fact]
  public void ShouldThrowOnUnavailableTcpPort()
  {
    var exception = Assert.Throws<ApplicationException>(
      () => WaitForTcpPort(
        5432,
        10000
      )
    );

    Assert.Equal(
      "Timeout waiting for endpoint 127.0.0.1:5432",
      exception.Message
    );
  }
}
