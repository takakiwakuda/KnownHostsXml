using System;
using System.IO;
using System.Security;
using System.Xml;
using System.Xml.Serialization;
using SpecialFolder = System.Environment.SpecialFolder;

namespace KnownHostsXml;

/// <summary>
/// Provides utilities to read and write XML-based known_hosts.
/// </summary>
public static class KnownHostsFile
{
    /// <summary>
    /// Gets the absolute path of the ssh directory for the current user.
    /// </summary>
    public static string DirectoryName
    {
        get
        {
            if (_directoryName is null)
            {
                _directoryName = Path.Combine(Environment.GetFolderPath(SpecialFolder.UserProfile), ".ssh");
            }

            return _directoryName;
        }
    }

    /// <summary>
    /// Gets the file name of the known_hosts for the current user.
    /// </summary>
    public static string FullName
    {
        get
        {
            if (_fullName is null)
            {
                _fullName = Path.Combine(DirectoryName, "known_hosts.xml");
            }

            return _fullName;
        }
    }

    private static string? _directoryName;
    private static string? _fullName;

    /// <summary>
    /// Reads an array of the <see cref="KnownHost"/> objects at the <see cref="FullName"/>.
    /// </summary>
    /// <returns>An array of the <see cref="KnownHost"/> objects for the <see cref="FullName"/>.</returns>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="UnauthorizedAccessException"/>
    /// <exception cref="DirectoryNotFoundException"/>
    /// <exception cref="FileNotFoundException"/>
    /// <exception cref="IOException"/>
    /// <exception cref="SecurityException"/>
    public static KnownHost[] ReadRecords()
    {
        return ReadRecords(FullName);
    }

    /// <summary>
    /// Reads an array of the <see cref="KnownHost"/> objects from the specified file name.
    /// </summary>
    /// <param name="path">The file to be read.</param>
    /// <returns>An array of the <see cref="KnownHost"/> objects for the <paramref name="path"/>.</returns>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="UnauthorizedAccessException"/>
    /// <exception cref="DirectoryNotFoundException"/>
    /// <exception cref="FileNotFoundException"/>
    /// <exception cref="IOException"/>
    /// <exception cref="SecurityException"/>
    public static KnownHost[] ReadRecords(string path)
    {
        if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using XmlReader reader = XmlReader.Create(stream);
        XmlSerializer serializer = new(typeof(KnownHost[]));

        object? hosts = serializer.Deserialize(reader);
        if (hosts is null)
        {
            return Array.Empty<KnownHost>();
        }

        return (KnownHost[])hosts;
    }

    /// <summary>
    /// Writes the specified <see cref="KnownHost"/> objects to the <see cref="FullName"/>.
    /// </summary>
    /// <remarks>The <see cref="FullName"/> will be overwritten with the <paramref name="hosts"/>.</remarks>
    /// <param name="hosts">An array of the <see cref="KnownHost"/> objects to be written.</param>
    /// <param name="indent">
    /// <see langword="true"/> if individual records have new lines and indent; otherwise, <see langword="false"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="hosts"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="UnauthorizedAccessException"/>
    /// <exception cref="IOException"/>
    /// <exception cref="SecurityException"/>
    public static void WriteRecords(KnownHost[] hosts, bool indent = false)
    {
        if (hosts is null)
        {
            throw new ArgumentNullException(nameof(hosts));
        }

        DirectoryInfo directory = new(DirectoryName);
        if (!directory.Exists)
        {
            directory.Create();
        }

        using FileStream stream = new(FullName, FileMode.Create, FileAccess.Write, FileShare.Read);
        WriteRecords(stream, hosts, indent);
    }

    /// <summary>
    /// Writes the specified <see cref="KnownHost"/> objects to the specified file name.
    /// </summary>
    /// <remarks>The <paramref name="path"/> will be overwritten with the <paramref name="hosts"/>.</remarks>
    /// <param name="path">The file to write to.</param>
    /// <param name="hosts">An array of the <see cref="KnownHost"/> objects to be written.</param>
    /// <param name="indent">
    /// <see langword="true"/> if individual records have new lines and indent; otherwise, <see langword="false"/>.
    /// </param>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="path"/> or <paramref name="hosts"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="UnauthorizedAccessException"/>
    /// <exception cref="DirectoryNotFoundException"/>
    /// <exception cref="IOException"/>
    /// <exception cref="SecurityException"/>
    public static void WriteRecords(string path, KnownHost[] hosts, bool indent = false)
    {
        if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (hosts is null)
        {
            throw new ArgumentNullException(nameof(hosts));
        }

        using FileStream stream = new(path, FileMode.Create, FileAccess.Write, FileShare.Read);
        WriteRecords(stream, hosts, indent);
    }

    /// <summary>
    /// Writes the specified <see cref="KnownHost"/> objects to the stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="hosts">An array of the <see cref="KnownHost"/> objects to be written.</param>
    /// <param name="indent">
    /// <see langword="true"/> if individual records have new lines and indent; otherwise, <see langword="false"/>.
    /// </param>
    /// <exception cref="InvalidOperationException"/>
    private static void WriteRecords(Stream stream, KnownHost[] hosts, bool indent)
    {
        using XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = indent });

        XmlSerializer serializer = new(typeof(KnownHost[]));
        serializer.Serialize(writer, hosts);
    }
}
