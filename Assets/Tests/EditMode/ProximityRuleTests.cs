using NUnit.Framework;
using UnityEngine;
using DungeonEclipse.Combat;

public class ProximityRuleTests
{
    [Test]
    public void Adjacente_Jogando_AplicaDano()
    {
        Assert.IsTrue(ProximityRule.ShouldDamage(
            new Vector2Int(7, 3), new Vector2Int(7, 4), false, true));
    }

    [Test]
    public void Distante_NaoAplica()
    {
        Assert.IsFalse(ProximityRule.ShouldDamage(
            new Vector2Int(7, 3), new Vector2Int(7, 5), false, true));
    }

    [Test]
    public void MesmaCelula_NaoAplica()
    {
        Assert.IsFalse(ProximityRule.ShouldDamage(
            new Vector2Int(7, 3), new Vector2Int(7, 3), false, true));
    }

    [Test]
    public void Diagonal_NaoAplica()
    {
        Assert.IsFalse(ProximityRule.ShouldDamage(
            new Vector2Int(7, 3), new Vector2Int(8, 4), false, true));
    }

    [Test]
    public void Derrotado_NaoAplica()
    {
        Assert.IsFalse(ProximityRule.ShouldDamage(
            new Vector2Int(7, 3), new Vector2Int(7, 4), true, true));
    }

    [Test]
    public void NaoJogando_NaoAplica()
    {
        Assert.IsFalse(ProximityRule.ShouldDamage(
            new Vector2Int(7, 3), new Vector2Int(7, 4), false, false));
    }
}
