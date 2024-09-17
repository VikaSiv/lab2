﻿using System;
using System.Collections.Generic;

public class Servers
{
    private static readonly Lazy<Servers> _instance = new Lazy<Servers>(() => new Servers());

    private HashSet<string> _servers;

    private readonly object _lock = new object();

    private Servers()
    {
        _servers = new HashSet<string>();
    }

    public static Servers Instance => _instance.Value;

    public bool AddServer(string serverAddress)
    {
        if (string.IsNullOrWhiteSpace(serverAddress))
        {
            return false;
        }

        lock (_lock)
        {
            if ((serverAddress.StartsWith("http://") || serverAddress.StartsWith("https://")) &&
                _servers.Add(serverAddress))
            {
                return true;
            }
        }

        return false;
    }

    public List<string> GetHttpServers()
    {
        List<string> httpServers = new List<string>();

        lock (_lock)
        {
            foreach (var server in _servers)
            {
                if (server.StartsWith("http://"))
                {
                    httpServers.Add(server);
                }
            }
        }

        return httpServers;
    }

    public List<string> GetHttpsServers()
    {
        List<string> httpsServers = new List<string>();

        lock (_lock)
        {
            foreach (var server in _servers)
            {
                if (server.StartsWith("https://"))
                {
                    httpsServers.Add(server);
                }
            }
        }

        return httpsServers;
    }
}

class Program
{
    static void Main()
    {
        var servers = Servers.Instance;

        Console.WriteLine(servers.AddServer("http://example.com"));
        Console.WriteLine(servers.AddServer("https://example.com"));
        Console.WriteLine(servers.AddServer("ftp://example.com"));
        Console.WriteLine(servers.AddServer("http://example.com"));
        Console.WriteLine(servers.AddServer("http://example1.com"));
        Console.WriteLine(servers.AddServer("https://example1.com"));

        var httpServers = servers.GetHttpServers();
        var httpsServers = servers.GetHttpsServers();

        Console.WriteLine("HTTP Servers:");
        foreach (var server in httpServers)
        {
            Console.WriteLine(server);
        }

        Console.WriteLine("HTTPS Servers:");
        foreach (var server in httpsServers)
        {
            Console.WriteLine(server);
        }
    }
}
