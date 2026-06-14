using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Player;
using DungeonEclipse.Core;
using DungeonEclipse.UI;

namespace DungeonEclipse.Interactables
{
    /// <summary>
    /// Célula-alvo: ao Kael entrar nela, avança para a próxima sala (com fade) se
    /// houver uma cena seguinte configurada, ou dispara a vitória (sala final).
    /// </summary>
    public class GoalTrigger : MonoBehaviour
    {
        private Vector2Int _cell;
        private PlayerController _player;
        private string _nextScene;
        private System.Func<bool> _unlocked; // null = sempre aberto
        private bool _triggered;
        private bool _warnedLocked;

        public void Init(Board board, Vector2Int cell, PlayerController player,
            string nextScene = null, System.Func<bool> unlocked = null)
        {
            _cell = cell;
            _player = player;
            _nextScene = nextScene;
            _unlocked = unlocked;
            transform.position = board.Grid.CellToWorld(cell.x, cell.y);
        }

        private void Update()
        {
            if (_triggered || _player == null) return;
            if (_player.Mover.Cell != _cell)
            {
                _warnedLocked = false; // saiu da célula: pode avisar de novo
                return;
            }

            if (_unlocked != null && !_unlocked())
            {
                if (!_warnedLocked)
                {
                    Messages.Raise("A sala ainda está corrompida — restaure os núcleos.");
                    _warnedLocked = true;
                }
                return;
            }

            _triggered = true;
            if (!string.IsNullOrEmpty(_nextScene))
            {
                Messages.Raise("Fase Concluída");
                ScreenFader.Ensure().FadeToScene(_nextScene);
            }
            else if (GameManager.Instance != null)
            {
                GameManager.Instance.Vitoria();
            }
        }
    }
}
