using NUnit.Framework;
using DungeonEclipse.Combat;

public class CombatResolverTests
{
    [Test]
    public void Resolve_DefensorMorre_SemContraAtaque()
    {
        var defensor = new Health(1);
        var atacante = new Health(5);
        bool morreu = CombatResolver.ResolveMelee(defensor, 1, atacante, 1);
        Assert.IsTrue(morreu);
        Assert.IsTrue(defensor.IsDead);
        Assert.AreEqual(5, atacante.Current); // nao tomou contra-ataque
    }

    [Test]
    public void Resolve_DefensorSobrevive_AtacanteRecebeContra()
    {
        var defensor = new Health(3);
        var atacante = new Health(5);
        bool morreu = CombatResolver.ResolveMelee(defensor, 1, atacante, 1);
        Assert.IsFalse(morreu);
        Assert.AreEqual(2, defensor.Current);
        Assert.AreEqual(4, atacante.Current); // tomou o contra-ataque
    }

    [Test]
    public void Resolve_ContraAtaquePodeMatarAtacante()
    {
        var defensor = new Health(3);
        var atacante = new Health(1);
        bool morreu = CombatResolver.ResolveMelee(defensor, 1, atacante, 1);
        Assert.IsFalse(morreu);            // defensor sobreviveu
        Assert.IsTrue(atacante.IsDead);    // atacante morreu no contra-ataque
    }
}
