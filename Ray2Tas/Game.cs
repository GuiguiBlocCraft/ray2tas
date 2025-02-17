using MTK;
using MTK.MemUtils.Pointers;

namespace Ray2Tas;

internal class Game
{
	private StringPointer levelName;
	private Pointer<float> posX;
	private Pointer<float> posY;
	private Pointer<float> posZ;
	private Pointer<bool> isLoading;

	private readonly MemoryManager memory;

	public Game()
	{
		memory = new("Rayman2")
		{
			OnHook = () =>
			{
				InitPointers();
			}
		};
	}

	public void Check()
	{
		if(memory.IsHooked)
			memory.TickUp();
	}

	public bool IsHooked()
	{
		if(memory != null)
			return memory.IsHooked;
		return false;
	}

	public void InitPointers()
	{
		levelName = new(memory, 0x10039F);
		isLoading = new(memory, 0x11663C);
		posX = new(memory, 0x100578, 0x4, 0x0, 0x1C);
		posY = new(memory, 0x100578, 0x4, 0x0, 0x20);
		posZ = new(memory, 0x100578, 0x4, 0x0, 0x24);
	}

	public void Loop()
	{
		if(levelName.Old != levelName.Current)
		{
			Console.WriteLine(levelName.Current);
		}

		if(posX.Old != posX.Current
			|| posY.Old != posY.Current
			|| posZ.Old != posZ.Current)
			Console.WriteLine($"X: {posX.Current} / Y: {posY.Current} / Z: {posZ.Current}");

		if(!isLoading.Old && isLoading.Current)
		{
			Console.WriteLine("Loading");
		}
		else if(isLoading.Old && !isLoading.Current)
		{
			Console.WriteLine("Not loading");
		}
	}

	public bool IsLoading()
	{
		return levelName.Current == "" || isLoading.Current;
	}
}