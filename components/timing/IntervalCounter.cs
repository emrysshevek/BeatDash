using Godot;

public partial class IntervalCounter : Metronome
{
	[Export]
	public int BeatsPerInterval = 1;
	[Export]
	public Metronome Metronome;

	public override double SecondsPerBeat
	{
		get => Metronome.SecondsPerBeat * BeatsPerInterval;
	}
	private int Count = 0;

	public override void _Ready()
	{
		Metronome ??= GetNode<Metronome>("/root/Metronome");
		Metronome.Beat += OnBeat;
	}

	private void OnBeat()
	{
		Count = (Count + 1) % BeatsPerInterval;
		if (Count == 0)
		{
			EmitSignal(SignalName.Beat);
		}
	}
}
