using UnityEngine;

namespace DungeonEclipse.CameraRig
{
    /// <summary>Câmera top-down que segue suavemente um alvo no plano XY.</summary>
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float smooth = 8f;
        private Transform _target;

        public void SetTarget(Transform t) => _target = t;

        private void LateUpdate()
        {
            if (_target == null) return;
            Vector3 goal = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(
                transform.position, goal, 1f - Mathf.Exp(-smooth * Time.deltaTime));
        }
    }
}
