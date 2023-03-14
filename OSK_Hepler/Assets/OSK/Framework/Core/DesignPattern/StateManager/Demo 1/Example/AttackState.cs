using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OSK
{
    public class AttackState : State
    {
        public IdleState idleState;
        public bool isIde = false;
        public override State RunCurrentState()
        {
            if (isIde)
            {
                return idleState;
            }
            else
            {
                return this;
            }
        }
    }
}