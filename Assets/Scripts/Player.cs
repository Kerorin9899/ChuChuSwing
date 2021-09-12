using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject Earth;

    private GameObject g;
    private Rigidbody rig;

    const float GRAVIZATION = 6.67f * 0.0001f;

    protected TestGravity tg;
    protected PlayerCollider pc;
    protected PlayScript ps;
    protected Graze gr;
    public CameraFllow cf;

    protected List<GameObject> InRangeStars  = new List<GameObject>();
    protected List<Rigidbody> InRangeStarsRigidbody = new List<Rigidbody>();
    public void SetStarsInRange(GameObject stars) { InRangeStars.Add(stars); }
    public void SetStarsInRangeRigidbody(Rigidbody rig) { InRangeStarsRigidbody.Add(rig); }
    public void RemoveStarOutRange(GameObject stars) { InRangeStars.Remove(stars); }
    public void RemoveStarOutRangeRigidbody(Rigidbody stars) { InRangeStarsRigidbody.Remove(stars); }

    protected GameObject InitStar;
    protected Rigidbody InitStarRigid;
    public bool ShootPlayer = false;
    public void SetCollisionStar(GameObject star) { InitStar = star; InitStarRigid = star.GetComponent<Rigidbody>(); }
    public void RemoveCollisionStar() { InitStar = null; InitStarRigid = null; }

    public Vector3 InitStarPos() { return InitStar.transform.position; }

    public float ConstGrav() { return GRAVIZATION; }

    private float SumTime = 0;

    private bool lightTrip = false;

    private Vector3 first_spacevec = Vector3.zero;
    public bool ExistStar() {
        if (InitStar != null)
            return false;
        else
            return true; 
    }

    private void Awake()
    {
        InitStar = Earth;

        //Debug
        InitStar = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        var space = GetComponentInChildren<TestGravity>();

        rig = GetComponent<Rigidbody>();

        Player pl = GetComponent<Player>();

        tg = space;
        tg.SetParent(pl);

        pc = GetComponent<PlayerCollider>();
        pc.SetParent(pl);
        pc.SetGameObject(rig);

        ps = GetComponent<PlayScript>();
        ps.SetParent(pl);

        cf = ps._main.GetComponent<CameraFllow>();
        cf.SetParent(pl);

        gr = GetComponentInChildren<Graze>();
        gr.SetParent(pl);
        gr.SetPlayerobj(rig);

    }

    // Update is called once per frame
    void Update()
    {
        //var vel = rig.velocity;
        //vel.z = 0;
        //rig.velocity = vel;

        //Debug.Log();
        if(!ExistStar())
        {
            if(!ShootPlayer)
            {
                ChangeBool(ShootPlayer);
            }

            RevolveGravizarion();
        }
        else
        {
            if (ShootPlayer)
            {
                ChangeBool(ShootPlayer);
            }

            univGravization();
        }
    }

    private void univGravization()
    {
        foreach (var s in InRangeStarsRigidbody)
        {
            var dir = gameObject.transform.position - s.transform.position;
            float r = dir.magnitude;
            Vector3 force_vec = - GRAVIZATION * rig.mass * s.mass * dir / (r * r * r);

            var acc = force_vec / rig.mass;
            var vec = acc * Time.deltaTime;

            var plus_vec = rig.velocity * Time.deltaTime * 0.01f;

            rig.velocity += vec + plus_vec;
        }
    }

    private void RevolveGravizarion()
    {
        var radius = InitStar.transform.localScale.x * 0.5f + 1;

        var dir = gameObject.transform.position - InitStar.transform.position;
        float r = dir.magnitude;

        if (radius > r)
        {
            var force_vec = -GRAVIZATION * rig.mass * InitStarRigid.mass * dir / (r * r * r);
            var acc = force_vec / rig.mass;
            var vec = acc * Time.deltaTime;

            rig.velocity += vec;
        }
        else
        {
            var vec = new Vector3((radius + 0.9f) * Mathf.Cos(SumTime), (radius + 0.9f) * Mathf.Sin(SumTime), 0);

            transform.position = InitStar.transform.position + vec;

            transform.rotation = RotateToQuaternion(transform, - transform.up, dir);

            SumTime += Time.deltaTime * 3;
        }
    }

    public void FirstSpaceVector()
    {
        var dir = gameObject.transform.position - InitStar.transform.position;
        float d = 1.6f;
        float r = dir.magnitude;
        Vector3 force_vec = - GRAVIZATION * InitStarRigid.mass * rig.mass * dir / (r * r * r);

        Vector3 ref_vec = (-force_vec).normalized;
        float Secondvec = Mathf.Sqrt(2 * GRAVIZATION * InitStarRigid.mass * d) / r;

        //var acc = force_vec / rig.mass;
        //var vec = acc * Time.deltaTime;

        float deg = Vector3.Angle(Vector3.right, dir);

        var cro = Vector3.Cross(Vector3.right, dir);

        float rad;
        if (cro.z > 0)
        {
            rad = deg * 3.141592f / 180.0f;
        }
        else
        {
            rad = -deg * 3.141592f / 180.0f;
        }

        var space_vec = ref_vec * Secondvec;

        SumTime =  rad;

        rig.AddForce(space_vec, ForceMode.VelocityChange);
    }

    private Quaternion RotateToQuaternion(Transform t, Vector3 from, Vector3 to)
    {
        float deg = Vector3.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        Quaternion rot = Quaternion.AngleAxis(deg, cross);
        Quaternion q = t.rotation;

        return q * rot;
    }

    private void ChangeBool(bool b)
    {
        b = !b;
    }

    private void LimitSpped()
    {

    }
}
