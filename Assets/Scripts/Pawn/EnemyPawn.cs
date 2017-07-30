using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum AI_TYPE
{
    STAND_STILL,
    WALK,
    PATROL
}

public class EnemyPawn : Pawn
{
    private Coroutine _aiCoroutine = null;
    [SerializeField] private AI_TYPE _aiType;
    public AI_TYPE m_AiType
    {
        get { return _aiType; }
        set
        {
            StopAIHandler();
            _aiType = value;
            switch (_aiType)
            {
                case AI_TYPE.STAND_STILL:
                    _aiCoroutine = StartCoroutine(StandStillHandler());
                    break;

                case AI_TYPE.WALK:
                    _aiCoroutine = StartCoroutine(WalkHandler());
                    break;

                default:
                    throw new Exception(
                        string.Format(
                            "Undefined ai type: {0}",
                            _aiType
                        ));
            }
        }
    }



    [SerializeField] public float m_MoveSpeed;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        // call update
        m_AiType = _aiType;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void StopAIHandler()
    {
        if (_aiCoroutine != null)
        {
            StopCoroutine(_aiCoroutine);
            _aiCoroutine = null;
        }
    }

    private IEnumerator StandStillHandler()
    {
        while (true)
        {
            yield return null;
        }
    }

    private IEnumerator WalkHandler()
    {
        while (true)
        {
            yield return null;

            if (IsLookRight)
            {
                Move(Vector3.right * m_MoveSpeed * Time.deltaTime);
            }
            else
            {
                Move(Vector3.left * m_MoveSpeed * Time.deltaTime);
            }
        }
    }


    protected void OnControllerHitWall(ControllerColliderHit hit)
    {
        var wall = hit.collider.GetComponent<Walkable>();
        if (wall == null)
        {
            return;
        }

        var dotL = Vector3.Dot(hit.point - transform.position, Vector3.right);
        var dotR = Vector3.Dot(hit.point - transform.position, Vector3.left);
        if (dotL > 0.5F || dotR > 0.5F)
        {
            IsLookRight = !IsLookRight;
        }
    }

    protected void OnControllerHitPawn(ControllerColliderHit hit)
    {
        var pawn = hit.collider.GetComponent<Pawn>();
        if (pawn == null)
        {
            return;
        }

        var dx = transform.position.x - hit.collider.transform.position.x;
        IsLookRight = dx > 0;
    }

    public override void OnControllerColliderHit(ControllerColliderHit hit)
    {
        DefaultControllerHitHandler(hit);

        var player = hit.collider.GetComponent<PlayerPawn>();
        if (player != null)
        {
            var mc = transform.position + m_CharCon.center;
            var pc = player.transform.position + player.m_CharCon.center;
            var delta = mc - pc;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                player.Die();
            }
        }
        else
        {
            OnControllerHitPawn(hit);
        }

        OnControllerHitWall(hit);
    }
}
