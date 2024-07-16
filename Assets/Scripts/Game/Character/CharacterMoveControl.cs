using UnityEngine;

public abstract class CharacterMoveControl : MainControl, IMove
{
    [SerializeField] private CharacterMainController characterComponent;


    protected override void SetController()
    {
        iController = characterComponent;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        characterComponent.onSwitchState += OnSwitchController;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        characterComponent.onSwitchState -= OnSwitchController;
    }

    protected abstract void OnSwitchController(CharacterStateType state);

    protected virtual void Update()
    {
        Move();
    }
    public abstract void Move();
}
