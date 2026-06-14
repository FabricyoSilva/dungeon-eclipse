using System.Collections.Generic;
using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Player;
using DungeonEclipse.Interactables;
using DungeonEclipse.Building;
using DungeonEclipse.Capture;
using DungeonEclipse.CameraRig;
using DungeonEclipse.Effects;
using DungeonEclipse.UI;

namespace DungeonEclipse.Core
{
    /// <summary>
    /// Monta a sala no Start: instancia Kael, cristal, (guardião) e sala-alvo
    /// sobre o Board e conecta câmera, HUD, dicas, efeitos e transições. As
    /// posições e flags de progressão são dados (Inspector).
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
        [SerializeField] private int playerHp = 6;

        [Header("Progressão")]
        [SerializeField] private bool spawnCrystal = true;
        [SerializeField] private bool spawnGuardian = false;
        [SerializeField] private string nextScene = ""; // vazio = sala final (vitória)

        [Header("Building (Andar 3 — Minas Escuras)")]
        [SerializeField] private Vector2Int[] buildMaterialCells = new Vector2Int[0];
        [SerializeField] private Vector2Int[] buildBridgeCells = new Vector2Int[0];
        [SerializeField] private int requiredMaterials = 0;

        [Header("Captura (Andar 4 — Jardins Subterrâneos)")]
        [SerializeField] private Vector2Int[] captureCells = new Vector2Int[0];

        [Header("Combate avançado (Andar 5 — Fortaleza Perdida)")]
        [SerializeField] private Vector2Int[] guardianCells = new Vector2Int[0];

        [Header("Final — Coração Eclipse")]
        [SerializeField] private int bossHp = 0;          // 0 = sem chefe
        [SerializeField] private Vector2Int bossCell = new Vector2Int(4, 3);
        [SerializeField] private int bossCounterDamage = 1;

