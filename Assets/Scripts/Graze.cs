using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graze : MonoBehaviour
{
    private Player pl;

    public void SetParent(Player p) { this.pl = p; }

    private Rigidbody player;

    public void SetPlayerobj(Rigidbody obj) { player = obj; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "star")
        {
            var vec = player.velocity;
            vec = vec.normalized;


            player.velocity += vec * 0.3f;
        }
    }
}
