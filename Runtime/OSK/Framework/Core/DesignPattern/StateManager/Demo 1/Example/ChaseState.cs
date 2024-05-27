using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OSK
{
    public class ChaseState : State
    {
        public AttackState attackState;
        public bool ableAttack = false;

        public override State RunCurrentState()
        {
            if (ableAttack)
            {
                return attackState;
            }
            else
            {
                return this;
            }
        }
    }
}