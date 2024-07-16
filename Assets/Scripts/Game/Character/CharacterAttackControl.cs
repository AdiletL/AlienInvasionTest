using UnityEngine;

public abstract class CharacterAttackControl : MainControl, IAttack
{
    [SerializeField] private CharacterMainController characterMainController;

    protected Damage damage;

    protected abstract Damage CreateDamage();

    public override void Initialize()
    {
        base.Initialize();
        damage = CreateDamage();
    }
    protected override void SetController()
    {
        iController = characterMainController;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        characterMainController.onSwitchState += OnSwitchController;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        characterMainController.onSwitchState -= OnSwitchController;
    }

    protected abstract void OnSwitchController(CharacterStateType state);

    protected virtual void Update()
    {
        Attack();
    }
    public abstract void Attack();
    public abstract void ApplyDamage(IHealth health);
}
