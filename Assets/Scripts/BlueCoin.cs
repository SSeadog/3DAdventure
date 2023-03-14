using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCoin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMove>().AddCoin();
            Destroy(transform.parent.gameObject);
        }
    }
}
