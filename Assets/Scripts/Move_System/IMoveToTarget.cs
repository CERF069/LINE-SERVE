using UnityEngine;

namespace Move_System
{
    public interface IMoveToTarget
    {
        void MoveToTarget(Vector3 target, float speed);
    }
}