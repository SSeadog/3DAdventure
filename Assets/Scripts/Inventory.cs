using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject _ui;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _itemPrefab;
    List<Item> _items = new List<Item>();

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenOrClose();
        }
    }

    public void OpenOrClose()
    {
        Debug.Log("OpenOrClose()");
        _ui.SetActive(!_ui.activeSelf);
    }

    public void AddItem(Item item)
    {
        Debug.Log(_sprites.Length);
        foreach (Sprite sp in _sprites)
        {
            foreach (EItemType type in Enum.GetValues(typeof(EItemType)))
            {
                if (type.ToString().Equals(sp.name))
                {
                    item._sprite = sp;
                }
            }
        }

        _items.Add(item);
        GameObject temp = Instantiate(_itemPrefab, _content);
        temp.GetComponent<ItemUI>().Init(item);
    }
}

public enum EItemType
{
    None,
    Blue,
    Green,
    Gold,
    Brown,
    Purple,
    Max
}

public class Item
{
    public EItemType _eType;
    public int _count;
    public Sprite _sprite;
}