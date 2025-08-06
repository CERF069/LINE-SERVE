using UnityEngine;

namespace BallСollection
{
    public class TargetLinePoints : MonoBehaviour
    {
        public TargetType targetType;

        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private int numberOfPoints = 5;
        
        [SerializeField] private Transform _midPosition;
        public Vector3 MidPosition => _midPosition.position;

        private Vector3[] _linePoints;

        private void Awake()
        {
            GenerateLinePoints();
        }

        private void GenerateLinePoints()
        {
            _linePoints = new Vector3[numberOfPoints];
            for (int i = 0; i < numberOfPoints; i++)
            {
                float t = (float)i / (numberOfPoints - 1);
                _linePoints[i] = Vector3.Lerp(pointA.position, pointB.position, t);
            }
        }

        public Vector3 GetRandomTarget()
        {
            if (_linePoints == null || _linePoints.Length == 0)
                return transform.position;

            int index = Random.Range(0, _linePoints.Length);
            return _linePoints[index];
        }
    }
}