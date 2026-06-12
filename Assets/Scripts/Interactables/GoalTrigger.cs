using UnityEngine;
using UnityEngine.SceneManagement;
using DungeonEclipse.Grid;
using DungeonEclipse.Player;
using DungeonEclipse.Core;

namespace DungeonEclipse.Interactables
{
    /// <summary>
    /// Célula-alvo: ao Kael entrar nela, avança para a próxima sala (se houver
    /// uma cena seguinte configurada) ou dispara a vitória (sala final).
    /// </summary>
    public class GoalTrigger : MonoBehaviour
    {
        private Vector2Int _cell;
        private PlayerController _player;
        private string _nextScene;
        private bool _triggered;

        public void Init(Board board, Vector2Int cell, PlayerController player,
            string nextScene = null)
        {
            _cell = cell;
            _player = player;
            _nextScene = nextScene;
            transform.position = board.Grid.CellToWorld(cell.x, cell.y);
        }

        private void Update()
        {
            if (_triggered || _player == null) return;
            if (_player.Mover.Cell == _cell)
            {
                _triggered = true;
                if (!string.IsNullOrEmpty(_nextScene))
                {
                    Messages.Raise("Fase Concluída");
                    SceneManager.LoadScene(_nextScene);
                }
                else if (GameManager.Instance != null)
                {
                    GameManager.Instance.Vitoria();
                }
            }
        }
    }
}
