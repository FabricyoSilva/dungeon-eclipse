using System;

namespace DungeonEclipse.Combat
{
    /// <summary>
    /// Vida pura (sem MonoBehaviour) — testável. Reusável para Kael e futuros
    /// inimigos/guardião.
    /// </summary>
    [Serializable]
    public class Health
    {
        public int Max { get; private set; }
        public int Current { get; private set; }
        public bool IsDead => Current <= 0;

        public event Action OnDied;
        public event Action<int, int> OnChanged; // (current, max)

        public Health(int max)
        {
            Max = max;
            Current = max;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || IsDead) return;
            Current = Math.Max(0, Current - amount);
            OnChanged?.Invoke(Current, Max);
            if (IsDead) OnDied?.Invoke();
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || IsDead) return;
            Current = Math.Min(Max, Current + amount);
            OnChanged?.Invoke(Current, Max);
        }
    }
}
