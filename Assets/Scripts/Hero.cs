using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    [SerializeField] GameObject _gameOver;
    [SerializeField] Inventory _inven;
    [SerializeField] Transform _hpBar;
    [SerializeField] int _hp = 5;
    Animator _anim;
    int _coin = 0;
    bool _canHitted = true;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Hitted()
    {
        if (!_canHitted)
            return;

        _hp--;
        SetHpBar();
        
        if (_hp <= 0)
            OnDead();
        else
            _anim.SetTrigger("Hitted");

        _canHitted = false;
        StartCoroutine(CoHittedCoolTime());
    }

    void SetHpBar()
    {
        _hpBar.localScale = new Vector3(_hp / 5f, 1f, 1f);
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

    public void AddCoin(Item item)
    {
        _inven.AddItem(item);
    }
}
