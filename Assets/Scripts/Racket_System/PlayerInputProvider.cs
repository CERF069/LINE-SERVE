using Unity.VisualScripting;

namespace Racket_System
{
    using UnityEngine;
    public abstract class PlayerInputProvider
    {
        public abstract float? GetTargetX();
    }
    
        public class ScreenClickInputProvider : PlayerInputProvider, IInitializable
        {
            Camera _mainCamera;

            public void Initialize()
            {
                _mainCamera = Camera.main;

                if (_mainCamera == null)
                {
                    Debug.LogWarning("Main camera not found in Initialize(). Will try to get it in runtime.");
                }
            }

            public override float? GetTargetX()
            {
                if (_mainCamera == null)
                    _mainCamera = Camera.main;

                if (_mainCamera == null)
                    return null;

                // Вместо GetMouseButtonDown(0) используем GetMouseButton(0)
                if (Input.GetMouseButton(0))
                {
                    Vector3 screenPosition = Input.mousePosition;
                    Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(screenPosition);
                    return worldPosition.x;
                }

                return null;
            }
        }
    }
