using UnityEngine;

namespace DungeonEclipse.Combat
{
    /// <summary>
    /// Regra pura: decide se o guardião deve aplicar dano de proximidade neste
    /// tick. Verdadeiro quando o jogador está ortogonalmente adjacente (distância
    /// Manhattan = 1) e os estados permitem. Sem dependência de cena (testável).
    /// </summary>
    public static class ProximityRule
    {
        public static bool ShouldDamage(Vector2Int guardianCell, Vector2Int playerCell,
            bool defeated, bool jogando)
        {
            if (defeated || !jogando) return false;
            int dist = Mathf.Abs(guardianCell.x - playerCell.x)
                     + Mathf.Abs(guardianCell.y - playerCell.y);
            return dist == 1;
        }
    }
}
