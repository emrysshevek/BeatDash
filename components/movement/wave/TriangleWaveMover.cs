using Godot;
using System;

public partial class TriangleWaveMover : BaseWaveMover
{
    private float Period 
    {
        get => 1 / _frequency;
    }

    public override float Phase 
    { 
        get => _phase / Period; 
        set
        {
            _cachedPhase = value;
            _phase = (value + 0.25f) * Period * 4; // magic numbers but they work!
        } 
    }

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);

        var yoffset = (4 * _amplitude / Period) * Mathf.Abs(Mathf.PosMod(_t - ((Period-_phase)/4), Period) - (Period / 2)) - _amplitude;
        var rotatedOffset = GetBasis().BasisXform(new Vector2(0, (float)yoffset));

        GD.Print($"Relative offset: {yoffset}, rotatedOffset: {rotatedOffset}");
        // Offset object's position
        Body.Position += rotatedOffset - _prevOffset;
        _prevOffset = rotatedOffset;

        _t += (float)delta % _frequency;
    }
}
