using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(CharacterController))]
public abstract class Pawn : MonoBehaviour
{
    public bool IsLookRight
    {
        get
        {
            return transform.rotation.eulerAngles.y > 90;
        }

        set
        {
            if (value)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    protected bool IsDead = false;

    [SerializeField] public MotionData m_MotionData;
    [SerializeField] public GameObject m_MotionTarget;
    [SerializeField] public float m_JumpCooldown;

    private CharacterController _charCon;
    public CharacterController m_CharCon
    {
        get
        {
            if (_charCon == null)
            {
                _charCon = GetComponent<CharacterController>();
            }
            return _charCon;
        }
    }

    private Vector3 m_PrevPos;
    private Quaternion m_TargetRot;
    [SerializeField] private float m_YSpeed;
    private float m_JumpCooldownFinishTime;

    protected virtual void Start()
    {
        m_PrevPos = transform.position;
        m_JumpCooldownFinishTime = Time.time;
        m_TargetRot = Quaternion.Euler(0, 5, 0);

        MotionIdle();
    }

    protected virtual void Update()
    {
        if (IsDead)
        {
            return;
        }

        // Look at direction
        var delta = transform.position - m_PrevPos;
        if (delta.x < 0)
        {
            m_TargetRot = Quaternion.Euler(0, 5, 0);
        }
        else if (delta.x > 0)
        {
            m_TargetRot = Quaternion.Euler(0, 175, 0);
        }

        // if Y movement is faster than 1 m/s,
        if (delta.y < -1.0F * Time.deltaTime || delta.y > 1.0F * Time.deltaTime)
        {
            MotionY(delta);
        }

        m_PrevPos = transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, m_TargetRot, m_MotionData.LOOK_ROT_SPD_FACTOR);

        // Fall
        Move(new Vector3(0, m_YSpeed, -transform.position.z));
        m_YSpeed += Physics.gravity.y * Time.deltaTime;
    }

    public void Jump(float JumpPower, bool force = false)
    {
        if (force ||
            (m_CharCon.isGrounded &&
             Time.time > m_JumpCooldownFinishTime))
        {
            m_JumpCooldownFinishTime = Time.time + m_JumpCooldown;
            m_YSpeed = JumpPower;
        }
    }

    // Wrapper for intellisense
    public void Move(Vector3 movement)
    {
        if (m_CharCon.enabled)
        {
            m_CharCon.Move(movement);
        }
    }

    protected void MotionIdle()
    {
        float EX = 1.0F + m_MotionData.IDLE_SCALE_INTENS;
        float MN = 1.0F - m_MotionData.IDLE_SCALE_INTENS;

        m_MotionTarget.transform.DOKill();
        m_MotionTarget.transform.localPosition = Vector3.zero;
        m_MotionTarget.transform.DOScale(new Vector3(MN, EX, MN), m_MotionData.IDLE_SCALE_SOOTH)
            .OnComplete(() =>
            {
                m_MotionTarget.transform
                    .DOScale(new Vector3(EX, MN, EX), m_MotionData.IDLE_SCALE_DUR)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.OutQuad);
            });
    }

    protected void MotionY(Vector3 delta)
    {
        const float MXDRV = 0.2F;

        var derivY = delta.y;
        var absD = Mathf.Abs(derivY);
        if (absD > MXDRV)
        {
            derivY = (derivY / absD) * MXDRV;
        }
        var targetScale = new Vector3(1.0F + derivY, 1.0F - derivY, 1.0F + derivY);
        m_MotionTarget.transform.DOKill();
        m_MotionTarget.transform.localScale = targetScale;
        m_MotionTarget.transform.DOScale(targetScale, 0.03F).OnComplete(MotionIdle);
    }

    protected void MotionDie()
    {
        m_MotionTarget.transform.DOKill();
        m_MotionTarget.transform.DOScale(1.0F, m_MotionData.DIE_ROT_DUR);
        m_MotionTarget.transform.DORotate(new Vector3(-90, -90, 0), m_MotionData.DIE_ROT_DUR);
    }

    protected void MotionInvincible()
    {
        m_MotionTarget.transform.DOKill();
        m_MotionTarget.transform.DOShakePosition(m_MotionData.INVIN_SHAK_DUR).OnComplete(MotionIdle);
    }

    public virtual void Die()
    {
        IsDead = true;
        m_CharCon.enabled = false;
        MotionDie();
    }

    /// <summary>
    /// Handles ground hit, trap hit
    /// </summary>
    /// <param name="hit"></param>
    protected void DefaultControllerHitHandler(ControllerColliderHit hit)
    {
        var ground = hit.collider.GetComponent<Walkable>();
        if (ground != null)
        {
            if (hit.moveDirection.y < 0)
            {
                // Hit ground
                m_YSpeed = 0.0F;
            }
            else
            {
                var dotL = Vector3.Dot(hit.point - transform.position, Vector3.right);
                var dotR = Vector3.Dot(hit.point - transform.position, Vector3.left);
                if (dotL > 0.5F || dotR > 0.5F)
                {
                    // Hit wall
                }
                else
                {
                    // Hit ceiling
                    m_YSpeed *= -0.6F;
                }
            }
        }
    }

    public virtual void OnControllerColliderHit(ControllerColliderHit hit)
    {
        DefaultControllerHitHandler(hit);
    }
}

[Serializable]
public class MotionData
{
    public float IDLE_SCALE_SOOTH = 0.025F;
    public float IDLE_SCALE_DUR = 0.1F;
    public float IDLE_SCALE_INTENS = 0.05F;

    public float LOOK_ROT_SPD_FACTOR = 0.27F;

    public float DIE_ROT_DUR = 0.3F;

    public float INVIN_SHAK_DUR = 1.5F;
}
