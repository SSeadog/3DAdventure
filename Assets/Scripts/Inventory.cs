using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject _ui;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _itemPrefab;
    List<Item> _items = new List<Item>();

    public void OpenOrClose()
    {
        _ui.SetActive(!_ui.activeSelf);
    }

    public void AddItem(Item item)
    {
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