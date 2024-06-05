public interface IController
{
    public void Initialize(IController components);
    public T GetControl<T>();
}

public interface IGameObjectController : IController
{
    public void Die();
}