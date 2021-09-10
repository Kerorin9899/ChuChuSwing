using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGravity : MonoBehaviour
{
    private Player pl;
    public void SetParent(Player p) { this.pl = p; }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "stars")
        {
            pl.SetStarsInRange(col.gameObject);
            pl.SetStarsInRangeRigidbody(col.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "stars")
        {
            pl.RemoveStarOutRange(other.gameObject);
            pl.RemoveStarOutRangeRigidbody(other.GetComponent<Rigidbody>());
        }
    }
}
