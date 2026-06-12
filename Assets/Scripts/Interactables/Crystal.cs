using UnityEngine;
using DungeonEclipse.Grid;

namespace DungeonEclipse.Interactables
{
    /// <summary>
    /// Cristal corrompido que ocupa e bloqueia uma célula. Destrutível: ao ser
    /// destruído, libera a célula no BoardGrid e some.
    /// </summary>
    public class Crystal : MonoBehaviour
    {
        private Board _board;
        public Vector2Int Cell { get; private set; }
        public bool Destroyed { get; private set; }

        public void Init(Board board, Vector2Int cell)
        {
            _board = board;
            Cell = cell;
            transform.position = board.Grid.CellToWorld(cell.x, cell.y);
            transform.rotation = Quaternion.Euler(0, 0, 45f); // losango
            _board.Grid.SetWalkable(cell.x, cell.y, false);    // bloqueia
        }

        public void DestroyCrystal()
        {
            if (Destroyed) return;
            Destroyed = true;
            _board.Grid.SetWalkable(Cell.x, Cell.y, true); // abre o caminho
            Destroy(gameObject);
        }

        private void Update()
        {
            // leve pulsar placeholder
            float s = (0.6f + 0.08f * Mathf.Sin(Time.time * 4f));
            transform.localScale = new Vector3(s, s, 1f);
        }
    }
}
