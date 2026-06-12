using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Player;
using DungeonEclipse.Interactables;
using DungeonEclipse.CameraRig;

namespace DungeonEclipse.Core
{
    /// <summary>
    /// Monta o Andar 1 no Start: instancia Kael, cristal e sala-alvo sobre o
    /// Board e conecta câmera e HUD. As posições são dados (Inspector).
    /// </summary>
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private Board board;
        [SerializeField] private HudController hud;
        [SerializeField] private CameraFollow cameraFollow;

        [Header("Posições na grade")]
        [SerializeField] private Vector2Int startCell = new Vector2Int(1, 1);
        [SerializeField] private Vector2Int crystalCell = new Vector2Int(4, 1);
        [SerializeField] private Vector2Int guardianCell = new Vector2Int(7, 3);
        [SerializeField] private Vector2Int goalCell = new Vector2Int(7, 5);
        [SerializeField] private int playerHp = 5;

        [Header("Progressão")]
        [SerializeField] private bool spawnGuardian = false;
        [SerializeField] private string nextScene = ""; // vazio = sala final (vitória)

        private void Start()
        {
            if (board == null) board = FindObjectOfType<Board>();
            if (hud == null) hud = FindObjectOfType<HudController>();
            if (cameraFollow == null) cameraFollow = FindObjectOfType<CameraFollow>();

            // Kael
            var kael = new GameObject("Kael");
            var sr = kael.AddComponent<SpriteRenderer>();
            sr.sprite = PlaceholderSprite.Square;
            sr.color = new Color(0.3f, 0.6f, 1f);
            sr.sortingOrder = 5;
            kael.transform.localScale = Vector3.one * 0.7f;
            kael.AddComponent<GridMover>();
            var player = kael.AddComponent<PlayerController>();
            player.Init(board, startCell, playerHp);

            // Cristal
            var crystalGo = new GameObject("Crystal");
            var csr = crystalGo.AddComponent<SpriteRenderer>();
            csr.sprite = PlaceholderSprite.Square;
            csr.color = new Color(0.5f, 0.2f, 0.7f);
            csr.sortingOrder = 4;
            crystalGo.AddComponent<Crystal>().Init(board, crystalCell);

            // Guardião (vermelho) — bloqueia a subida até o alvo (só nas salas de combate)
            if (spawnGuardian)
            {
                var guardianGo = new GameObject("Guardian");
                var gdsr = guardianGo.AddComponent<SpriteRenderer>();
                gdsr.sprite = PlaceholderSprite.Square;
                gdsr.color = new Color(0.8f, 0.2f, 0.2f);
                gdsr.sortingOrder = 4;
                guardianGo.AddComponent<Guardian>().Init(board, guardianCell);
            }

            // Sala-alvo
            var goalGo = new GameObject("Goal");
            var gsr = goalGo.AddComponent<SpriteRenderer>();
            gsr.sprite = PlaceholderSprite.Square;
            gsr.color = new Color(1f, 0.84f, 0.3f);
            gsr.sortingOrder = 1;
            goalGo.AddComponent<GoalTrigger>().Init(board, goalCell, player, nextScene);

            // Câmera + HUD
            if (cameraFollow != null) cameraFollow.SetTarget(kael.transform);
            if (hud != null) hud.Bind(player.Health);

            Messages.Raise("Use WASD para mover. Espaço destrói cristais.");
        }
    }
}
