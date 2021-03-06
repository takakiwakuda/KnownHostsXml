using System.Xml.Serialization;

namespace KnownHostsXml;

/// <summary>
/// Represents a record in known_hosts.
/// </summary>
[XmlRoot("host")]
public sealed record KnownHost
{
    /// <summary>
    /// Gets or sets the host name of the SSH host.
    /// </summary>
    [XmlAttribute("name")]
    public string? HostName { get; init; }

    /// <summary>
    /// Gets or sets the fingerprint of the SSH host key.
    /// </summary>
    [XmlAttribute("fingerprint")]
    public string? Fingerprint { get; init; }
}
