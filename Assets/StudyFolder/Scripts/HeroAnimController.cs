using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimController : MonoBehaviour
{
    Animator _anim;
    float _moveValue = 0;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        HeroAnimControll();
        //HeroDirAnim();
        //HeroSetAnim();
    }

    void HeroAnimControll()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveZ > 0f && Input.GetKey(KeyCode.LeftShift))
        {
            moveZ *= 2f;
        }
        else if (moveZ > 1f && !Input.GetKey(KeyCode.LeftShift))
        {
            moveZ /= 2f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _anim.SetTrigger("Jump");
        }

        if (Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            _anim.SetTrigger("Attacked");
        }

        transform.position += new Vector3(moveX, 0f, moveZ) * Time.deltaTime * 10f;

        _anim.SetFloat("AxisX", moveX);
        _anim.SetFloat("AxisZ", moveZ);

        
    }

    void HeroDirAnim()
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");

        GetComponent<Rigidbody>().velocity = new Vector3(dirX, 0f, dirZ) * 3f;
        _anim.SetFloat("AxisX", dirX);
        _anim.SetFloat("AxisZ", dirZ);
    }

    void HeroSetAnim()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _moveValue += Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftShift))
                _moveValue += Time.deltaTime * 2f;
        }
        else
        {
            _moveValue -= Time.deltaTime;
        }

        if (_moveValue < 0f)
            _moveValue = 0f;
        if (_moveValue > 1f)
            _moveValue = 1f;

        _anim.SetFloat("MoveValue", _moveValue);
    }
}
