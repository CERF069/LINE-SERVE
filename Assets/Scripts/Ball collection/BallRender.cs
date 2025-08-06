using UnityEngine;

namespace BallСollection
{
    public class BallRender
    {
        private GameObject _renderBall;
        private float _minScale;
        private float _maxScale;
        private float _minY;
        private float _maxY;

        public BallRender(GameObject renderBall, float minScale, float maxScale, float minY, float maxY)
        {
            _renderBall = renderBall;
            _minScale = minScale;
            _maxScale = maxScale;
            _minY = minY;
            _maxY = maxY;
        }

        public void UpdateScale(float currentY)
        {
            float t = Mathf.InverseLerp(_maxY, _minY, currentY);
            float scale = Mathf.Lerp(_minScale, _maxScale, t);
            _renderBall.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}