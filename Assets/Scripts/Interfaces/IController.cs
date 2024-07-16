using UnityEngine;

public interface IController
{
    public void Initialize();
}

public interface IGameObjectController : IController
{
    public T GetControl<T>();

    public bool TryGetControl<T>(out T control) where T : Component;
}

public interface ICanvasController : IController
{

}