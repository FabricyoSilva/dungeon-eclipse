using UnityEngine;
using DungeonEclipse.Combat;
using DungeonEclipse.CameraRig;
using DungeonEclipse.Audio;

namespace DungeonEclipse.Effects
{
    /// <summary>
    /// Reage ao dano sofrido por um Health: som de dano + screen shake.
    /// (O flash visual fica a cargo de DamageFlash.) Cobre dano de qualquer
    /// origem — contra-ataque do guardião e dano de proximidade.
    /// </summary>
    public class HurtFeedback : MonoBehaviour
    {
        private Health _health;
        private CameraFollow _camera;
        private int _last;

        public void Bind(Health health, CameraFollow camera)
        {
            _health = health;
            _camera = camera;
            _last = health.Current;
            _health.OnChanged += OnChanged;
        }

        private void OnDestroy()
        {
            if (_health != null) _health.OnChanged -= OnChanged;
        }

        private void OnChanged(int current, int max)
        {
            if (current < _last)
            {
                Sfx.Hurt();
                if (_camera != null) _camera.Shake(0.18f, 0.18f);
            }
            _last = current;
        }
    }
}
