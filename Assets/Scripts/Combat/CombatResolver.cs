namespace DungeonEclipse.Combat
{
    /// <summary>
    /// Regra pura de combate corpo-a-corpo, determinística e sem dependência de
    /// cena (testável). O defensor recebe o dano do atacante; se sobreviver,
    /// revida causando o contra-ataque no atacante.
    /// </summary>
    public static class CombatResolver
    {
        /// <summary>
        /// Resolve uma troca de golpes. Retorna true se o defensor morreu.
        /// </summary>
        public static bool ResolveMelee(Health defensor, int danoAtacante,
            Health atacante, int contraAtaque)
        {
            defensor.TakeDamage(danoAtacante);
            if (!defensor.IsDead)
                atacante.TakeDamage(contraAtaque);
            return defensor.IsDead;
        }
    }
}
