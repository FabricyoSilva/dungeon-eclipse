using NUnit.Framework;
using DungeonEclipse.Capture;

public class CaptureObjectiveTests
{
    [Test]
    public void Novo_NaoEstaCompleto()
    {
        var obj = new CaptureObjective(3);
        Assert.IsFalse(obj.Complete);
        Assert.AreEqual(0, obj.Captured);
    }

    [Test]
    public void CapturarTodos_Completa()
    {
        var obj = new CaptureObjective(2);
        obj.RegisterCapture();
        Assert.IsFalse(obj.Complete);
        obj.RegisterCapture();
        Assert.IsTrue(obj.Complete);
        Assert.AreEqual(2, obj.Captured);
    }

    [Test]
    public void NaoPassaDoTotal()
    {
        var obj = new CaptureObjective(1);
        obj.RegisterCapture();
        obj.RegisterCapture();
        Assert.AreEqual(1, obj.Captured);
        Assert.IsTrue(obj.Complete);
    }

    [Test]
    public void TotalZero_JaCompleto()
    {
        var obj = new CaptureObjective(0);
        Assert.IsTrue(obj.Complete);
    }

    [Test]
    public void OnChanged_ReportaProgresso()
    {
        var obj = new CaptureObjective(2);
        int captured = -1, total = -1;
        obj.OnChanged += (c, t) => { captured = c; total = t; };
        obj.RegisterCapture();
        Assert.AreEqual(1, captured);
        Assert.AreEqual(2, total);
    }
}
