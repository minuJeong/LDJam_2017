using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
public class Pawn : MonoBehaviour
{
    [SerializeField] public GameObject m_MotionTarget;
    [SerializeField] public float m_JumpCooldown;

    private CharacterController m_CharCon;
    private Vector3 m_PrevPos;
    private Quaternion m_TargetRot;
    private float m_FallSpeed;
    private float m_JumpCooldownFinishTime;

    protected virtual void Start()
    {
        if (m_CharCon == null)
        {
            m_CharCon = GetComponent<CharacterController>();
        }

        m_PrevPos = transform.position;
        m_JumpCooldownFinishTime = Time.time;

        MotionIdle();
    }

    protected virtual void Update()
    {
        // Look at direction
        var delta = transform.position - m_PrevPos;
        if (delta.x < 0)
        {
            m_TargetRot = Quaternion.identity * Quaternion.Euler(0, 5, 0);
        }
        else if (delta.x > 0)
        {
            m_TargetRot = Quaternion.identity * Quaternion.Euler(0, 175, 0);
        }

        // if Y movement is faster than 1 m/s,
        if (delta.y < -1.0F * Time.deltaTime || delta.y > 1.0F * Time.deltaTime)
        {
            MotionY(delta);
        }

        m_PrevPos = transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, m_TargetRot, 0.25F);

        // Fall
        m_FallSpeed += Physics.gravity.y * Time.deltaTime;
        m_CharCon.Move(new Vector3(0, m_FallSpeed, 0));
        if (m_CharCon.isGrounded)
        {
            m_FallSpeed = 0.0F;
        }
    }

    public void Jump(float JumpPower)
    {
        if (m_CharCon.isGrounded && Time.time > m_JumpCooldownFinishTime)
        {
            m_JumpCooldownFinishTime = Time.time + m_JumpCooldown;
            m_FallSpeed = JumpPower;
        }
    }

    public void Move(Vector3 movement)
    {
        m_CharCon.Move(movement);
    }

    protected void MotionIdle()
    {
        const float EX = 1.05F;
        const float MN = 0.95F;

        m_MotionTarget.transform.DOKill();
        m_MotionTarget.transform.DOScale(new Vector3(MN, EX, MN), 0.025F)
            .OnComplete(() =>
            {
                m_MotionTarget.transform
                    .DOScale(new Vector3(EX, MN, EX), 0.1F)
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
}
