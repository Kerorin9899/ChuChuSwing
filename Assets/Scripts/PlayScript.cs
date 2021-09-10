using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private Camera _main;

    private Rigidbody rig;

    private Vector3 mouse_pos;

    private const float max_rotate_degree = 30.0f;
    private const float brake_bias = 0.1f;

    private float _max = 0;

    private Player pl;

    public void SetParent(Player p) { pl = p; }

    // Start is called before the first frame update
    void Start()
    {
        rig = Player.GetComponent<Rigidbody>();

        rig.AddForce(Vector3.right * 2, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        //if(pl.ShootPlayer)
        //{

        //}

        //　マウスポインタ座標取得
        MousePoint();

        // ベクトル変更
        VectorChange();

        // ブレーキ
        if (Input.GetKey(KeyCode.Space))
        {
            BrakeSpeed();
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
        var forward = gameObject.transform.right;
        var pos = _main.WorldToScreenPoint(gameObject.transform.position);
        pos.z = 0;
        var PlayerToMousePointer = (mouse_pos - pos).normalized;

        Vector3 rot = RotateToVector(forward, PlayerToMousePointer);

        float det = rig.velocity.magnitude;

        rig.velocity = rot * det;
    }

    private Vector3 RotateToVector(Vector3 from, Vector3 to)
    {
        float deg = Vector3.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);

        Quaternion rot = Quaternion.AngleAxis(Mathf.Clamp( deg, 0, max_rotate_degree) * Time.deltaTime, cross);
        Quaternion q = transform.rotation;

        transform.rotation = q * rot;

        return transform.right;
    }
}
