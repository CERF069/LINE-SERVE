using UnityEngine;
using Move_System;
using System.Collections;

namespace Human_System
{
    [RequireComponent(typeof(Transform))]
    public class Human : MonoBehaviour, IMoveToTarget
    {
        [Header("Movement")]
        [SerializeField, Min(0.01f)] private float _speed = 2f;
        [SerializeField] private Transform _renderObject;
        [SerializeField] private Transform _ballTarget;

        [Header("Pause Settings")]
        [SerializeField, Min(0.01f)] private float _minPauseInterval = 3f;
        [SerializeField, Min(0.01f)] private float _maxPauseInterval = 10f;
        [SerializeField, Min(0.01f)] private float _pauseDuration = 2f;

        private HumanRender _render;
        private bool _shouldMove = false;
        private bool _isPaused = false;

        private void OnValidate()
        {
            _speed = Mathf.Max(0.01f, _speed);
            _minPauseInterval = Mathf.Max(0.01f, _minPauseInterval);
            _maxPauseInterval = Mathf.Max(0.01f, _maxPauseInterval);
            _pauseDuration = Mathf.Max(0.01f, _pauseDuration);
            
            if (_minPauseInterval > _maxPauseInterval)
            {
                float temp = _minPauseInterval;
                _minPauseInterval = _maxPauseInterval;
                _maxPauseInterval = temp;
            }
            
            if (_pauseDuration > _minPauseInterval)
            {
                _pauseDuration = _minPauseInterval;
            }
        }
        private Vector3 _initialPosition;

        private void Awake()
        {
            _initialPosition = transform.position;
        }

        public void ResetHuman()
        {
            transform.position = _initialPosition;
            _shouldMove = false;
            _isPaused = false;
            StopAllCoroutines();
            StartCoroutine(PauseRoutine());

            if (_ballTarget != null)
                SetTargetBall(_ballTarget);
        }
        private void Start()
        {
            if (_renderObject == null)
            {
                Debug.LogError("[Human] RenderObject не назначен!", this);
                enabled = false;
                return;
            }

            if (_ballTarget == null)
            {
                Debug.LogWarning("[Human] Цель мяча не назначена. Движение не начнётся.");
            }

            _render = new HumanRender(_renderObject);

            if (_ballTarget != null)
                SetTargetBall(_ballTarget);

            StartCoroutine(PauseRoutine());
        }

        private void Update()
        {
            if (_shouldMove && !_isPaused && _ballTarget != null)
            {
                MoveTowardsBallXOnly();
            }
        }

        private IEnumerator PauseRoutine()
        {
            while (true)
            {
                float waitTime = Random.Range(_minPauseInterval, _maxPauseInterval);
                yield return new WaitForSeconds(waitTime);

                _isPaused = true;
                yield return new WaitForSeconds(_pauseDuration);
                _isPaused = false;
            }
        }

        public void SetTargetBall(Transform ball)
        {
            if (ball == null)
            {
                Debug.LogWarning("[Human] Указана пустая цель мяча.", this);
                return;
            }

            _ballTarget = ball;
            _shouldMove = true;
        }

        private void MoveTowardsBallXOnly()
        {
            Vector3 currentPos = transform.position;
            Vector3 targetPos = new Vector3(_ballTarget.position.x, currentPos.y, currentPos.z);
            Vector3 direction = (targetPos - currentPos).normalized;

            transform.position = Vector3.MoveTowards(currentPos, targetPos, _speed * Time.deltaTime);
            _render?.UpdateFacing(direction.x);
        }

        public void MoveToTarget(Vector3 target, float speed)
        {
            if (_render == null) return;

            Vector3 currentPos = transform.position;
            Vector3 direction = (target - currentPos).normalized;

            transform.position = Vector3.MoveTowards(currentPos, target, speed * Time.deltaTime);
            _render.UpdateFacing(direction.x);
        }
    }
}
