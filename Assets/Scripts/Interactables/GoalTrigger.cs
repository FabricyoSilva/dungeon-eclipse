using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Player;
using DungeonEclipse.Core;

namespace DungeonEclipse.Interactables
{
    /// <summary>Célula-alvo: ao Kael entrar nela, dispara a vitória.</summary>
    public class GoalTrigger : MonoBehaviour
    {
        private Vector2Int _cell;
        private PlayerController _player;
        private bool _triggered;

        public void Init(Board board, Vector2Int cell, PlayerController player)
        {
            _cell = cell;
            _player = player;
            transform.position = board.Grid.CellToWorld(cell.x, cell.y);
        }

        private void Update()
        {
            if (_triggered || _player == null) return;
            if (_player.Mover.Cell == _cell)
            {
                _triggered = true;
                if (GameManager.Instance != null)
                    GameManager.Instance.Vitoria();
            }
        }
    }
}
