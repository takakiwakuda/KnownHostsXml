using System.IO;
using Xunit;

namespace KnownHostsXml.Tests;

public class TestKnownHosts
{

    [Fact]
    public void Test()
    {
        string path = Path.Combine(Path.GetTempPath(), "known_hosts.xml");

        KnownHost[] hosts = new KnownHost[] {
            new()
            {
                HostName = "nuko",
                Fingerprint = "ssh-ed25519 255 ...="
            },
            new()
            {
                HostName = "cat",
                Fingerprint = "ssh-rsa 4096 ...="
            }
        };

        KnownHostsXml.WriteRecords(path, hosts, true);
        Assert.Equal<KnownHost>(hosts, KnownHostsXml.ReadRecords(path));
    }
}
