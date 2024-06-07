using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class BaseAudio : Node2D
{
	[Export(PropertyHint.Dir)]
	public string AudioDirectory { get; set; }
	[Export]
	public BaseActor Actor;
	[Export]
	public bool TriggerOnWallCollision = true;
	[Export]
	public bool TriggerOnObjectCollision = true;
	[Export]
	public bool TriggerOnCellIntersection = true;

	private AudioStreamPlayer2D[] Players = new AudioStreamPlayer2D[8];

	public override void _Ready()
	{
		LoadAudioStreams();
		ConnectTriggers();
	}

	private void LoadAudioStreams()
	{
		if (AudioDirectory == null)
		{
			GD.PushError("Audio Directory missing. Please add");
		}
		else 
		{
			using var dir = DirAccess.Open(AudioDirectory);
			if (dir != null)
			{
				dir.ListDirBegin();
				string fileName = dir.GetNext();
				while (fileName != "")
				{
					var filepath = AudioDirectory + "\\" + fileName;
					if (!dir.CurrentIsDir() && !fileName.Contains(".import"))
					{
            GD.Print($"Loading audio file: {fileName}");
            GD.Print($"idx: {fileName.Split(".")[0][^1] - '0'}");
						var idx = fileName.Split(".")[0][^1] - '0';
						if (idx < 0 || idx > 7) GD.PushWarning($"Invalid audio filename: {fileName}");
						var stream = GD.Load<AudioStream>(filepath);
            var player = new AudioStreamPlayer2D();
            player.Stream = stream;
						AddChild(player);
						Players[idx] = player;
					}
					fileName = dir.GetNext();
				}
			}
			else
			{
				GD.Print("An error occurred when trying to access the path.");
			}
		}
	}

	private void ConnectTriggers()
	{
		if (Actor == null) GD.PushError("Actor is missing. pls add uwu");
		else 
		{
			if (TriggerOnObjectCollision) Actor.ObjectCollision += OnSoundTrigger;
			if (TriggerOnWallCollision) Actor.WallCollision += OnSoundTrigger;
			if (TriggerOnCellIntersection) Actor.CellIntersection += OnSoundTrigger;
		}
	}

	public void OnSoundTrigger()
	{
		var cellIdx = (Vector2I)(GlobalPosition / Global.TileSize);
    GD.Print($"Playing sound at position {cellIdx}");
		Players[cellIdx.Y].Play();
	}
}
