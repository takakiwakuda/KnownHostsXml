using System;
using System.Xml.Serialization;

namespace KnownHostsXml;

/// <summary>
/// Defines a class that contains records of the <see cref="KnownHost"/>.
/// </summary>
/// <remarks>This class is used by the <see cref="KnownHostsFile"/> class.</remarks>
[XmlRoot("root")]
public sealed class KnownHosts
{
    /// <summary>
    /// Gets an array of <see cref="KnownHost"/> objects.
    /// </summary>
    [XmlArray("hosts")]
    [XmlArrayItem("host")]
    public KnownHost[] Hosts
    {
        get
        {
            if (_hosts is null)
            {
                return Array.Empty<KnownHost>();
            }

            return _hosts;
        }
        set
        {
            _hosts = value;
        }
    }

    private KnownHost[]? _hosts;
}
