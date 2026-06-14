using System.Collections.Generic;
using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Core;
using DungeonEclipse.Building;

namespace DungeonEclipse.Interactables
{
    /// <summary>
    /// Mecanismo a construir (ponte/plataforma) que ocupa e bloqueia um conjunto
    /// de células de abismo. Ao ser construído — com materiais suficientes —
    /// libera as células no BoardGrid e revela as tábuas da ponte.
    /// </summary>
    public class BuildSite : MonoBehaviour
    {
        private Board _board;
        private BuildInventory _inventory;
        private int _required;
        private readonly List<Vector2Int> _cells = new List<Vector2Int>();

        public bool Built { get; private set; }
        public int Required => _required;
        public IReadOnlyList<Vector2Int> Cells => _cells;

        public void Init(Board board, IEnumerable<Vector2Int> cells,
            BuildInventory inventory, int required)
        {
            _board = board;
            _inventory = inventory;
            _required = required;
            _cells.Clear();
            _cells.AddRange(cells);
            foreach (var c in _cells)
                _board.Grid.SetWalkable(c.x, c.y, false); // abismo intransponível
            if (_cells.Count > 0)
                transform.position = board.Grid.CellToWorld(_cells[0].x, _cells[0].y);
        }

        /// <summary>True se a célula faz parte do abismo ainda não construído.</summary>
        public bool Occupies(Vector2Int cell) => !Built && _cells.Contains(cell);

        /// <summary>
        /// Tenta construir. Retorna true se construiu agora (ou já estava
        /// construído). False se faltam materiais.
        /// </summary>
        public bool TryBuild()
        {
            if (Built) return true;
            int have = _inventory != null ? _inventory.Materials : 0;
            if (!BuildRule.CanBuild(have, _required, Built)) return false;

            _inventory?.Spend(_required);
            Built = true;
            foreach (var c in _cells)
            {
                _board.Grid.SetWalkable(c.x, c.y, true); // ponte transponível
                SpawnPlank(c);
            }
            return true;
        }

        private void SpawnPlank(Vector2Int cell)
        {
            var go = new GameObject($"Plank_{cell.x}_{cell.y}");
            go.transform.SetParent(transform);
            go.transform.position = _board.Grid.CellToWorld(cell.x, cell.y);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = PlaceholderSprite.Square;
            sr.color = new Color(0.5f, 0.36f, 0.2f); // madeira
            sr.sortingOrder = 1;
        }
    }
}
