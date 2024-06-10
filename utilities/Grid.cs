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
		Cells[cell.Coordinates] = cell;
	}

    public void CreateCell(Vector2I coord)
	{
		if (Cells.Count > 0 || HasNeighbor(coord))
		{
			var cell = CellScene.Instantiate<Cell>();
			cell.Coordinates = coord;
			CellContainer.AddChild(cell);
			Cells[coord] = cell;
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
}
