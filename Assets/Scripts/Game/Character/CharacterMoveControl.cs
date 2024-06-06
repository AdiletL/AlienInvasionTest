using UnityEngine;

public class CharacterMoveControl : MonoBehaviour, IControl, IMove
{
    public IController iController { get; set; }

    private CharacterMainController characterComponent;

    private bool isEnabled;

    public virtual void Initialize(IController controller)
    {
        iController = controller;
        characterComponent = controller as CharacterMainController;
        if (characterComponent == null)
        {
            enabled = false;
            return;
        }
    }
    protected virtual void OnEnable()
    {
        characterComponent.onSwitchState += OnSwitchController;
    }
    protected virtual void OnDisable()
    {
        characterComponent.onSwitchState -= OnSwitchController;
    }

    private void OnSwitchController(CharacterStateType state)
    {
        isEnabled = state.HasFlag(CharacterStateType.move) ? true : false;
    }

    protected virtual void Update()
    {
        if (!isEnabled) return;

        Move();
    }
    public virtual void Move()
    {
        
    }
}
