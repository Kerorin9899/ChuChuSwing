using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFllow : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        rig = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var pos = player.transform.position + player.transform.right * 10;
        var Camera_zIsZero = gameObject.transform.position;
        Camera_zIsZero.z = 0;

        var dir = pos - Camera_zIsZero;

        transform.position += dir * Time.deltaTime * rig.velocity.magnitude;
    }
}
