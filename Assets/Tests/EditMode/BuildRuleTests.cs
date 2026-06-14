using NUnit.Framework;
using DungeonEclipse.Building;

public class BuildRuleTests
{
    [Test]
    public void MateriaisSuficientes_PodeConstruir()
    {
        Assert.IsTrue(BuildRule.CanBuild(3, 3, false));
    }

    [Test]
    public void MateriaisDeSobra_PodeConstruir()
    {
        Assert.IsTrue(BuildRule.CanBuild(5, 3, false));
    }

    [Test]
    public void MateriaisInsuficientes_NaoPode()
    {
        Assert.IsFalse(BuildRule.CanBuild(2, 3, false));
    }

    [Test]
    public void JaConstruido_NaoPode()
    {
        Assert.IsFalse(BuildRule.CanBuild(5, 3, true));
    }

    [Test]
    public void SemCusto_SemprePode_SeNaoConstruido()
    {
        Assert.IsTrue(BuildRule.CanBuild(0, 0, false));
    }
}
