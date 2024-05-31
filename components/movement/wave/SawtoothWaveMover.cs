using Godot;
using System;

public partial class SawtoothWaveMover : BaseWaveMover
{
    public float Period { get => 1 / _frequency; }
    public override float Phase 
    { 
        get => _phase / Period - .5f; 
        set
        {
            _cachedPhase = value;
            _phase = (value + 0.5f) * Period; // magic numbers but they work!
        } 
    }

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);

        var yoffset = 2 * _amplitude * ((_phase-_t)/Period - Mathf.Floor(.5f + (_phase-_t)/Period));
        var rotatedOffset = GetBasis().BasisXform(new Vector2(0, (float)yoffset));

        GD.Print($"Relative offset: {yoffset}, rotatedOffset: {rotatedOffset}");
        // Offset object's position
        Body.Position += rotatedOffset - _prevOffset;
        _prevOffset = rotatedOffset;

        _t += (float)delta % _frequency;
    }
}
