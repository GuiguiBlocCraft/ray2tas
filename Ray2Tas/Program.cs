using Nefarius.ViGEm.Client;

namespace Ray2Tas;

class Program
{
    public static ViGEmClient emClient = new();
	private static bool Hooked = false;

	static void Main(string[] args)
	{
		Game Game = new();
		Tas Tas = new();

		string fileName = args[0];

		// Read file
		Tas.ReadFile(fileName);
		Console.WriteLine($"File {fileName} readed!");

		// Loop
		while(true)
		{
			Game.Check();

			if(Game.IsHooked())
			{
				if(!Hooked) {
					Hooked = true;
					Tas.ConnectController();
					Console.WriteLine("XBox360 controller connected!");

					Tas.Started = true;
				}

				if(!Game.IsLoading() && Tas.Started)
					Tas.Loop();
			}
			else if(Hooked)
			{
				Hooked = false;
				Tas.Reset();
				Tas.DisconnectController();
				Console.WriteLine("XBox360 controller disconnected!");
			}

			Thread.Sleep(16); // 60 fps, to improve
		}
	}
}