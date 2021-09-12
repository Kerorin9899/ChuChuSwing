using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFllow : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject InitilizeStar;

    private Rigidbody rig;

    private Player pl;
    private PlayerCollider pc;
    private PlayScript ps;

    public void SetParent(Player pl) { this.pl = pl; }

    private const float CollisionWaitTime = 0.8f;
    private const int CameraUpZ = -20;
    private const int CameraCommandZ = -40;

    public bool CommandView = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = player.GetComponent<Rigidbody>();

        pc = player.GetComponent<PlayerCollider>();
        pc.SetScripts(this.gameObject.GetComponent<CameraFllow>());

        ps = player.GetComponent<PlayScript>();
        ps.SetCameraFollow(this.gameObject.GetComponent<CameraFllow>());

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(CommandView);
        if (!CommandView)
        {
            UpViewCamera();
        }
        else
        {
            CommandingViewCamera();
        }
    }

    private void UpViewCamera()
    {
        if(transform.position.z < CameraUpZ)
        {
            transform.position += new Vector3(0,0,Time.deltaTime * 30);
        }
        else
        {
            CameraZAxisReset();
        }

        var pos = player.transform.position + player.transform.up * 10;
        var Camera_zIsZero = gameObject.transform.position;
        Camera_zIsZero.z = 0;

        var dir = pos - Camera_zIsZero;

        transform.position += dir * Time.deltaTime * rig.velocity.magnitude;
    }

    private void CameraZAxisReset()
    {
        if (transform.position.z > CameraUpZ)
        {
            var set = transform.position;
            set.z = CameraUpZ;

            transform.position = set;
        }
    }

    private void CommandingViewCamera()
    {
        if(transform.position.z > CameraCommandZ)
        {
            transform.position -= new Vector3(0,0,Time.deltaTime * 30);
        }
        else
        {
            CameraZAxisCommand();
        }

        var dir = pl.InitStarPos() - InitilizeStar.transform.position;

        dir = pl.InitStarPos() + dir.normalized * 30;
        var pos = gameObject.transform.position;
        pos.z = 0;

        dir = dir - pos;

        transform.position += dir * Time.deltaTime * 10;
    }

    private void CameraZAxisCommand()
    {
        if (transform.position.z > CameraCommandZ)
        {
            var set = transform.position;
            set.z = CameraCommandZ;

            transform.position = set;
        }
    }

    public IEnumerator CameraShake(Vector3 vec1, Vector3 vec2, Vector3 hitvec)
    {

        var vec = vec1 - vec2;
        //Debug.Log(vec);
        float length_vec = vec.magnitude;

        float deg = Vector3.Angle(vec, hitvec);
        var cro = Vector3.Cross(vec, hitvec);

        float rad = 0;
        if(cro.z > 0)
        {
            rad = deg * 3.14f / 180.0f;
        }
        else
        {
            rad = - deg * 3.14f / 180.0f;
        }

        var shake_vec = new Vector3(length_vec * Mathf.Cos(rad), length_vec * Mathf.Sin(rad), 0);
        shake_vec = shake_vec.normalized;
        float time = 0;

        var pos = transform.position;

        while (true)
        {
            var exp_bias = length_vec * 0.2f - 0.4f * time;
            transform.position = pos + (shake_vec * Mathf.Cos(time) * exp_bias);

            if (exp_bias < Time.deltaTime)
            {
                break;
            }

            time += Time.deltaTime * 10;

            yield return null;
        }

        StartCoroutine(RotateUp(vec));

        yield return new WaitForSeconds(CollisionWaitTime);

        CommandView = true;
        player.transform.position -= hitvec.normalized * 0.3f;
        pl.FirstSpaceVector();
    }

    private IEnumerator RotateUp(Vector3 hitvec)
    {
        
        while ( true )
        {
            var vec = rig.gameObject.transform.up;
            rig.transform.rotation = RotateToQuaternion(rig.transform, vec, - hitvec);

            float deg = Vector3.Angle(vec, hitvec);

            Debug.Log(deg);

            if (deg <= 0.1f)
            {
                break;
            }

            yield return null;
        }

    }

    private Quaternion RotateToQuaternion(Transform t, Vector3 from, Vector3 to)
    { 
        Vector3 cross = Vector3.Cross(from, to);

        Quaternion rot = Quaternion.AngleAxis(Time.deltaTime * 50, cross);
        Quaternion q = t.rotation;

        return q * rot;
    }

}
