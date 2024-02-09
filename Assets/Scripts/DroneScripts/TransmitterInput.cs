using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DroneControl
{
    [RequireComponent(typeof(PlayerInput))]
    public class TransmitterInput : MonoBehaviour
    {
        #region Variables
        private Vector2 leftStick = new Vector2(0f, 0f);
        private Vector2 rightStick = new Vector2(0f, 0f);

        public Vector2 LeftStick { get => leftStick; }
        public Vector2 RightStick { get => rightStick; }
        #endregion

        #region Main Methods
        void Update()
        {

        }
        #endregion

        #region Input Methods
        private void OnRx(InputValue value)
        {
            leftStick = new Vector2((-1) * value.Get<float>(), leftStick.y);
        }

        private void OnRy(InputValue value)
        {
            
        }

        private void OnRz(InputValue value)
        {
            
        }

        private void OnStickX(InputValue value)
        {
            rightStick = new Vector2(value.Get<float>(), rightStick.y);
        }

        private void OnStickY(InputValue value)
        {
            rightStick = new Vector2(rightStick.x, (-1) * value.Get<float>());
        }

        private void OnZ(InputValue value)
        {
            leftStick = new Vector2(leftStick.x, value.Get<float>());
        }
        #endregion
    }
}
