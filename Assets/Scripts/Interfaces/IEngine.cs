using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneControl
{
    public interface IEngine
    {
        void InitEngine();
        void UpdateEngine(Rigidbody rb, TransmitterInput input);
    }
}
