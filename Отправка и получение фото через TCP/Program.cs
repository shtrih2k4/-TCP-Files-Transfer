using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

TcpListener server = new(IPAddress.Any, 8888);
string uploadpath = @$"{Directory.GetCurrentDirectory()}\uploads";
Directory.CreateDirectory(uploadpath);
try
{
    server.Start();
    Console.WriteLine("Server is loaded");
    while (true)
    {
        TcpClient client = await server.AcceptTcpClientAsync();
        await Task.Run(async () => await ProcessClient(client,uploadpath));
    }
}
finally
{
    server.Stop();
}
async Task ProcessClient(TcpClient client,string uploadpath)
{
    NetworkStream stream = client.GetStream();
    using StreamReader reader= new (stream);
    using StreamWriter writer = new(stream);
    while (true)
    {
        string? filepath = await reader.ReadLineAsync();
        if (filepath == "END") break;
        if (filepath is not null)
        {
            string? filename = filepath.Split(@"\")[^1];
            string fullpath = $@"{uploadpath}\{filename}";
            byte[] file_to_bytes = await File.ReadAllBytesAsync(filepath!);
            using (FileStream newfile = new FileStream(fullpath, FileMode.Create))
            {
                await newfile.WriteAsync(file_to_bytes);
                await newfile.FlushAsync();
            }

            await writer.WriteLineAsync($"{filepath} is downloaded");

            await writer.FlushAsync();
        }
       
    }
    Console.WriteLine($"Files from {client.Client.RemoteEndPoint} are downloaded ");
}

