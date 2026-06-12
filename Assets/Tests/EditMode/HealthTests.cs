using NUnit.Framework;
using DungeonEclipse.Combat;

public class HealthTests
{
    [Test]
    public void New_StartsAtMax()
    {
        var h = new Health(5);
        Assert.AreEqual(5, h.Current);
        Assert.IsFalse(h.IsDead);
    }

    [Test]
    public void TakeDamage_ReducesCurrent()
    {
        var h = new Health(5);
        h.TakeDamage(2);
        Assert.AreEqual(3, h.Current);
    }

    [Test]
    public void TakeDamage_ClampsAtZero_AndDies()
    {
        var h = new Health(5);
        bool died = false;
        h.OnDied += () => died = true;
        h.TakeDamage(10);
        Assert.AreEqual(0, h.Current);
        Assert.IsTrue(h.IsDead);
        Assert.IsTrue(died);
    }

    [Test]
    public void TakeDamage_WhenDead_DoesNothing()
    {
        var h = new Health(5);
        h.TakeDamage(10);
        int deaths = 0;
        h.OnDied += () => deaths++;
        h.TakeDamage(1);
        Assert.AreEqual(0, deaths);
    }
}
