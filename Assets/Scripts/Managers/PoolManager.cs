using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour, IManager
{
    [SerializeField] private GameObject[] prefabs;

    private List<GameObject> objectPool = new List<GameObject>(500);

    public static PoolManager Instance { get; private set; }

    public void Initialize()
    {
        Instance = this;
    }

    public GameObject GetObject<T>(Type type = null)
    {
        foreach (var item in objectPool)
        {
            var component = item.GetComponent<T>();
            if (component == null) continue;

            if (type != null && component.GetType() != type) continue;

            item.SetActive(true);
            item.transform.SetParent(null);
            objectPool.Remove(item);
            return item;
        }

        foreach (var item in prefabs)
        {
            var component = item.GetComponent<T>();
            if (component == null) continue;

            if (type != null && component.GetType() != type) continue;

            return Instantiate(item);
        }

        new NullReferenceException();
        return null;
    }
    public void ReturnObjectToPool(GameObject gameObject)
    {
        if (!gameObject) return;
        gameObject.transform.SetParent(transform);
        objectPool.Add(gameObject);
        gameObject.SetActive(false);
    }
}
