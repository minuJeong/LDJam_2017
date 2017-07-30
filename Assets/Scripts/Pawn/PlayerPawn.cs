using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerPawn : Pawn
{
    [SerializeField] public int m_Life;
    [SerializeField] public float m_InvincibleTime;
    [SerializeField] public float m_MoveSpeed;
    [SerializeField] public float m_JumpPower;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        ShuffleAbilities();
    }

    private void ShuffleAbilities()
    {
        m_MoveSpeed = Random.Range(6.0F, 19.0F);
        m_JumpPower = Random.Range(0.55F, 0.80F);
        m_Life = Random.Range(1, 3);

        Game.Instance.m_AbilityBoard.text = string.Format(
            "Move Speed: {0:F2}\nJump Power: {1:F2}\nLife: {2}",
            m_MoveSpeed, m_JumpPower, m_Life
        );

        Game.Instance.m_LifeHeartContainer.SetHeartcount(m_Life);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Game.Instance.Restart();
            return;
        }

        if (IsDead)
        {
            return;
        }

        if (Input.GetButton("Jump"))
        {
            Jump(m_JumpPower, false);
        }
        Move(new Vector3(Input.GetAxis("Horizontal") * m_MoveSpeed * Time.deltaTime, 0, 0));

        base.Update();
    }

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        DefaultControllerHitHandler(hit);

        var enemy = hit.collider.GetComponent<EnemyPawn>();
        if (enemy != null && !(enemy is CivilanPawn))
        {
            var dotD = Vector3.Dot(hit.point - enemy.transform.position, Vector3.up);
            if (dotD > 0.5F)
            {
                Jump(m_JumpPower, true);
                enemy.Die();
            }
        }
    }

    public override void Die()
    {
        if (Time.time < m_InvincibleTime)
        {
            return;
        }

        if (m_Life-- > 0)
        {
            Game.Instance.m_LifeHeartContainer.SetHeartcount(m_Life);
            m_InvincibleTime = Time.time + 1.5F;
            MotionInvincible();
            Jump(m_JumpPower, true);
            return;
        }
        base.Die();
    }
}
