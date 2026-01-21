using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

// using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

class TestClass
{
    private List<System.Net.IPNetwork> _networks = new();
    public ForwardedHeaders? Headers { get; } = null;

    void X()
    {
        
    }
}