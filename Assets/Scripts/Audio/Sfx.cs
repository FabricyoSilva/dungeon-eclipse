using UnityEngine;

namespace DungeonEclipse.Audio
{
    /// <summary>
    /// SFX sintetizados em runtime (sem assets). Um AudioSource oculto é criado
    /// sob demanda e persiste entre cenas. Trocável por AudioClips reais depois
    /// sem mudar os call sites.
    /// </summary>
    public static class Sfx
    {
        private static AudioSource _source;

        private static AudioSource Source
        {
            get
            {
                if (_source == null)
                {
                    var go = new GameObject("SfxSource");
                    Object.DontDestroyOnLoad(go);
                    go.hideFlags = HideFlags.HideAndDontSave;
                    _source = go.AddComponent<AudioSource>();
                    _source.playOnAwake = false;
                }
                return _source;
            }
        }

        public static void Move()    => Play(220f, 0.06f, 0.20f);
        public static void Destroy() => Play(440f, 0.18f, 0.40f, sweep: -220f);
        public static void Attack()  => Play(330f, 0.10f, 0.40f, square: true);
        public static void Hurt()    => Play(160f, 0.20f, 0.50f, sweep: -60f);
        public static void Victory() => Play(523f, 0.40f, 0.40f, sweep: 264f);
        public static void Defeat()  => Play(196f, 0.50f, 0.45f, sweep: -90f);

        private static void Play(float freq, float dur, float volume,
            float sweep = 0f, bool square = false)
        {
            Source.PlayOneShot(Tone(freq, dur, sweep, square), volume);
        }

        private static AudioClip Tone(float freq, float dur, float sweep, bool square)
        {
            const int rate = 44100;
            int samples = Mathf.Max(1, (int)(rate * dur));
            var data = new float[samples];
            double phase = 0;
            for (int i = 0; i < samples; i++)
            {
                float t = (float)i / samples;
                float f = freq + sweep * t;
                phase += 2.0 * Mathf.PI * f / rate;
                float s = square ? (Mathf.Sin((float)phase) >= 0 ? 1f : -1f)
                                 : Mathf.Sin((float)phase);
                float env = Mathf.Min(1f, (1f - t) * 4f); // decai no fim
                data[i] = s * env;
            }
            var clip = AudioClip.Create("tone", samples, 1, rate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
