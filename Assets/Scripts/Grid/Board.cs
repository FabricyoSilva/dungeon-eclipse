using UnityEngine;
using DungeonEclipse.Core;

namespace DungeonEclipse.Grid
{
    /// <summary>
    /// Componente de cena que constrói e renderiza a grade placeholder a partir
    /// de um layout em texto, e mantém o BoardGrid (lógica pura).
    /// </summary>
    public class Board : MonoBehaviour
    {
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private Color floorColor = new Color(0.45f, 0.55f, 0.72f);
        [SerializeField] private Color wallColor = new Color(0.18f, 0.2f, 0.26f);

        // '#' parede, '.' chão. Linha 0 = topo (y mais alto).
        [SerializeField] private string[] layout =
        {
            "#########", // y6
            "#######.#", // y5  (sala-alvo em x=7)
            "#######.#", // y4
            "#######.#", // y3
            "#######.#", // y2
            "#.......#", // y1  (corredor: start x=1, cristal x=4, sobe em x=7)
            "#########", // y0
        };

        public BoardGrid Grid { get; private set; }

        private void Awake() => Build();

        public void Build()
        {
            int height = layout.Length;
            int width = layout[0].Length;
            Grid = new BoardGrid(width, height, cellSize, Vector2.zero);

            for (int row = 0; row < height; row++)
            {
                int y = height - 1 - row; // linha 0 do texto fica no topo
                string line = layout[row];
                for (int x = 0; x < width; x++)
                {
                    bool wall = x < line.Length && line[x] == '#';
                    Grid.SetWalkable(x, y, !wall);
                    SpawnTile(x, y, wall ? wallColor : floorColor,
                        wall ? "Wall" : "Floor", wall ? 1 : 0);
                }
            }
        }

        private void SpawnTile(int x, int y, Color color, string label, int sortingOrder)
        {
            var go = new GameObject($"{label}_{x}_{y}");
            go.transform.SetParent(transform);
            go.transform.position = Grid.CellToWorld(x, y);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = PlaceholderSprite.Square;
            sr.color = color;
            sr.sortingOrder = sortingOrder;
        }
    }
}
