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

        private Dictionary<string, float> rawAxis = new Dictionary<string, float>() { { "Rx", 0 }, { "Ry", 0 }, { "Rz", 0 }, { "StickX", 0 }, { "StickY", 0 }, { "Z", 0 } };

        public Vector2 LeftStick { get => leftStick; }
        public Vector2 RightStick { get => rightStick; }

        private Dictionary<string, string> axisMap = new Dictionary<string, string>()
        {
            { "StickX", "RightStickX" },
            { "StickY", "RightStickY" },
            { "Rx", "LeftStickX" },
            { "Z", "LeftStickY" }
        };
        private Dictionary<string, bool> axisReverse = new Dictionary<string, bool>()
        {
            { "RightStickX", false },
            { "RightStickY", true },
            { "LeftStickX", true },
            { "LeftStickY", false }
        };

        private bool isCallibration = false;
        private string axisCallibration = "";
        private bool waitCallibration = false;
        private bool waitCentering = false;
        private List<string> names = new List<string>() { "Rx", "Ry", "Rz", "StickX", "StickY", "Z" };
        private Dictionary<string, float> axisMin = new Dictionary<string, float>();
        private Dictionary<string, float> axisMax = new Dictionary<string, float>();
        private List<string> stickAxis = new List<string>();
        private Dictionary<string, string> newAxisMap = new Dictionary<string, string>();
        private Dictionary<string, bool> newAxisReverse = new Dictionary<string, bool>();
        #endregion

        #region Main Methods
        private void HandleAxisInput(InputValue value, string inputName)
        {
            switch (inputName)
            {
                case "Rx":
                    rawAxis["Rx"] = value.Get<float>();
                    break;
                case "Ry":
                    rawAxis["Ry"] = value.Get<float>();
                    break;
                case "Rz":
                    rawAxis["Rz"] = value.Get<float>();
                    break;
                case "StickX":
                    rawAxis["StickX"] = value.Get<float>();
                    break;
                case "StickY":
                    rawAxis["StickY"] = value.Get<float>();
                    break;
                case "Z":
                    rawAxis["Z"] = value.Get<float>();
                    break;
            }

            if (!isCallibration)
            {
                if (axisMap.TryGetValue(inputName, out string axisName))
                {
                    switch (axisName)
                    {
                        case "RightStickX":
                            if (axisReverse["RightStickX"])
                            {
                                rightStick = new Vector2((-1) * value.Get<float>(), rightStick.y);
                            }
                            else
                            {
                                rightStick = new Vector2(value.Get<float>(), rightStick.y);
                            }
                            break;
                        case "RightStickY":
                            if (axisReverse["RightStickY"])
                            {
                                rightStick = new Vector2(rightStick.x, (-1) * value.Get<float>());
                            }
                            else
                            {
                                rightStick = new Vector2(rightStick.x, value.Get<float>());
                            }
                            break;
                        case "LeftStickX":
                            // ToDo
                            if (!axisReverse["LeftStickX"])  // idk why
                            {
                                leftStick = new Vector2((-1) * value.Get<float>(), leftStick.y);
                            }
                            else
                            {
                                leftStick = new Vector2(value.Get<float>(), leftStick.y);
                            }
                            break;
                        case "LeftStickY":
                            if (axisReverse["LeftStickY"])
                            {
                                leftStick = new Vector2(leftStick.x, (-1) * value.Get<float>());
                            }
                            else
                            {
                                leftStick = new Vector2(leftStick.x, value.Get<float>());
                            }
                            break;
                    }
                }
            }
        }

        public void CallibrateJoystick()
        {
            Debug.Log("Start joystick callibration");
            isCallibration = true;
            axisCallibration = "RotateSticks";
            newAxisMap = new Dictionary<string, string>();
            newAxisReverse = new Dictionary<string, bool>();
    }

        private void Update()
        {
            if (axisCallibration == "RotateSticks")
            {
                if (!waitCallibration)
                {
                    Debug.Log("Rotate sticks");
                    axisMin = new Dictionary<string, float>()
                    {
                        { "Rx", 0},
                        { "Ry", 0},
                        { "Rz", 0},
                        { "StickX", 0},
                        { "StickY", 0},
                        { "Z", 0}
                    };
                    axisMax = new Dictionary<string, float>()
                    {
                        { "Rx", 0},
                        { "Ry", 0},
                        { "Rz", 0},
                        { "StickX", 0},
                        { "StickY", 0},
                        { "Z", 0}
                    };
                    waitCallibration = true;
                }

                for (int i = 0; i < 6; i++)
                {
                    if (rawAxis[names[i]] < axisMin[names[i]])
                    {
                        axisMin[names[i]] = rawAxis[names[i]];
                    }
                    if (rawAxis[names[i]] > axisMax[names[i]])
                    {
                        axisMax[names[i]] = rawAxis[names[i]];
                    }
                }

                List<string> axis = new List<string>();
                for (int i = 0; i < 6; i++)
                {
                    if (axisMin[names[i]] < -0.9f & axisMax[names[i]] > 0.9f)
                    {
                        axis.Add(names[i]);
                    }
                }

                if (axis.Count >= 4)
                {
                    Debug.Log("Done");
                    foreach (string name in axis)
                    {
                        Debug.Log(name);
                    }
                    axisCallibration = "RightStickX";
                    waitCallibration = false;
                    stickAxis = axis;
                    waitCentering = true;
                }
            }

            if (axisCallibration == "RightStickX")
            {
                CallibrateAxis("RightStickX", "RightStickY");
            }

            if (axisCallibration == "RightStickY")
            {
                CallibrateAxis("RightStickY", "LeftStickX");
            }

            if (axisCallibration == "LeftStickX")
            {
                CallibrateAxis("LeftStickX", "LeftStickY");
            }

            if (axisCallibration == "LeftStickY")
            {
                CallibrateAxis("LeftStickY", "");
            }

            if (axisCallibration == "" & isCallibration)
            {
                isCallibration = false;
                axisMap = newAxisMap;
                axisReverse = newAxisReverse;
            }
        }

        private void CallibrateAxis(string curAxis, string nextAxis)
        {
            if (waitCentering)
            {
                if (!waitCallibration)
                {
                    Debug.Log("Center sticks");
                    waitCallibration = true;
                }

                var success = true;
                foreach (string name in stickAxis)
                {
                    if (rawAxis[name] < -0.1f | rawAxis[name] > 0.1f)
                    {
                        success = false;
                    }
                }

                if (success)
                {
                    Debug.Log("Done centering");
                    waitCentering = false;
                    waitCallibration = false;
                }
            }
            else
            {
                if (!waitCallibration)
                {
                    Debug.Log(curAxis + " up");
                    waitCallibration = true;
                }
                else
                {
                    var success = false;

                    foreach (string name in stickAxis)
                    {
                        if (rawAxis[name] > 0.9f)
                        {
                            newAxisMap[name] = curAxis;
                            newAxisReverse[curAxis] = false;
                            success = true;
                            break;
                        }
                        else if (rawAxis[name] < -0.9f)
                        {
                            newAxisMap[name] = curAxis;
                            newAxisReverse[curAxis] = true;
                            success = true;
                            break;
                        }
                    }

                    if (success)
                    {
                        Debug.Log(curAxis + " is set");
                        axisCallibration = nextAxis;
                        waitCallibration = false;
                        waitCentering = true;
                    }
                }
            }
        }
        #endregion

        #region Input Methods
        private void OnRx(InputValue value)
        {
            HandleAxisInput(value, "Rx");
        }

        private void OnRy(InputValue value)
        {
            HandleAxisInput(value, "Ry");
        }

        private void OnRz(InputValue value)
        {
            HandleAxisInput(value, "Rz");
        }

        private void OnStickX(InputValue value)
        {
            HandleAxisInput(value, "StickX");
        }

        private void OnStickY(InputValue value)
        {
            HandleAxisInput(value, "StickY");
        }

        private void OnZ(InputValue value)
        {
            HandleAxisInput(value, "Z");
        }
        #endregion
    }
}
