using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class StateManager : MonoBehaviour
    {
        [SerializeField] private State currentState;

        private void Update()
        {
            RunCurrentState();
        }

        public void RunCurrentState()
        {
            State nextState = currentState?.RunCurrentState();
            if (nextState != null)
            {
                SwitchState(nextState);
            }
        }

        public void SwitchState(State newState)
        {
            currentState = newState;
        }
    }
}
