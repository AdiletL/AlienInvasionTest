using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class MainBehaviour : MonoBehaviour, IGameObjectController
{
    [SerializeField] protected List<GameObject> componentInGameObjects;

    protected List<IControl> controls = new List<IControl>();


    protected virtual void Awake()
    {
        Initialize(this);
    }

    public virtual void Initialize(IController component)
    {
        foreach (var item1 in componentInGameObjects)
        {
            var components = item1.GetComponents<IControl>();
            foreach (var item2 in components)
            {
                if (item2 != null)
                {
                    this.controls.Add(item2);
                    item2.Initialize(component);
                }
            }
        }
    }

    public T GetControl<T>()
    {
        foreach (var item in componentInGameObjects)
        {
            if (item.GetComponent<T>() != null)
                return item.GetComponent<T>();
        }
        return default;
    }
    public T GetControl<T>(Type value)
    {
        foreach (var item in componentInGameObjects)
        {
            var result = item.GetComponent<T>();
            if (result != null && result.GetType() == value)
                return result;
        }
        return default;
    }

    public List<T> GetControls<T>()
    {
        List<T> values = new List<T>(componentInGameObjects.Capacity);
        foreach (var item in componentInGameObjects)
        {
            T value = item.GetComponent<T>();
            if (value != null)
                values.Add(value);
        }
        return values;
    }

    public void AddGameObjectWithControl<T>(GameObject gameObject, Type value)
    {
        foreach (var item in componentInGameObjects)
        {
            var target = item.GetComponent<T>();
            if (target != null && target.GetType() == value)
            {
                return;
            }
        }

        componentInGameObjects.Add(gameObject);
        gameObject.transform.parent = transform.GetChild(0);
    }

    public void RemoveGameObjectWithControl<T>(Type value)
    {
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (GameObject obj in objectsToRemove)
        {
            var target = obj.GetComponent<T>();
            if (target != null && target.GetType() == value)
            {
                objectsToRemove.Add(obj);
            }
        }

        foreach (GameObject obj in objectsToRemove)
        {
            componentInGameObjects.Remove(obj);
        }
    }

    public virtual void Die()
    {
        
    }
}
