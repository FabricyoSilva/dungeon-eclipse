using NUnit.Framework;
using DungeonEclipse.Building;

public class BuildInventoryTests
{
    [Test]
    public void Add_AumentaMateriais()
    {
        var inv = new BuildInventory();
        inv.Add(2);
        Assert.AreEqual(2, inv.Materials);
    }

    [Test]
    public void Add_IgnoraValorNaoPositivo()
    {
        var inv = new BuildInventory();
        inv.Add(0);
        inv.Add(-3);
        Assert.AreEqual(0, inv.Materials);
    }

    [Test]
    public void Spend_ComSaldo_Subtrai()
    {
        var inv = new BuildInventory();
        inv.Add(3);
        Assert.IsTrue(inv.Spend(2));
        Assert.AreEqual(1, inv.Materials);
    }

    [Test]
    public void Spend_SemSaldo_Falha()
    {
        var inv = new BuildInventory();
        inv.Add(1);
        Assert.IsFalse(inv.Spend(2));
        Assert.AreEqual(1, inv.Materials);
    }

    [Test]
    public void OnChanged_DisparaAoMudar()
    {
        var inv = new BuildInventory();
        int last = -1;
        inv.OnChanged += v => last = v;
        inv.Add(5);
        Assert.AreEqual(5, last);
    }
}
