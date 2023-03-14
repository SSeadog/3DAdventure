using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Coin : MonoBehaviour
{
    public Item itemType;
    public void Start()
    {
        itemType = new Item();
        int count = Random.Range(1, 100);
        EItemType eType = (EItemType)Random.Range(1, (int)EItemType.Max - 1);
        itemType._eType = eType;
        itemType._count = count;

        foreach (Sprite sp in ResourceManager.Instance._sprites)
        {
            if (itemType._eType.ToString() == sp.name)
                itemType._sprite = sp;
        }

        GetComponent<SpriteRenderer>().sprite = itemType._sprite;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Hero>().AddCoin(itemType);
            Destroy(transform.parent.gameObject);
        }
    }
}
