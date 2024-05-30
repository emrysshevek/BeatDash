using Godot;
using System;

public partial class SineWaveMover : BaseWaveMover
{

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
