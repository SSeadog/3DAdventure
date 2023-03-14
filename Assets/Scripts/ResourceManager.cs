using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    public static ResourceManager Instance { get { Init();  return instance; } }
    
    [SerializeField] public Sprite[] _sprites;

    public static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("Managers");
            if (go == null)
            {
                go = new GameObject("Managers");
                ResourceManager mg = go.AddComponent<ResourceManager>();
                instance = mg;
            }
            else
            {
                ResourceManager mg = go.GetComponent<ResourceManager>();
                instance = mg;
            }
        }
    }

    void Start()
    {
        Init();
    }
}
