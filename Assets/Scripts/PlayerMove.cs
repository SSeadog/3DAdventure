using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Transform _cam;
    [SerializeField] Collider _sword;
    
    Animator _anim;
    Hero _hero;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _hero = GetComponent<Hero>();
    }

    void Update()
    {
        Move();
        Action();
    }

    void Move()
    {
        bool isRun = false;

        transform.rotation = Quaternion.LookRotation(new Vector3(_cam.transform.forward.x, 0f, _cam.transform.forward.z));

        float vX = Input.GetAxisRaw("Horizontal");
        float vZ = Input.GetAxisRaw("Vertical");
        float vY = GetComponent<Rigidbody>().velocity.y;

        if (Input.GetKey(KeyCode.LeftShift))
            isRun = true;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 v3;

        if (isRun)
            v3 = (forward * vZ + right * vX).normalized + forward * vZ;
        else
            v3 = (forward * vZ + right * vX).normalized;

        Vector3 vYz = v3 * 4.5f;
        vYz.y += vY;
        GetComponent<Rigidbody>().velocity = vYz;

        if (isRun)
            vZ *= 2f;

        _anim.SetFloat("AxisX", vX);
        _anim.SetFloat("AxisZ", vZ);
    }

    void Action()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _anim.SetTrigger("Jump");
        if (Input.GetMouseButtonDown(0))
        {
            _sword.enabled = true;
            _anim.SetTrigger("Attack");
        }
    }

    void EndAttack()
    {
        _sword.enabled = false;
    }
}
