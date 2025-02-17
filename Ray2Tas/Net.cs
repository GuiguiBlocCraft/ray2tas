using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Ray2Tas;

class NetServer
{
	private TcpListener server;
	private bool isRunning;
	private Tas TasControl;

	public NetServer(Tas tas)
	{
		TasControl = tas;
	}

	public void Start(string ipAddress, int port)
	{
		server = new TcpListener(IPAddress.Parse(ipAddress), port);
		server.Start();

		isRunning = true;
		Console.WriteLine($"Server started");

		Task.Run(() => ListenForClients());
	}

	private async Task ListenForClients()
	{
		while(isRunning)
		{
			TcpClient client = await server.AcceptTcpClientAsync();
			Console.WriteLine("Client connected");
			Task.Run(() => HandleClient(client));
		}
	}

	private async Task HandleClient(TcpClient client)
	{
		var buffer = new byte[1024];
		var stream = client.GetStream();

		while(isRunning)
		{
			int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
			if(bytesRead == 0)
			{
				Console.WriteLine("Client disconnected");
				break;
			}

			string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
			switch(message.TrimEnd())
			{
				case "start":
					TasControl.Start();
					Console.WriteLine("TAS started!");
					break;
				case "stop":
					TasControl.Reset();
					Console.WriteLine("TAS resetted!");
					break;
			}
		}

		client.Close();
	}

	public void Stop()
	{
		isRunning = false;
		server.Stop();
		Console.WriteLine("Server stopped");
	}
}