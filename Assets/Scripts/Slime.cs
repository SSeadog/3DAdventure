using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] Monster _monBase;
    
    public void OnHittedEnd()
    {
        _monBase.OnHittedEnd();
    }

    public void OnDead()
    {
        _monBase.OnDead();
    }
}
