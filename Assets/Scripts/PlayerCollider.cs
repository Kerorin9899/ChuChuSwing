using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private Player pl;
    public void SetParent(Player pl) { this.pl = pl; }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "stars")
        {
            pl.SetCollisionStar(col.gameObject);
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            pl.RemoveStarOutRange(col.gameObject);
            pl.RemoveStarOutRangeRigidbody(col.gameObject.GetComponent<Rigidbody>());
        }
    }
}
