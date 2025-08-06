using UnityEngine;

namespace Human_System
{
    public class HumanRender
    {
        private readonly Transform _renderTransform;

        public HumanRender(Transform renderTransform)
        {
            _renderTransform = renderTransform;
        }

        public void UpdateFacing(float directionX)
        {
            if (Mathf.Abs(directionX) < 0.01f) return;

            Vector3 scale = _renderTransform.localScale;
            scale.x = directionX > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            _renderTransform.localScale = scale;
        }
    }
}