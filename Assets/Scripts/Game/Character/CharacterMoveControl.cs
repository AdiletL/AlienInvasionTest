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

    private void Start()
    {
        if (!characterComponent)
        {
            enabled = false;
            return;
        }
        InitializeEvent();
    }
    protected virtual void InitializeEvent()
    {
        characterComponent.onSwitchState += OnSwitchController;
    }
    protected virtual void DeInitializeEvent()
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

    private void OnDestroy()
    {
        DeInitializeEvent();
    }
}
