using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IControl
{
    public event Action<Vector3> OnSwipe;
    public IController iController { get; set; }

    private PlayerMainController playerComponent;
    private Platform platform;

    private bool isEnabled = true;

    public void Initialize(IController controller)
    {
        playerComponent = controller as PlayerMainController;
        if (playerComponent == null)
        {
            enabled = false;
            return;
        }

#if UNITY_EDITOR
        var mobile = new Mobile();
        mobile.joystick = FindObjectOfType<Joystick>();
        platform = mobile;
        //platform = mobilePlatform;
#elif UNITY_ANDROID
        var mobile = new Mobile();
        mobile.joystick = FindObjectOfType<Joystick>();
        platform = mobile;
#endif

        var playerConfig = (SO_PlayerConfig)playerComponent.so_CharacterConfig;
    }

    private void Start()
    {
        InitializeEvent();
    }
    private void InitializeEvent()
    {
       
    }
    private void DeInitializeEvent()
    {
       
    }


    private void Update()
    {
        if (!isEnabled) return;

        platform.SwipeCheck();
        OnSwipe?.Invoke(platform.direction);
    }

    private void OnDestroy()
    {
        DeInitializeEvent();
    }

    public abstract class Platform
    {
        public Vector2 startPosition { get; protected set; }
        public Vector2 currentPosition { get; protected set; }
        public Vector3 direction { get; protected set; }

        public float swipeRange { get; protected set; } = 50.0f;
        public float initialSwipeThreshold { get; protected set; } = 20.0f;

        public abstract void SwipeCheck();
    }

    [System.Serializable]
    public class Mobile : Platform
    {
        [HideInInspector] public Joystick joystick;

        public override void SwipeCheck()
        {
            direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        }
    }

    public class Desktop : Platform
    {
        public override void SwipeCheck()
        {
            
        }
    }
}
