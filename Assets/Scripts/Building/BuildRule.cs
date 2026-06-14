namespace DungeonEclipse.Building
{
    /// <summary>
    /// Regra pura: decide se um mecanismo (ponte/plataforma) pode ser construído
    /// agora. Verdadeiro quando ainda não foi construído e há materiais
    /// suficientes. Sem dependência de cena (testável).
    /// </summary>
    public static class BuildRule
    {
        public static bool CanBuild(int collected, int required, bool built)
        {
            if (built) return false;
            return collected >= required;
        }
    }
}
