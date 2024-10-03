using Ray2Tas.Controller;

namespace Ray2Tas;

class Tas
{
	private readonly OutputControllerXbox360 out_xbox = new();

	private List<OutputControllerXbox360InputState> InputLines = new();
	private int Tick = 0;

	public bool Started { get; set; }

	public void ReadFile(string fileName)
	{
		string text = File.ReadAllText(fileName);
		string[] lines = text.Split(Environment.NewLine);

		// Mapping
		foreach(string line in lines)
		{
			string[] col = line.Split(';');

			InputLines.Add(
				new OutputControllerXbox360InputState()
				{
					axis_left_x = short.Parse(col[0]), // X
					axis_left_y = short.Parse(col[1]), // Y
					a = col[2] == "1", // Jump
					x = col[3] == "1", // Shoot
					trigger_left = col[4] == "1" ? byte.MaxValue : byte.MinValue, // Strafe
					shoulder_left = col[5] == "1", // Camera left
					shoulder_right = col[6] == "1", // Camera right
					start = col[7] == "1", // Pause
					y = col[8] == "1", // View
					dpad_up = col[9] == "1", // Up
					dpad_down = col[10] == "1", // Down
					dpad_left = col[11] == "1", // Left
					dpad_right = col[12] == "1", // Right
					thumb_stick_right = col[13] == "1", // Reset camera
					trigger_right = col[14] == "1" ? byte.MaxValue : byte.MinValue, // Think
					back = col[15] == "1", // Photo
					b = col[16] == "1", // Accept button
				}
			);
		}
	}

	public void ConnectController()
	{
		out_xbox.Connect();
	}

	public void DisconnectController()
	{
		out_xbox.Disconnect();
	}

	public void Loop()
	{
		if(InputLines.Count == Tick)
		{
			Reset();
			Console.WriteLine("Done!");
		}
		else
		{
			// Send input
			out_xbox.UpdateInput(InputLines[Tick]);
			Console.WriteLine($"Tick: {Tick}");

			Tick++;
		}
	}

	public void Reset()
	{
		Tick = 0;
		Started = false;
		out_xbox.UpdateInput(new OutputControllerXbox360InputState()); // Reset inputs
	}
}