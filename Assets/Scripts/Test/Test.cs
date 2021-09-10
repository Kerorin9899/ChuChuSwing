using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private GameObject sp;

    private Rigidbody rig;

    private const float GRAVIZATION = 6.67f * 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        rig = sp.GetComponent<Rigidbody>();

        var dir = gameObject.transform.position - sp.transform.position;
        float r = dir.magnitude;
        Vector3 force_vec = -GRAVIZATION * rig.mass * this.GetComponent<Rigidbody>().mass * dir / (r * r * r);

        var acc = force_vec / this.GetComponent<Rigidbody>().mass;
        var vec = acc * Time.deltaTime;


        float first_SpaceVec = Mathf.Sqrt(GRAVIZATION * rig.mass / r);

        float deg = Vector3.Angle(Vector3.up, dir);

        var cro = Vector3.Cross(Vector3.up, dir);

        float rad;
        if (cro.z > 0)
        {
            rad = deg * 3.141592f / 180.0f;
        }
        else
        {
            rad = - deg * 3.141592f / 180.0f;
        }


        this.gameObject.GetComponent<Rigidbody>().AddForce( new Vector3(first_SpaceVec * Mathf.Cos(rad), first_SpaceVec * Mathf.Sin(rad), 0), ForceMode.VelocityChange);

    }

    // Update is called once per frame
    void Update()
    {
        var dir = gameObject.transform.position - sp.transform.position;
        float r = dir.magnitude;
        Vector3 force_vec = -GRAVIZATION * rig.mass * this.GetComponent<Rigidbody>().mass * dir / (r * r * r);

        var acc = force_vec / this.GetComponent<Rigidbody>().mass;
        var vec = acc * Time.deltaTime;


        float first_SpaceVec = Mathf.Sqrt(GRAVIZATION * rig.mass / r);

        float deg = Vector3.Angle(Vector3.up, dir);

        var cro = Vector3.Cross(Vector3.up, dir);

        float rad;
        if(cro.z > 0)
        {
            rad = deg * 3.141592f / 180.0f;
        }
        else
        {
            rad = - deg * 3.141592f / 180.0f;
        }


        //this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(first_SpaceVec * Mathf.Cos(rad), first_SpaceVec * Mathf.Sin(rad), 0);
        this.gameObject.GetComponent<Rigidbody>().velocity += vec;

        //Debug.Log(this.gameObject.GetComponent<Rigidbody>().velocity.magnitude);

        //Debug.Log(force_vec);
    }
}
