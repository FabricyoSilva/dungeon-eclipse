using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Combat;
using DungeonEclipse.Core;
using DungeonEclipse.Interactables;

namespace DungeonEclipse.Player
{
    /// <summary>
    /// Controla o Kael: lê input de movimento (passo a passo na grade) e a ação
    /// de destruir um cristal numa célula adjacente.
    /// </summary>
    [RequireComponent(typeof(GridMover))]
    public class PlayerController : MonoBehaviour
    {
        public Health Health { get; private set; }
        public GridMover Mover { get; private set; }

        private static readonly Vector2Int[] Directions =
            { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        public void Init(Board board, Vector2Int startCell, int maxHp)
        {
            Mover = GetComponent<GridMover>();
            Mover.Init(board, startCell);
            Health = new Health(maxHp);
        }

        private void Update()
        {
            if (GameManager.Instance != null &&
                GameManager.Instance.State != GameState.Jogando) return;
            HandleMove();
            HandleAction();
        }

        private void HandleMove()
        {
            Vector2Int dir = Vector2Int.zero;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) dir = Vector2Int.up;
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) dir = Vector2Int.down;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) dir = Vector2Int.left;
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) dir = Vector2Int.right;

            if (dir != Vector2Int.zero)
                Mover.TryMove(dir);
        }

        private void HandleAction()
        {
            if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.E)) return;
            foreach (var d in Directions)
            {
                var crystal = FindCrystalAt(Mover.Cell + d);
                if (crystal != null)
                {
                    crystal.DestroyCrystal();
                    Messages.Raise("Cristal Destruído");
                    return;
                }
            }
        }

        private Crystal FindCrystalAt(Vector2Int cell)
        {
            foreach (var c in FindObjectsOfType<Crystal>())
                if (!c.Destroyed && c.Cell == cell) return c;
            return null;
        }
    }
}
