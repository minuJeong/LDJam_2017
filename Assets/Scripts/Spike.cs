using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Trap
{
    public override void OnHit(Pawn pawn)
    {
        pawn.Die();
    }
}
