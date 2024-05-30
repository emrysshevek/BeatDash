using Godot;
using System;

public partial class WaveMover : BaseMover
{
    // Unit is in tiles, but is counted from the midpoint so it's effectively doubled
    [Export(PropertyHint.Range, "0,4,")]
    private float _amplitude = Global.TileSize;
    [Export]
    public float Amplitude
    {
        get => _amplitude / (float)Global.TileSize;
        set { _amplitude = value * Global.TileSize; }
    }

    // Wavelength gives the number of tiles between wave repetitions
    // Frequency is calculated based on speed and wavelength so they are both consistent
    private float _frequency = 1;
    private float _wavelength = Global.TileSize;
    [Export]
    public float WaveLength
    {
        get => _wavelength / Global.TileSize;
        set 
        { 
            _wavelength = value * Global.TileSize;
            _frequency = _speed / _wavelength; 
        }
    }

    // Phase is a ratio of Wavelength
    private float _t = 0;
    private float _phase = 0;
    [Export(PropertyHint.Range, "0,1,")]
    public float Phase
    {
        get => _phase;
        set 
        {
            var newPhase = value * WaveLength;
            _t += newPhase - _phase; 
            _phase = newPhase;
        }
    }

    // Speed is strictly the propagation speed in the indicated direction, not the absolute speed of the object
    private float _speed = Global.TileSize;
    [Export]
    public float Speed
    {
        get => _speed / Global.TileSize;
        set 
        { 
            var speedScale = _m == null ? 1f : (float)(1/_m.SecondsPerBeat);
            _speed = value * Global.TileSize * speedScale; 
        }
    }

    // Rotation is counter clockwise from the positive x-axis
    [Export(PropertyHint.Range, "-180,180,")]
    public float Rotation = 0f;

    private Metronome _m;
    private Vector2 _prevOffset = Vector2.Zero;

    public override void _Ready()
    {
        Engine.TimeScale = .5;
        _m = GetNode<Metronome>("/root/Metronome");
        Body.Velocity = _speed * Vector2.Right.Rotated(Mathf.DegToRad(-Rotation));
    }

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
        
        // Calculate relative offset
        var offset = new Vector2(0, _amplitude * Mathf.Cos(2 * Mathf.Pi * _frequency * _t));
        GD.Print($"Regular offset={offset}, a={_amplitude}, f={_frequency}");

        // Rotate it based on current traveling direction (in case we bounced at an angle)
        var xbasis = Body.Velocity.Normalized();
        var ybasis = xbasis.Rotated(Mathf.DegToRad(90));
        var transform = new Transform2D(xbasis, ybasis, Body.Position);

        offset = transform.BasisXform(offset);

        // Offset object's position
        Body.Position += offset - _prevOffset;
        _prevOffset = offset;

        GD.Print($"offset={offset}");        

        _t += (float)delta % _frequency;
    }
}
