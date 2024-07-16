using UnityEngine;

public interface IControl
{
}
public interface IGameObjectControl : IControl
{
    public IGameObjectController iController { get; set; }
    public void Initialize();

}
public interface ICameraControl : IControl
{
    public void Initialize(IController controller);
}
public interface ICanvasControl : IControl
{
    public void Initialize();
}

public interface IAttack : IDamage
{
    public void Attack();
}

public interface IMove
{
    public void Move();
}

public interface IHealth
{
    public int GetMaxHealth();
    public int GetCurrentHealth();
    public void TakeDamage(Damage damage);
}
public interface ICollision
{
    public IGameObjectController iController { get; set; }
}

public interface IAnimation
{

}

public interface IDetect
{
    public IGameObjectController iController { get; set; }
    public T GetControl<T>();
    public bool TryGetControl<T>(out T control);
}