public interface IControl
{
    public IController iController { get; set; }
    public void Initialize(IController controller);
}

public interface IAttack
{
    public void Attack();
    public void ApplyDamage(IHealth health);
}

public interface IMove
{
    public void Move();
}

public interface IHealth
{
    public int GetHealth();
    public void Revival();
    public void TakeDamage(int damage);
}

public interface ICollision
{

}