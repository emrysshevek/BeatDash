using Godot;
using System;

public partial class BaseWaveMover : BaseMover
{
    // Unit is in tiles, but is counted from the midpoint so it's effectively doubled
    protected float _amplitude = Global.TileSize;
    [Export(PropertyHint.Range, "0,4,")]
    public float Amplitude
    {
        get => _amplitude / (float)Global.TileSize;
        set { _amplitude = value * Global.TileSize; }
    }

    // Wavelength gives the number of tiles between wave repetitions
    // Frequency is calculated based on speed and wavelength so they are both consistent
    protected float _frequency = 1;
    protected float _wavelength = Global.TileSize;
    private float _cachedWavelength;
    [Export]
    public float WaveLength
    {
        get => _wavelength / Global.TileSize;
        set 
        { 
            _cachedWavelength = value;
            _wavelength = value * Global.TileSize;
            _frequency = _speed / _wavelength; 
        }
    }

    // Phase is a ratio of Wavelength
    protected float _t = 0;
    protected float _phase = 0;
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
    protected float _speed = Global.TileSize;
    private float _cachedSpeed;
    [Export]
    public float Speed
    {
        get => _speed / Global.TileSize;
        set 
        { 
            _cachedSpeed = value;
            _speed = value * Global.TileSize / (float)(_m == null ? 1f : _m.SecondsPerBeat); 
        }
    }

    // Rotation is counter clockwise from the positive x-axis
    [Export(PropertyHint.Range, "-180,180,")]
    public float Rotation = 0f;

    protected Metronome _m;
    protected Vector2 _prevOffset = Vector2.Zero;

    public override void _Ready()
    {
        _m = GetNode<Metronome>("/root/Metronome");
        Speed = _cachedSpeed;
        WaveLength = _cachedWavelength;
        Body.Velocity = _speed * Vector2.Right.Rotated(Mathf.DegToRad(-Rotation));
        Engine.TimeScale = .25f;
    }

    public Transform2D GetBasis()
    {
        var xbasis = Body.Velocity.Normalized();
        var ybasis = xbasis.Rotated(Mathf.DegToRad(90));
        return new Transform2D(xbasis, ybasis, Body.Position);
    }

    public Vector2 GetRotatedOffset(Vector2 offset)
    {
        return GetBasis().BasisXform(offset);
    }

}
