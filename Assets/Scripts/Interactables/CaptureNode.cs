using UnityEngine;
using DungeonEclipse.Grid;
using DungeonEclipse.Capture;

namespace DungeonEclipse.Interactables
{
    /// <summary>
    /// Núcleo de Equilíbrio corrompido sobre uma célula andável. Não bloqueia: o
    /// Kael fica em cima (ou ao lado) e o restaura (captura) com a ação. Ao ser
    /// capturado, muda de corrompido para restaurado e avisa o objetivo da sala.
    /// </summary>
    public class CaptureNode : MonoBehaviour
    {
        private static readonly Color Corrupted = new Color(0.5f, 0.15f, 0.45f);
        private static readonly Color Restored = new Color(0.3f, 0.9f, 0.6f);

        private CaptureObjective _objective;
        private SpriteRenderer _sr;

        public Vector2Int Cell { get; private set; }
        public bool Captured { get; private set; }

        public void Init(Board board, Vector2Int cell, CaptureObjective objective)
        {
            _objective = objective;
            Cell = cell;
            transform.position = board.Grid.CellToWorld(cell.x, cell.y);
            _sr = GetComponent<SpriteRenderer>();
            if (_sr != null) _sr.color = Corrupted;
        }

        /// <summary>Restaura o núcleo. Retorna true se capturou agora.</summary>
        public bool Capture()
        {
            if (Captured) return false;
            Captured = true;
            if (_sr != null) _sr.color = Restored;
            _objective?.RegisterCapture();
            return true;
        }

        private void Update()
        {
            // corrompido pulsa inquieto; restaurado fica estável
            float s = Captured ? 0.7f : (0.6f + 0.08f * Mathf.Sin(Time.time * 3f));
            transform.localScale = new Vector3(s, s, 1f);
        }
    }
}
