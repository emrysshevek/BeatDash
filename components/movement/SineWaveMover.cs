using Godot;
using System;

public partial class SineWaveMover : BaseWaveMover
{

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
        
        // Calculate relative offset
        var offset = new Vector2(0, _amplitude * Mathf.Cos(2 * Mathf.Pi * _frequency * _t));
        // GD.Print($"Regular offset={offset}, a={_amplitude}, f={_frequency}");

        // Rotate based on current direction vector
        var rotatedOffset = GetBasis().BasisXform(offset);

        // Offset object's position
        Body.Position += rotatedOffset - _prevOffset;
        _prevOffset = rotatedOffset;

        // GD.Print($"offset={rotatedOffset}");        

        _t += (float)delta % _frequency;
    }
}
