using UnityEngine;

namespace DungeonEclipse.Grid
{
    /// <summary>
    /// Lógica e dados puros da grade do tabuleiro. Sem dependência de cena —
    /// testável em EditMode. Fonte única da verdade sobre quais células são
    /// andáveis e como converter entre coordenadas de célula e de mundo.
    /// </summary>
    public class BoardGrid
    {
        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; }
        public Vector2 Origin { get; }

        private readonly bool[,] _walkable;

        public BoardGrid(int width, int height, float cellSize, Vector2 origin)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            Origin = origin;
            _walkable = new bool[width, height];
        }

        public bool IsInside(int x, int y)
            => x >= 0 && x < Width && y >= 0 && y < Height;

        public bool IsWalkable(int x, int y)
            => IsInside(x, y) && _walkable[x, y];

        public void SetWalkable(int x, int y, bool value)
        {
            if (IsInside(x, y))
                _walkable[x, y] = value;
        }

        public Vector3 CellToWorld(int x, int y)
            => new Vector3(Origin.x + x * CellSize, Origin.y + y * CellSize, 0f);

        public Vector2Int WorldToCell(Vector3 world)
        {
            int x = Mathf.RoundToInt((world.x - Origin.x) / CellSize);
            int y = Mathf.RoundToInt((world.y - Origin.y) / CellSize);
            return new Vector2Int(x, y);
        }
    }
}
