using Nefarius.ViGEm.Client;

namespace Ray2Tas;

class Program
{
    public static ViGEmClient emClient = new();
	private static bool GameStarted = false;

	static int Main(string[] args)
	{
		Game Game = new();
		Tas Tas = new();
		NetServer net = new(Tas);

		if(args.Length == 0)
		{
			Console.WriteLine("Usage: ray2tas.exe FILENAME");
			return 1;
		}

		string fileName = args[0];

		// Read file
		Tas.ReadFile(fileName);
		Console.WriteLine($"File {fileName} readed!");

		// Start server to starting TAS remotely
		net.Start("0.0.0.0", 4444);

		// Loop
		while(true)
		{
			Game.Check();

			if(Game.IsHooked())
			{
				if(!GameStarted) {
					GameStarted = true;
					Console.WriteLine("Rayman2 started!");

					Tas.ConnectController();
				}

				if(!Game.IsLoading() && Tas.Started)
					Tas.Loop();
			}
			else if(GameStarted)
			{
				GameStarted = false;
				Console.WriteLine("Rayman2 stopped!");

				Tas.Reset();
				Tas.DisconnectController();
			}

			Thread.Sleep(16); // 60 fps, can be improved
		}
	}
}