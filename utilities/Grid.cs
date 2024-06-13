using Godot;
using System.Collections.Generic;

public partial class Grid : Node2D
{
	private Node2D CellContainer;
	private PackedScene CellScene = GD.Load<PackedScene>("res://utilities/cell.tscn");
	private Dictionary<Vector2I, Cell> Cells = new();

	public override void _Ready()
	{
		CellContainer = GetNode<Node2D>("CellContainer");
		foreach (var node in CellContainer.GetChildren())
		{
			if (node is Cell cell) AddCell(cell);
		}
	}

	public void AddCell(Cell cell)
	{
		if (cell.Coordinates == Vector2I.MaxValue)
		{
			cell.Coordinates = (Vector2I) (cell.GlobalPosition / Global.TileSize);
		}
		GD.Print($"Added cell {cell.Name} at {cell.Coordinates}");
		Cells[cell.Coordinates] = cell;
		for (int i = -1; i < 1; i++)
		{
			for (int j = -1; j < 1; j++)
			{
				var direction = new Vector2I(i, j);
				if (direction != Vector2I.Zero && Cells.TryGetValue(direction + cell.Coordinates, out Cell neighbor))
                {
					GD.Print($"Adding neighbor at {neighbor.Coordinates} to {direction}");
                    cell.Neighbors[direction] = neighbor;
					neighbor.Neighbors[-direction] = cell;
                }
			}
		}
	}

    public void CreateCell(Vector2I coord)
	{
		if (Cells.Count > 0 || HasNeighbor(coord))
		{
			var cell = CellScene.Instantiate<Cell>();
			cell.Coordinates = coord;
			cell.GlobalPosition = coord * Global.TileSize;
			CellContainer.AddChild(cell);
			AddCell(cell);
		}
	}

	public bool HasNeighbor(Vector2I coord)
	{
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				var offset = new Vector2I(i, j);
				if (Cells.ContainsKey(coord + offset))
				{
					return true;
				}
			}
		}
		return false;
	}

	public Cell[] GetNeighbors(Vector2I coord)
	{
		var cells = new Cell[8];
		int cellIdx = 0;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				var offset = new Vector2I(i, j);
                if (Cells.TryGetValue(coord + offset, out Cell cell))
                {
                    cells[cellIdx] = cell;
                }
				cellIdx++;
            }
		}
		return cells;
	}

	public Vector2I GetCenterCoordinate()
	{
		int minx, miny, maxx, maxy;
		minx = miny = int.MaxValue;
		maxx = maxy = int.MinValue;

		foreach (var coord in Cells.Keys)
		{
			if (coord.X > maxx) maxx = coord.X;
			if (coord.X < minx) minx = coord.X;

			if (coord.Y > maxy) maxy = coord.Y;
			if (coord.Y < miny) miny = coord.Y;
		}

		return new Vector2I((maxx - minx) / 2, (maxy - miny) / 2);
	}

	public Vector2 GetCenterPosition()
	{
		float minx, miny, maxx, maxy;
		minx = miny = float.PositiveInfinity;
		maxx = maxy = float.NegativeInfinity;

		foreach (Cell cell in Cells.Values)
		{
			if (cell.GlobalPosition.X > maxx) maxx = cell.GlobalPosition.X;
			if (cell.GlobalPosition.X < minx) minx = cell.GlobalPosition.X;

			if (cell.GlobalPosition.Y > maxy) maxy = cell.GlobalPosition.Y;
			if (cell.GlobalPosition.Y < miny) miny = cell.GlobalPosition.Y;
		}

		return new Vector2((maxx - minx) / 2, (maxy - miny) / 2);
	}
}
