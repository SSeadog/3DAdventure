using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EMonState
{
    None,
    Idle,
    Move,
    Attack,
    Hitted,
    Die,
}

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject _mon;
    [SerializeField] GameObject _target;
    [SerializeField] GameObject _coin;
    [SerializeField] GameObject _attackSpace;

    Animator _anim;

    EMonState _eState = EMonState.None;

    int _hp = 0;
    float _searchDis = 10f;
    float _attackDis = 2f;
    float _speed = 2f;

    bool canHitted = true;

    void Start()
    {
        _anim = _mon.GetComponent<Animator>();

        StartCoroutine(CoSpawn());
    }

    void Update()
    {
        if (_eState == EMonState.Idle) MoveAndSearch();
        if (_eState == EMonState.Attack) FollowAndAttack();
        if (_eState == EMonState.Hitted) { }
        _attackSpace.SetActive(_eState == EMonState.Attack);
    }

    IEnumerator CoSpawn()
    {
        int rand = Random.Range(2, 5);
        yield return new WaitForSeconds(rand);
        Spawn();
    }

    void Spawn()
    {
        _hp = 1;
        _mon.SetActive(true);
        _eState = EMonState.Idle;
    }

    void MoveAndSearch()
    {
        float dis = Vector3.Distance(_target.transform.position, transform.position);
        if (dis < _searchDis)
        {
            if (dis < _attackDis)
            {
                _eState = EMonState.Attack;
            }
            else
            {
                _anim.Play("Move");
                Vector3 lookDir = _target.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0f, lookDir.z));
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
            }
        }
        else
        {
            _eState = EMonState.Move;
            StartCoroutine(CoRandomMove());
        }
    }

    void FollowAndAttack()
    {
        _anim.Play("Attack");

        float dis = Vector3.Distance(_target.transform.position, transform.position);
        if (dis > _searchDis)
        {
            _eState = EMonState.Idle;
        }
        else if (dis > _attackDis)
        {
            _anim.Play("Move");
            Vector3 lookDir = _target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0f, lookDir.z));
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
        }
    }

    IEnumerator CoRandomMove()
    {
        // 벽이 있는지 체크(몸을 돌려서 갈 수 있는지 확인)
        bool canGo = true;
        {
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            transform.rotation = Quaternion.LookRotation(randomDir);

            yield return new WaitForSeconds(1f);

            RaycastHit hit;
            Vector3 startPoint = transform.position + transform.forward * 0.6f + Vector3.up * 0.5f;
            Debug.DrawRay(startPoint, transform.forward * 2f, Color.yellow, 1f);
            if (Physics.Raycast(startPoint, transform.forward, out hit, 2f))
                canGo = false;
        }

        if (canGo)
        {
            // 낭떠러지인지 체크
            {
                RaycastHit hit;
                Vector3 targetDir = transform.position + transform.forward * 2f;
                Debug.DrawRay(targetDir + new Vector3(0f, 0.5f, 0f), Vector3.down * 2f, Color.red, 5f);
                if (Physics.Raycast(targetDir + new Vector3(0f, 0.5f, 0f), Vector3.down, out hit, 2f))
                {
                    while (Vector3.Distance(targetDir, transform.position) > 0.5f)
                    {
                        _anim.Play("Move");
                        transform.position = Vector3.MoveTowards(transform.position, targetDir, Time.deltaTime * _speed);
                        yield return null;
                    }
                }
            }

            yield return new WaitForSeconds(1f);
        }

        _eState = EMonState.Idle;
    }

    // 아래 버전은 갈수 있는 길을 찾을 때까지 while을 도는데 이때 플레이어 감지하는지 확인해보기
    //IEnumerator CoRandomMove()
    //{
    //    // 벽이 있는지 체크(몸을 돌려서 갈 수 있는지 확인)
    //    {
    //        while (true)
    //        {
    //            RaycastHit hit;
    //            Vector3 startPoint = transform.position + transform.forward * 0.6f + Vector3.up * 0.5f;
    //            Debug.DrawRay(startPoint, transform.forward * 2f, Color.yellow, 1f);
    //            if (Physics.Raycast(startPoint, transform.forward, out hit, 2f))
    //            {
    //                Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
    //                transform.rotation = Quaternion.LookRotation(randomDir);
    //                yield return new WaitForSeconds(0.1f);
    //            }
    //            else
    //            {
    //                Debug.DrawRay(startPoint, transform.forward * 2f, Color.red, 5f);
    //                Debug.Log("Can Go!!");
    //                yield return new WaitForSeconds(0.5f);
    //                break;
    //            }
    //        }
    //    }

    //    // 낭떠러지인지 체크
    //    {
    //        RaycastHit hit;
    //        Vector3 targetDir = transform.position + transform.forward * 2f;
    //        Debug.DrawRay(targetDir + new Vector3(0f, 0.5f, 0f), Vector3.down * 2f, Color.red, 5f);
    //        if (Physics.Raycast(targetDir + new Vector3(0f, 0.5f, 0f), Vector3.down, out hit, 2f))
    //        {
    //            while (Vector3.Distance(targetDir, transform.position) > 0.5f)
    //            {
    //                _anim.Play("Move");
    //                transform.position = Vector3.MoveTowards(transform.position, targetDir, Time.deltaTime * _speed);
    //                yield return null;
    //            }
    //        }
    //    }

    //    yield return new WaitForSeconds(1f);
        
    //    _eState = EMonState.Idle;
    //}

    public void Hitted()
    {
        if (!canHitted)
            return;

        _hp--;
        if (_hp <= 0)
        {
            Debug.Log("Die");
            _anim.Play("Die");
            _eState = EMonState.Die;
        }
        else
        {
            _anim.Play("Hitted");
        }

        canHitted = false;
        _eState = EMonState.Hitted;
    }

    public void OnHittedEnd()
    {
        canHitted = true;
        _eState = EMonState.Idle;
    }    

    public void OnDead()
    {
        _mon.SetActive(false);
        GameObject tmp = Instantiate(_coin);
        tmp.transform.position = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMove>().Hitted();
        }
    }
}
