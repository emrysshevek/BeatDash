using Godot;

public partial class BeatIntervalComponent : Node
{
    [Signal]
	public delegate void IntervalElapsedEventHandler(); 

    [Export]
    public int Frequency = 1;

    public float SecondsPerInterval
    {
        get => (float) Metronome.SecondsPerBeat * Frequency;
    }
    private Metronome Metronome;
    private int Count = 0;

    public override void _Ready()
    {
        Metronome = GetNode<Metronome>("/root/Metronome");
		Metronome.Beat += OnBeat;
    }

    public void OnBeat()
    {
        Count = (Count + 1) % Frequency;
        if (Count == 0)
        {
            EmitSignal(SignalName.IntervalElapsed);
        }
    }
}
