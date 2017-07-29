using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerPawn : Pawn
{
    [SerializeField] public float m_MoveSpeed;
    [SerializeField] public float m_JumpPower;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (Input.GetButton("Jump"))
        {
            Jump(m_JumpPower);
        }

        Move(
            new Vector3(
                Input.GetAxis("Horizontal") * m_MoveSpeed, 0, 0
            )
        );
    }
}
