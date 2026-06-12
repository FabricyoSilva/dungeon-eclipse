using System.Collections;
using UnityEngine;

namespace DungeonEclipse.Grid
{
    /// <summary>
    /// Move uma entidade célula a célula sobre um Board, com deslize suave.
    /// Reusável (Kael e futuros inimigos).
    /// </summary>
    public class GridMover : MonoBehaviour
    {
        [SerializeField] private float moveDuration = 0.12f;

        private Board _board;
        public Vector2Int Cell { get; private set; }
        public bool IsMoving { get; private set; }

        public void Init(Board board, Vector2Int startCell)
        {
            _board = board;
            Cell = startCell;
            transform.position = _board.Grid.CellToWorld(startCell.x, startCell.y);
        }

        /// <summary>Tenta mover 1 célula na direção; retorna true se iniciou.</summary>
        public bool TryMove(Vector2Int dir)
        {
            if (IsMoving || _board == null) return false;
            var target = Cell + dir;
            if (!_board.Grid.IsWalkable(target.x, target.y)) return false;
            StartCoroutine(MoveRoutine(target));
            return true;
        }

        private IEnumerator MoveRoutine(Vector2Int target)
        {
            IsMoving = true;
            Vector3 from = transform.position;
            Vector3 to = _board.Grid.CellToWorld(target.x, target.y);
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / moveDuration;
                transform.position = Vector3.Lerp(from, to, Mathf.Clamp01(t));
                yield return null;
            }
            transform.position = to;
            Cell = target;
            IsMoving = false;
        }
    }
}
