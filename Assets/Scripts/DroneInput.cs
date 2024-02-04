using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DroneControl
{
    [RequireComponent(typeof(PlayerInput))]
    public class DroneInput : MonoBehaviour
    {
        #region Variables
        private Vector2 leftStick;
        private Vector2 rightStick;

        public Vector2 LeftStick { get => leftStick; }
        public Vector2 RightStick { get => rightStick; }
        #endregion

        #region Main Methods
        void Update()
        {
        
        }
        #endregion

        #region Input Methods
        private void OnLeftStick(InputValue value)
        {
            leftStick = value.Get<Vector2>();
        }

        private void OnRightStick(InputValue value)
        {
            rightStick = value.Get<Vector2>();
        }
        #endregion
    }
}
