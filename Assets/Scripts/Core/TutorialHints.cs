using System.Collections.Generic;
using UnityEngine;
using DungeonEclipse.Player;

namespace DungeonEclipse.Core
{
    /// <summary>
    /// Dispara dicas (via Messages) uma única vez quando o jogador chega perto
    /// de pontos de interesse configurados (distância Manhattan &lt;= raio).
    /// </summary>
    public class TutorialHints : MonoBehaviour
    {
        private struct Hint
        {
            public Vector2Int cell;
            public int radius;
            public string text;
            public bool fired;
        }

        private PlayerController _player;
        private readonly List<Hint> _hints = new List<Hint>();

        public void Init(PlayerController player) => _player = player;

        public void AddHint(Vector2Int cell, int radius, string text)
        {
            _hints.Add(new Hint { cell = cell, radius = radius, text = text, fired = false });
        }

        private void Update()
        {
            if (_player == null) return;
            var p = _player.Mover.Cell;
            for (int i = 0; i < _hints.Count; i++)
            {
                var h = _hints[i];
                if (h.fired) continue;
                int dist = Mathf.Abs(p.x - h.cell.x) + Mathf.Abs(p.y - h.cell.y);
                if (dist <= h.radius)
                {
                    Messages.Raise(h.text);
                    h.fired = true;
                    _hints[i] = h;
                }
            }
        }
    }
}
