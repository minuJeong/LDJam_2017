using UnityEngine;

public class Portal : Interactable
{
    [SerializeField] public GameObject m_Whitehole;

    protected override void Start()
    {
        Debug.Assert(m_Whitehole != null);
        base.Start();
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerPawn>();
        if (player != null)
        {
            player.transform.position = m_Whitehole.transform.position;
            Game.Instance.RememberSpawnPos(m_Whitehole.transform.position);
        }
    }
}
