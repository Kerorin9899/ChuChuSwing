using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;

    public Camera _main;

    private Rigidbody rig;

    private Vector3 mouse_pos;

    private const float max_rotate_degree = 50.0f;
    private const float brake_bias = 0.1f;

    private float _max = 0;

    private Player pl;

    public void SetParent(Player p) { pl = p; }

    private CameraFllow cf;
    public void SetCameraFollow(CameraFllow c) { cf = c; }

    // Start is called before the first frame update
    void Start()
    {
        rig = Player.GetComponent<Rigidbody>();

        rig.AddForce(Vector3.up * 2, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(!pl.ExistStar());
        if (pl.ExistStar())
        {

            //　マウスポインタ座標取得
            MousePoint();

            // ベクトル変更
            VectorChange();

            // ブレーキ
            if (Input.GetKey(KeyCode.Space))
            {
                BrakeSpeed();
            }

            // 加速
            if (Input.GetKey(KeyCode.B))
            {

            }
        }
        else
        {

            if (pl.cf.CommandView)
            {

                // 射出
                if (Input.GetMouseButtonDown(0))
                {
                    Launch();
                }

            }
        }
    }

    private void MousePoint()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos.z = 0;
    }

    private void BrakeSpeed()
    {
        var norm = rig.velocity.normalized;

        rig.velocity -= norm * Time.deltaTime * brake_bias;
    }

    private void VectorChange()
    {
        var up = gameObject.transform.up;
        var pos = _main.WorldToScreenPoint(gameObject.transform.position);
        pos.z = 0;
        var PlayerToMousePointer = (mouse_pos - pos).normalized;

        Vector3 rot = RotateToVector(up, PlayerToMousePointer);

        float det = rig.velocity.magnitude;

        rig.velocity = rot * det;
    }

    public Vector3 RotateToVector(Vector3 from, Vector3 to)
    {
        float deg = Vector3.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        Quaternion rot = Quaternion.AngleAxis(Mathf.Clamp( deg, 0, max_rotate_degree) * Time.deltaTime, cross);
        Quaternion q = transform.rotation;

        transform.rotation = q * rot;

        return transform.up;
    }

    private void Launch()
    {
        ResetCommand();
    }


    private void ResetCommand()
    {
        pl.cf.CommandView = false;
        pl.RemoveCollisionStar();
    }
}
