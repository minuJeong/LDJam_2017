using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class CivilanPawn : EnemyPawn
{
    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        DefaultControllerHitHandler(hit);
        OnControllerHitPawn(hit);
        OnControllerHitWall(hit);
    }
}
