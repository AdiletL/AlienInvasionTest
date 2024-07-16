using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class MainController : MonoBehaviour, IGameObjectController
{
    [SerializeField] protected List<GameObject> componentInGameObjects;

    protected List<IGameObjectControl> controls = new List<IGameObjectControl>();


    public virtual void Initialize()
    {
        foreach (var item1 in componentInGameObjects)
        {
            var components = item1.GetComponents<IGameObjectControl>();
            foreach (var item2 in components)
            {
                if (item2 != null)
                {
                    this.controls.Add(item2);
                    item2.Initialize();
                }
            }
        }
    }

    public T GetControl<T>()
    {
        if (controls.Count == 0) Initialize();
        foreach (var item in controls)
        {
            if (item is T control)
                return control;
        }
        return GetControlInGameObject<T>();
    }

    public List<T> GetControls<T>()
    {
        List<T> values = new List<T>(controls.Count);
        foreach (var item in componentInGameObjects)
        {
            if (item is T control)
                values.Add(control);
        }
        return values;
    }

    public T GetControlInGameObject<T>()
    {
        foreach (var item in componentInGameObjects)
        {
            if (item is T control)
                return control;
        }
        return default;
    }

    public bool TryGetControl<T>(out T control) where T : Component
    {
        control = null;
        if (controls.Count == 0) Initialize();
        foreach (var item in controls)
        {
            if (item is T target)
            {
                control = (T)item;
                return true;
            }
        }
        return GetControlInGameObject<T>(out control);
    }
    private bool GetControlInGameObject<T>(out T control) where T : Component
    {
        foreach (var item in componentInGameObjects)
        {
            control = item.GetComponent<T>();
            if (control != null)
            {
                return true;
            }
        }
        control = null;
        return false;
    }

    public void AddGameObjectWithControl<T>(GameObject gameObject)
    {
        componentInGameObjects.Add(gameObject);
        gameObject.transform.SetParent(transform.GetChild(0));
    }

    public void RemoveGameObjectsWithControl<T>() where T : Component
    {
        var controls = GetComponentsInChildren<T>();
        for (int i = controls.Length - 1; i > 0; i--)
        {
            Destroy(controls[i].gameObject);
        }
    }
    public void RemoveControls<T>() where T : Component
    {
        var controls = GetComponentsInChildren<T>();
        for (int i = controls.Length - 1; i > 0; i--)
        {
            Destroy(controls[i]);
        }
    }
}
