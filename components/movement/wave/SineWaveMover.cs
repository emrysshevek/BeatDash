using Godot;
using System;

public partial class SineWaveMover : BaseWaveMover
{
    public override float Phase 
    { 
        get => _phase / (2 * Mathf.Pi );
        set
        {
            _phase = 2 * Mathf.Pi * value;
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
        
        // Calculate relative offset
        var offset = new Vector2(0, _amplitude * Mathf.Cos(2 * Mathf.Pi * _frequency * _t + _phase));

        // Rotate based on current direction vector
        var rotatedOffset = GetBasis().BasisXform(offset);
        GD.Print($"Basis={GetBasis()}, Regular offset={offset}, rotated offset={rotatedOffset}");

        // Offset object's position
        Body.Position += rotatedOffset - _prevOffset;
        _prevOffset = rotatedOffset;

        // GD.Print($"offset={rotatedOffset}");        

        _t += (float)delta % _frequency;
    }
}
