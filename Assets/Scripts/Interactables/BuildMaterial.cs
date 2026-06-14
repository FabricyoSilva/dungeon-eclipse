using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Building;

namespace DungeonEclipse.Interactables
{
    /// <summary>
    /// Engrenagem antiga coletável (material de construção). Fica sobre uma célula
    /// andável; ao ser coletada, soma ao inventário de construção e some.
    /// </summary>
    public class BuildMaterial : MonoBehaviour
    {
        private BuildInventory _inventory;
        public Vector2Int Cell { get; private set; }
        public bool Collected { get; private set; }

        public void Init(Board board, Vector2Int cell, BuildInventory inventory)
        {
            _inventory = inventory;
            Cell = cell;
            transform.position = board.Grid.CellToWorld(cell.x, cell.y);
        }

        /// <summary>Coleta a engrenagem e devolve o total acumulado.</summary>
        public int Collect()
        {
            if (Collected) return _inventory != null ? _inventory.Materials : 0;
            Collected = true;
            _inventory?.Add(1);
            int total = _inventory != null ? _inventory.Materials : 0;
            Destroy(gameObject);
            return total;
        }

        private void Update()
        {
            // leve giro placeholder (engrenagem girando)
            transform.Rotate(0f, 0f, 60f * Time.deltaTime);
        }
    }
}
