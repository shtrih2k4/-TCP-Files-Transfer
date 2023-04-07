using System.Net;
using System.Net.Sockets;

using System.Text;

string[] paths=new string[1] { @"C:\Users\zevas\Desktop\1.jpeg"};
using TcpClient client = new();
await client.ConnectAsync("127.0.0.1", 8888);
NetworkStream stream = client.GetStream();

using StreamReader reader=new StreamReader(stream);
using StreamWriter writer=new StreamWriter(stream);

foreach(string path in paths)
{

    await writer.WriteLineAsync(path);
    await writer.FlushAsync();
    string? result = await reader.ReadLineAsync();
    Console.WriteLine(result);
}
await writer.WriteLineAsync("END");
await writer.FlushAsync();
