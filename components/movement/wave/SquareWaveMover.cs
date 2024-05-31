using Godot;
using System;

public partial class SquareWaveMover : BaseWaveMover
{
    public float Period { get => 1 / _frequency; }
    public override float Phase 
    { 
        get => _phase / (_frequency * 2); 
        set
        {
            _cachedPhase = value;
            _phase = 2 * Mathf.Pi * _frequency; // magic numbers but they work!
        } 
    }

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);

        var yoffset = _amplitude * Mathf.Sign(Mathf.Cos(2 * Mathf.Pi * _frequency * _t + _phase));
        var rotatedOffset = GetBasis().BasisXform(new Vector2(0, yoffset));

        GD.Print($"Relative offset: {yoffset}, rotatedOffset: {rotatedOffset}");
        // Offset object's position
        Body.Position += rotatedOffset - _prevOffset;
        _prevOffset = rotatedOffset;

        _t += (float)delta % _frequency;
    }
}
