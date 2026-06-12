using NUnit.Framework;
using UnityEngine;
using DungeonEclipse.Grid;

public class BoardGridTests
{
    [Test]
    public void NewGrid_AllCellsNotWalkable_ByDefault()
    {
        var g = new BoardGrid(3, 3, 1f, Vector2.zero);
        Assert.IsFalse(g.IsWalkable(1, 1));
    }

    [Test]
    public void SetWalkable_MakesCellWalkable()
    {
        var g = new BoardGrid(3, 3, 1f, Vector2.zero);
        g.SetWalkable(1, 1, true);
        Assert.IsTrue(g.IsWalkable(1, 1));
    }

    [Test]
    public void IsWalkable_OutsideBounds_ReturnsFalse()
    {
        var g = new BoardGrid(3, 3, 1f, Vector2.zero);
        g.SetWalkable(1, 1, true);
        Assert.IsFalse(g.IsWalkable(-1, 0));
        Assert.IsFalse(g.IsWalkable(3, 3));
    }

    [Test]
    public void CellToWorld_UsesOriginAndCellSize()
    {
        var g = new BoardGrid(3, 3, 2f, new Vector2(10, 20));
        Assert.AreEqual(new Vector3(12, 22, 0), g.CellToWorld(1, 1));
    }

    [Test]
    public void WorldToCell_RoundsToNearestCell()
    {
        var g = new BoardGrid(3, 3, 2f, new Vector2(10, 20));
        Assert.AreEqual(new Vector2Int(1, 1), g.WorldToCell(new Vector3(12.4f, 21.6f, 0)));
    }
}
