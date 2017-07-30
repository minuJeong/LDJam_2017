using UnityEngine;

public abstract class Trap : Interactable
{
    public abstract void OnHit(Pawn pawn);
    private void OnTriggerEnter(Collider other)
    {
        var pawn = other.GetComponent<Pawn>();
        if (pawn)
        {
            OnHit(pawn);
        }
    }
}
