using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Combat;
using DungeonEclipse.Core;
using DungeonEclipse.Interactables;
using DungeonEclipse.CameraRig;
using DungeonEclipse.Effects;
using DungeonEclipse.Audio;

namespace DungeonEclipse.Player
{
    /// <summary>
    /// Controla o Kael: lê input de movimento (passo a passo na grade) e a ação
    /// de destruir um cristal ou atacar um guardião numa célula adjacente, com
    /// feedback sonoro e visual.
    /// </summary>
    [RequireComponent(typeof(GridMover))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int attackDamage = 1;

        public Health Health { get; private set; }
        public GridMover Mover { get; private set; }

        private CameraFollow _camera;

        private static readonly Vector2Int[] Directions =
            { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        public void Init(Board board, Vector2Int startCell, int maxHp)
        {
            Mover = GetComponent<GridMover>();
            Mover.Init(board, startCell);
            Health = new Health(maxHp);
            Health.OnDied += HandleDeath;
        }

        public void SetCamera(CameraFollow camera) => _camera = camera;

        private void HandleDeath()
        {
            if (GameManager.Instance != null) GameManager.Instance.Derrota();
        }

        private void Update()
        {
            if (Time.timeScale == 0f) return; // pausado (ex.: intro aberto)
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

            if (dir != Vector2Int.zero && Mover.TryMove(dir))
                Sfx.Move();
        }

        private void HandleAction()
        {
            if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.E)) return;
            foreach (var d in Directions)
            {
                var cell = Mover.Cell + d;

                var crystal = FindCrystalAt(cell);
                if (crystal != null)
                {
                    Vector3 pos = crystal.transform.position;
                    crystal.DestroyCrystal();
                    HitEffect.Burst(pos, new Color(0.6f, 0.3f, 0.9f));
                    Sfx.Destroy();
                    if (_camera != null) _camera.Shake(0.08f, 0.08f);
                    Messages.Raise("Cristal Destruído");
                    return;
                }

                var guardian = FindGuardianAt(cell);
                if (guardian != null)
                {
                    Vector3 pos = guardian.transform.position;
                    bool defeated = guardian.Engage(Health, attackDamage);
                    HitEffect.Burst(pos, new Color(1f, 0.4f, 0.3f));
                    Sfx.Attack();
                    if (_camera != null) _camera.Shake(0.12f, 0.12f);
                    Messages.Raise(defeated ? "Guardião Derrotado" : "Atingiu o Guardião");
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

        private Guardian FindGuardianAt(Vector2Int cell)
        {
            foreach (var g in FindObjectsOfType<Guardian>())
                if (!g.Defeated && g.Cell == cell) return g;
            return null;
        }
    }
}
