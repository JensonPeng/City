using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Transform player;
    [Header("Components")]
    public CharacterController controller;
    public Animator anim;
    [Header("Datas")]
    public float moveSpeed;
    public float rotateSpeed;
    [Header("Prefabs")]
    public GameObject spark;

    float minigunFireRate;
    float t;

    Vector3 target;

    void Start()
    {
        player = GetComponent<Transform>();

        //Cursor.lockState =  CursorLockMode.Confined;
    }

    
    void Update()
    {
        #region 移動
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float updown = Input.GetAxis("UpDown");

        float mouseX = Input.GetAxis("Rotate") * rotateSpeed * Time.deltaTime;
        player.Rotate(Vector3.up * mouseX);

        Vector3 move = transform.right * h + transform.forward * v + transform.up / 2 * updown;
        controller.Move(Vector3.ClampMagnitude(move, 1f) * moveSpeed * Time.deltaTime);

        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);
        #endregion

        Vector3 firePos = player.position - Vector3.up * 0.3f;

        RaycastHit mousehit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mousehit))
            target = mousehit.point;

        Ray ray = new Ray(firePos, (target + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f))) - firePos);
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                t += Time.deltaTime;
                if (t > minigunFireRate && mousehit.point.y < player.position.y)
                {
                    t = 0;
                    GameObject sparkClone = Instantiate(spark, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(sparkClone, 0.5f);
                }
            }
        }
    }
}
