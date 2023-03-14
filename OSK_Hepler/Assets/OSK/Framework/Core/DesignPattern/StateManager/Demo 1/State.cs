using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public abstract class State : MonoBehaviour
    {
        public abstract State RunCurrentState();
    }
}
