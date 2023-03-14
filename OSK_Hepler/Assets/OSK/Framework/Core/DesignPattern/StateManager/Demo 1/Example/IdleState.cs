using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OSK
{
    public class IdleState : State
    {
        public ChaseState chaseState;
        public bool isChase;
        public override State RunCurrentState()
        {
            if (isChase)
            {
                return chaseState;
            }
            else
            {
                return this;
            }
        }
    }
}
