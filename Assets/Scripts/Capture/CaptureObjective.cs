using System;

namespace DungeonEclipse.Capture
{
    /// <summary>
    /// Objetivo puro de captura: acompanha quantos núcleos foram restaurados de um
    /// total. Sem dependência de cena — testável em EditMode. Notifica progresso
    /// e conclusão (sala restaurada) via evento.
    /// </summary>
    public class CaptureObjective
    {
        public int Total { get; }
        public int Captured { get; private set; }
        public bool Complete => Captured >= Total;

        /// <summary>Disparado a cada captura com (capturados, total).</summary>
        public event Action<int, int> OnChanged;

        public CaptureObjective(int total)
        {
            Total = total < 0 ? 0 : total;
        }

        public void RegisterCapture()
        {
            if (Complete) return;
            Captured++;
            OnChanged?.Invoke(Captured, Total);
        }
    }
}
