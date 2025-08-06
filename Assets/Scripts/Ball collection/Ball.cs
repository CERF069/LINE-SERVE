using UnityEngine;
using Move_System;
using Zenject;
using Signals;

namespace BallСollection
{
    public class Ball : MonoBehaviour, IMoveToTarget
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _arcHeightFactor = 0.5f;
        [SerializeField] private float _stopDistance = 0.05f;
        [SerializeField] private GameObject _renderBall;
        [SerializeField] private float _minScale = 0.5f;
        [SerializeField] private float _maxScale = 1.2f;
        [SerializeField] private float _minY = -4f;
        [SerializeField] private float _maxY = 4f;

        private BallRender _ballRender;
        private Vector3 _startPosition;
        private Vector3 _midPosition;
        private Vector3 _finalTarget;
        private bool _shouldMove = false;
        private bool _movingToMid = false;
        private bool _movingToFinal = false;
        private float _moveDuration;
        private float _moveTimer = 0f;

        private SignalBus _signalBus;
        private TargetType _targetType;  // Хранит тип цели

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private Vector3 _initialPosition;

        private void Awake()
        {
            _initialPosition = transform.position;
        }

        public void ResetBall()
        {
            transform.position = _initialPosition;
            _shouldMove = false;
            _movingToMid = false;
            _movingToFinal = false;
            _moveTimer = 0f;
            
            TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
            if (trail != null)
            {
                trail.Clear();
            }
        }

        private void Start()
        {
            _ballRender = new BallRender(_renderBall, _minScale, _maxScale, _minY, _maxY);
        }
        private void Update()
        {
            if (_movingToMid)
            {
                _moveTimer += Time.deltaTime;
                float t = Mathf.Clamp01(_moveTimer / _moveDuration);
                transform.position = GetArcPosition(_startPosition, _midPosition, t);

                if (t >= 1f)
                {
                    _movingToMid = false;
                    StartMovingToFinal();
                }
            }
            else if (_movingToFinal)
            {
                _moveTimer += Time.deltaTime;
                float t = Mathf.Clamp01(_moveTimer / _moveDuration);
                transform.position = GetArcPosition(_startPosition, _finalTarget, t);

                if (t >= 1f)
                {
                    _movingToFinal = false;
                    _shouldMove = false;

                    // Отправка сигнала о достижении цели с типом
                    _signalBus.Fire(new BallReachedTargetSignal(_targetType));
                }
            }

            _ballRender?.UpdateScale(transform.position.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TargetLinePoints line = other.GetComponent<TargetLinePoints>();
            if (line != null)
            {
                _midPosition = line.MidPosition;
                _finalTarget = line.GetRandomTarget();

                _targetType = line.targetType;  // Получаем тип цели из TargetLinePoints

                StartMovingToMid();
            }
        }

        private void StartMovingToMid()
        {
            _startPosition = transform.position;
            _moveTimer = 0f;
            _shouldMove = true;
            _movingToMid = true;
            _moveDuration = Vector3.Distance(_startPosition, _midPosition) / _speed;
        }

        private void StartMovingToFinal()
        {
            _startPosition = transform.position;
            _moveTimer = 0f;
            _movingToFinal = true;
            _moveDuration = Vector3.Distance(_startPosition, _finalTarget) / _speed;
        }

        private Vector3 GetArcPosition(Vector3 start, Vector3 end, float t)
        {
            Vector3 linear = Vector3.Lerp(start, end, t);
            float arcHeight = _speed * _arcHeightFactor;
            float arc = Mathf.Sin(t * Mathf.PI) * arcHeight;
            Vector3 direction = end - start;
            Vector3 normal = Vector3.Cross(direction, Vector3.forward).normalized;
            return linear + normal * arc;
        }

        public void MoveToTarget(Vector3 target, float speed)
        {
            Vector2 currentPos = transform.position;
            Vector2 targetPos = target;
            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }
}
