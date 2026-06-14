using System;

namespace DungeonEclipse.Building
{
    /// <summary>
    /// Inventário puro de materiais de construção (engrenagens) coletados no
    /// andar. Sem dependência de cena — testável em EditMode. Notifica mudanças
    /// via evento para a HUD/feedback.
    /// </summary>
    public class BuildInventory
    {
        public int Materials { get; private set; }
        public event Action<int> OnChanged;

        public void Add(int amount)
        {
            if (amount <= 0) return;
            Materials += amount;
            OnChanged?.Invoke(Materials);
        }

        public bool Spend(int amount)
        {
            if (amount < 0 || Materials < amount) return false;
            Materials -= amount;
            OnChanged?.Invoke(Materials);
            return true;
        }
    }
}
