using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Transform _cam;
    [SerializeField] Collider _sword;
    [SerializeField] GameObject _gameOver;
    [SerializeField] Inventory _inven;
    Animator _anim;

    [SerializeField] int _hp = 5;
    int _coin = 0;

    bool _canHitted = true;

    void Start()
    {
        _anim = GetComponent<Animator>();
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

    public void Hitted()
    {
        if (!_canHitted)
            return;

        _hp--;
        if (_hp <= 0)
        {
            // GameOver
            OnDead();
        }
        else
        {
            // hitted;
            _anim.SetTrigger("Hitted");
        }
        _canHitted = false;
        StartCoroutine(CoHittedCoolTime());
    }

    IEnumerator CoHittedCoolTime()
    {
        yield return new WaitForSeconds(1f);
        _canHitted = true;
    }

    void OnDead()
    {
        _gameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void AddCoin()
    {
        Item item = new Item();
        int count = Random.Range(1, 100);
        EItemType eType = (EItemType)Random.Range(1, (int)EItemType.Max - 1);
        item._eType = eType;
        item._count = count;
        _inven.AddItem(item);

    }
}
