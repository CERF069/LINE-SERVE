namespace Racket_System
{
    using UnityEngine;
    public class RacketRender
    {
        private readonly Transform _renderTransform;
        private readonly float _rotationSpeed;

        private float _targetRotationZ;

        public RacketRender(Transform renderTransform, float rotationSpeed = 10f)
        {
            _renderTransform = renderTransform;
            _rotationSpeed = rotationSpeed;
            _targetRotationZ = 60f;
        }

        public void UpdateRotation(float targetX)
        {
            _targetRotationZ = targetX > 0 ? -60f : 60f;

            float currentZ = _renderTransform.rotation.eulerAngles.z;
            if (currentZ > 180f) currentZ -= 360f;

            float newZ = Mathf.LerpAngle(currentZ, _targetRotationZ, Time.deltaTime * _rotationSpeed);
            _renderTransform.rotation = Quaternion.Euler(0, 0, newZ);
        }
    }
}