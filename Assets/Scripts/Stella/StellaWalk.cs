using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreeningEx2019
{
    [CreateAssetMenu(menuName = "Greening/Stella Actions/Create Walk", fileName = "StellaActionWalk")]
    public class StellaWalk : StellaActionScriptableObject
    {
        public override void UpdateAction(float tick)
        {
            StellaMove.instance.Walk();
            StellaMove.instance.Gravity();
            StellaMove.instance.Move();

            if (!StellaMove.chrController.isGrounded)
            {
                StellaMove.instance.SetAnimState(StellaMove.AnimType.Jump);
                StellaMove.instance.ChangeAction(StellaMove.ActionType.Air);
                StellaMove.myVelocity.x = 0f;
            }
        }
    }
}
