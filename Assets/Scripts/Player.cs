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
    public void RemoveCollisionStar(GameObject star) { InitStar = null; InitStarRigid = null; }

    // Start is called before the first frame update
    void Start()
    {
        var space = GetComponentInChildren<TestGravity>();

        rig = GetComponent<Rigidbody>();

        tg = space;
        tg.SetParent(GetComponent<Player>());

        pc = GetComponent<PlayerCollider>();
        pc.SetParent(GetComponent<Player>());

        ps = GetComponent<PlayScript>();
        ps.SetParent(GetComponent<Player>());

        InitStar = Earth;
    }

    // Update is called once per frame
    void Update()
    {

        if(InitStar != null)
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

            var plus_vec = rig.velocity * Time.deltaTime * 0.1f;

            rig.velocity += vec + plus_vec;
        }
    }

    private void RevolveGravizarion()
    {
        var dir = gameObject.transform.position - InitStar.transform.position;
        float r = dir.magnitude;
        var force_vec = - GRAVIZATION * rig.mass * InitStarRigid.mass * dir / (r * r * r);

        float first_SpaceVec = Mathf.Sqrt( GRAVIZATION * InitStarRigid.mass / ((InitStar.transform.localScale.x * 0.5f) + 1) );

        var acc = force_vec / rig.mass;
        var vec = acc * 0.5f;

        float y_vec = 6;
    }

    private void ChangeBool(bool b)
    {
        b = !b;
    }

    private void LimitSpped()
    {

    }
}
