using UnityEngine;

namespace DungeonEclipse.CameraRig
{
    /// <summary>Câmera top-down que segue suavemente um alvo no plano XY,
    /// com um tremor curto opcional (screen shake).</summary>
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float smooth = 8f;
        private Transform _target;
        private float _shakeTime;
        private float _shakeMag;

        public void SetTarget(Transform t) => _target = t;

        /// <summary>Dispara um tremor de câmera por <paramref name="duration"/> segundos.</summary>
        public void Shake(float magnitude = 0.18f, float duration = 0.15f)
        {
            _shakeMag = magnitude;
            _shakeTime = duration;
        }

        private void LateUpdate()
        {
            if (_target == null) return;
            Vector3 goal = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            Vector3 pos = Vector3.Lerp(
                transform.position, goal, 1f - Mathf.Exp(-smooth * Time.deltaTime));

            if (_shakeTime > 0f)
            {
                _shakeTime -= Time.deltaTime;
                Vector2 off = Random.insideUnitCircle * _shakeMag;
                pos.x += off.x;
                pos.y += off.y;
            }
            transform.position = pos;
        }
    }
}
