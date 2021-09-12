using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private Player pl;
    public void SetParent(Player pl) { this.pl = pl; }

    private Rigidbody player;
    public void SetGameObject(Rigidbody obj) { player = obj; }

    private CameraFllow cf;

    public void SetScripts(CameraFllow c) { this.cf = c; }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "stars")
        {

            pl.RemoveStarOutRange(col.gameObject);
            pl.RemoveStarOutRangeRigidbody(col.gameObject.GetComponent<Rigidbody>());

            Vector3 hitpos = Vector3.zero;
            foreach(ContactPoint point in col.contacts)
            {
                hitpos = point.point;
            }

            Vector3 hitvec = (hitpos - player.transform.position).normalized;

            //Debug.Log(hitvec);

            StartCoroutine(cf.CameraShake(player.velocity, col.gameObject.GetComponent<Rigidbody>().velocity, hitvec));

            pl.SetCollisionStar(col.gameObject);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }


}
