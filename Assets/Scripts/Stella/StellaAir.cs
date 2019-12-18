using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreeningEx2019
{
    [CreateAssetMenu(menuName = "Greening/Stella Actions/Create Air", fileName = "StellaActionAir")]
    public class StellaAir : StellaActionScriptableObject
    {
        public override void UpdateAction(float tick)
        {
            StellaMove.instance.Gravity();
            StellaMove.instance.Move();

            if (StellaMove.chrController.isGrounded)
            {
                StellaMove.instance.SetAnimState(StellaMove.AnimType.Walk);
                StellaMove.instance.ChangeAction(StellaMove.ActionType.Walk);
            }
        }
    }
}
