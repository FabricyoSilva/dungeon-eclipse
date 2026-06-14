using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Combat;
using DungeonEclipse.Core;
using DungeonEclipse.Player;

namespace DungeonEclipse.Interactables
{
    /// <summary>
    /// Guardião corrompido: peça estacionária que ocupa e bloqueia uma célula.
    /// Tem vida própria e contra-ataca quem o ataca (corpo-a-corpo). Enquanto o
    /// jogador permanece adjacente, drena a vida dele (dano de proximidade). Ao
    /// morrer, libera a célula no BoardGrid e some.
    /// </summary>
    public class Guardian : MonoBehaviour
    {
        [SerializeField] private int maxHp = 3;
        [SerializeField] private int counterDamage = 1;

        [Header("Dano por proximidade")]
        [SerializeField] private float proximityInterval = 1.2f;
        [SerializeField] private int proximityDamage = 1;

        private Board _board;
        private PlayerController _player;
        private float _proxTimer;

        public Vector2Int Cell { get; private set; }
        public Health Health { get; private set; }
        public bool Defeated { get; private set; }

        public void Init(Board board, Vector2Int cell, PlayerController player)
        {
            _board = board;
            _player = player;
            Cell = cell;
            Health = new Health(maxHp);
            transform.position = board.Grid.CellToWorld(cell.x, cell.y);
            _board.Grid.SetWalkable(cell.x, cell.y, false); // bloqueia
        }

        /// <summary>
        /// Recebe um ataque corpo-a-corpo. Aplica o dano, e se sobreviver
        /// contra-ataca o atacante. Retorna true se foi derrotado nesta troca.
        /// </summary>
        public bool Engage(Health attackerHealth, int attackerDamage)
        {
            if (Defeated) return true;
            bool died = CombatResolver.ResolveMelee(Health, attackerDamage,
                attackerHealth, counterDamage);
            if (died) DefeatGuardian();
            return died;
        }

        private void DefeatGuardian()
        {
            if (Defeated) return;
            Defeated = true;
            _board.Grid.SetWalkable(Cell.x, Cell.y, true); // abre o caminho
            Destroy(gameObject);
        }

        private void Update()
        {
            // leve pulsar placeholder
            float s = (0.75f + 0.07f * Mathf.Sin(Time.time * 4f));
            transform.localScale = new Vector3(s, s, 1f);

            if (Defeated || _player == null) return;

            bool jogando = GameManager.Instance == null
                || GameManager.Instance.State == GameState.Jogando;

            _proxTimer += Time.deltaTime;
            if (_proxTimer >= proximityInterval)
            {
                _proxTimer = 0f;
                if (ProximityRule.ShouldDamage(Cell, _player.Mover.Cell, Defeated, jogando))
                    _player.Health.TakeDamage(proximityDamage);
            }
        }
    }
}
