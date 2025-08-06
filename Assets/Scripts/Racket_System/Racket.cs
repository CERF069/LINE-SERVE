using UnityEngine;
using Racket_System;
using Zenject;
using Move_System;

public class Racket : MonoBehaviour, IMoveToTarget
{
    [SerializeField] private Transform _racketVisual;

    private PlayerInputProvider _inputProvider;
    private RacketRender _render;

    [Inject]
    public void Construct(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    private void Start()
    {
        _render = new RacketRender(_racketVisual);
    }

    private void Update()
    {
        float? targetX = _inputProvider.GetTargetX();
        if (targetX.HasValue)
        {
            Vector3 targetPosition = new Vector3(targetX.Value, transform.position.y, transform.position.z);
            MoveToTarget(targetPosition, 100f);
            _render.UpdateRotation(targetX.Value);
        }
    }
    
    public void MoveToTarget(Vector3 target, float speed)
    {
        Vector3 currentPosition = transform.position;
        target.y = currentPosition.y;
        target.z = currentPosition.z;

        transform.position = Vector3.MoveTowards(currentPosition, target, speed * Time.deltaTime);
    }
}