        private void Start()
        {
            Time.timeScale = 1f;
            ScreenFader.Ensure().FadeIn();

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
            player.SetCamera(cameraFollow);

            var kaelFlash = kael.AddComponent<DamageFlash>();
            kaelFlash.Bind(player.Health);
            var kaelHurt = kael.AddComponent<HurtFeedback>();
            kaelHurt.Bind(player.Health, cameraFollow);

            // Cristal
            if (spawnCrystal)
            {
                var crystalGo = new GameObject("Crystal");
                var csr = crystalGo.AddComponent<SpriteRenderer>();
                csr.sprite = PlaceholderSprite.Square;
                csr.color = new Color(0.5f, 0.2f, 0.7f);
                csr.sortingOrder = 4;
                crystalGo.AddComponent<Crystal>().Init(board, crystalCell);
            }

            // Guardião(ões) vermelhos — bloqueiam o caminho até o alvo.
            // Andar 2: um guardião (spawnGuardian). Andar 5: vários (guardianCells).
            var guardians = new List<Guardian>();
            if (spawnGuardian) guardians.Add(SpawnGuardianAt(guardianCell, player));
            foreach (var gc in guardianCells) guardians.Add(SpawnGuardianAt(gc, player));
            bool advancedCombat = guardianCells != null && guardianCells.Length > 0;

            // Chefe final (Coração Eclipse): guardião reforçado que bloqueia o núcleo
            bool finalBoss = bossHp > 0;
            if (finalBoss)
                guardians.Add(SpawnGuardianAt(bossCell, player, bossHp, bossCounterDamage, true));

            // Building (Andar 3): engrenagens coletáveis + ponte sobre o abismo
            bool building = buildBridgeCells != null && buildBridgeCells.Length > 0;
            if (building)
            {
                var inventory = new BuildInventory();

                foreach (var mc in buildMaterialCells)
                {
                    var mGo = new GameObject("BuildMaterial");
                    var msr = mGo.AddComponent<SpriteRenderer>();
                    msr.sprite = PlaceholderSprite.Square;
                    msr.color = new Color(0.85f, 0.7f, 0.3f); // engrenagem dourada
                    msr.sortingOrder = 4;
                    mGo.transform.localScale = Vector3.one * 0.5f;
                    mGo.AddComponent<BuildMaterial>().Init(board, mc, inventory);
                }

                var siteGo = new GameObject("BuildSite");
                siteGo.AddComponent<BuildSite>().Init(board, buildBridgeCells, inventory, requiredMaterials);
            }

            // Captura (Andar 4): núcleos corrompidos que trancam a sala-alvo
            bool capturing = captureCells != null && captureCells.Length > 0;
            CaptureObjective objective = null;
            if (capturing)
            {
                objective = new CaptureObjective(captureCells.Length);
                foreach (var cc in captureCells)
                {
                    var nGo = new GameObject("CaptureNode");
                    var nsr = nGo.AddComponent<SpriteRenderer>();
                    nsr.sprite = PlaceholderSprite.Square;
                    nsr.sortingOrder = 4;
                    nGo.AddComponent<CaptureNode>().Init(board, cc, objective);
                }
                objective.OnChanged += (captured, total) =>
                {
                    if (captured >= total)
                        Messages.Raise(finalBoss
                            ? "O Coração Eclipse foi reconstruído!"
                            : "Equilíbrio restaurado — a sala dourada se abriu!");
                    else
                        Messages.Raise($"Núcleos restaurados: {captured}/{total}");
                };
            }

            // Sala-alvo (trancada até o objetivo de captura, se houver)
            var goalGo = new GameObject("Goal");
            var gsr = goalGo.AddComponent<SpriteRenderer>();
            gsr.sprite = PlaceholderSprite.Square;
            gsr.color = new Color(1f, 0.84f, 0.3f);
            gsr.sortingOrder = 1;
            System.Func<bool> goalUnlocked = null;
            if (capturing) goalUnlocked = () => objective.Complete;
            else if (advancedCombat) goalUnlocked = () => AllDefeated(guardians);
            goalGo.AddComponent<GoalTrigger>().Init(board, goalCell, player, nextScene, goalUnlocked);
            goalGo.AddComponent<GoalPulse>();

            // Câmera + HUD
            if (cameraFollow != null) cameraFollow.SetTarget(kael.transform);
            if (hud != null) hud.Bind(player.Health);

            // Dicas contextuais
            var hints = gameObject.AddComponent<TutorialHints>();
            hints.Init(player);
            if (spawnCrystal)
                hints.AddHint(crystalCell, 2, "Aperte Espaço perto do cristal para destruí-lo.");
            if (spawnGuardian)
                hints.AddHint(guardianCell, 2,
                    "Guardião à frente! Ataque com Espaço e não fique parado ao lado dele.");
            if (advancedCombat)
                hints.AddHint(guardianCells[0], 2,
                    "Vários guardiões guardam a fortaleza! Derrote todos para abrir a saída.");
            if (building)
            {
                if (buildMaterialCells.Length > 0)
                    hints.AddHint(buildMaterialCells[0], 2,
                        "Engrenagem! Aperte Espaço ao lado para coletar material de construção.");
                hints.AddHint(buildBridgeCells[0], 1,
                    "Abismo à frente. Junte as engrenagens e aperte Espaço para erguer a ponte.");
            }
            if (finalBoss)
                hints.AddHint(bossCell, 2,
                    "O Guardião do Coração bloqueia o núcleo! Derrote-o para alcançar os fragmentos.");
            if (capturing)
                hints.AddHint(captureCells[0], 2,
                    "Núcleo corrompido. Fique em cima e aperte Espaço para restaurá-lo.");
            hints.AddHint(goalCell, 1, "Sala restaurada à frente — entre nela.");

            // Painel de introdução (texto derivado da fase)
            if (hud != null)
            {
                string intro;
                if (finalBoss)
                    intro = "Coração Eclipse\n\nVocê alcançou o núcleo central. Derrote o Guardião do Coração e restaure os fragmentos do núcleo (fique em cima e aperte Espaço) para reconstruí-lo e salvar a dungeon.\n\nPressione Enter ou clique para começar.";
                else if (capturing)
                    intro = "Jardins Subterrâneos\n\nA sala está corrompida. Restaure todos os Núcleos de Equilíbrio (fique em cima e aperte Espaço) para abrir a sala dourada.\n\nPressione Enter ou clique para começar.";
                else if (building)
                    intro = "Minas Escuras\n\nO caminho está partido por um abismo. Colete as engrenagens antigas (Espaço) e construa a ponte para atravessar até a sala dourada.\n\nPressione Enter ou clique para começar.";
                else if (advancedCombat)
                    intro = "Fortaleza Perdida\n\nVários guardiões corrompidos bloqueiam o caminho. Ataque com Espaço, avance derrotando um a um e não fique parado ao lado deles. A saída só abre quando todos caírem.\n\nPressione Enter ou clique para começar.";
                else if (spawnGuardian)
                    intro = "Sala do Guardião\n\nDerrote o guardião: aproxime-se e ataque com Espaço. Mas cuidado — ficar parado ao lado dele drena sua vida!\n\nPressione Enter ou clique para começar.";
                else
                    intro = "Prisão Abandonada\n\nUse WASD para mover. Aperte Espaço perto do cristal para destruí-lo e alcance a sala dourada.\n\nPressione Enter ou clique para começar.";
                hud.ShowIntro(intro);
            }

            string rodape;
            if (finalBoss) rodape = "Use WASD para mover. Espaço ataca o chefe e restaura o núcleo.";
            else if (capturing) rodape = "Use WASD para mover. Espaço restaura os núcleos.";
            else if (building) rodape = "Use WASD para mover. Espaço coleta engrenagens e constrói.";
            else if (spawnGuardian || advancedCombat) rodape = "Use WASD para mover. Espaço ataca os guardiões.";
            else rodape = "Use WASD para mover. Espaço destrói cristais.";
            Messages.Raise(rodape);
        }

        private Guardian SpawnGuardianAt(Vector2Int cell, PlayerController player,
            int hp = -1, int counter = -1, bool boss = false)
        {
            var guardianGo = new GameObject(boss ? "Boss" : "Guardian");
            var gdsr = guardianGo.AddComponent<SpriteRenderer>();
            gdsr.sprite = PlaceholderSprite.Square;
            gdsr.color = boss ? new Color(0.55f, 0.08f, 0.15f) : new Color(0.8f, 0.2f, 0.2f);
            gdsr.sortingOrder = 4;
            var guardian = guardianGo.AddComponent<Guardian>();
            guardian.Init(board, cell, player, hp, counter, boss ? 1.15f : 0.75f);

            var gFlash = guardianGo.AddComponent<DamageFlash>();
            gFlash.Bind(guardian.Health);

            var barGo = new GameObject("GuardianHpBar");
            barGo.AddComponent<WorldHealthBar>().Bind(guardian.Health, guardianGo.transform);
            return guardian;
        }

        // True quando todos os guardiões foram derrotados (referência destruída = derrotado).
        private static bool AllDefeated(List<Guardian> guardians)
        {
            foreach (var g in guardians)
                if (g != null && !g.Defeated) return false;
            return true;
        }
    }
}
