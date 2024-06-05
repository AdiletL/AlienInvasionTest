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
        platform = new Desktop();
#elif UNITY_ANDROID
        platform = new Mobile();
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

        public float swipeRange = 50.0f;
        public float initialSwipeThreshold = 20.0f;

        public abstract void SwipeCheck();
    }

    public class Mobile : Platform
    {
        public override void SwipeCheck()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPosition = touch.position;
                        break;

                    case TouchPhase.Moved:
                        currentPosition = touch.position;
                        Vector2 distance = currentPosition - startPosition;

                        if (distance.magnitude >= initialSwipeThreshold)
                        {
                            if (distance.magnitude >= swipeRange)
                            {
                                direction = new Vector3(distance.x, 0, distance.y).normalized;
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                        direction = Vector3.zero;
                        break;
                }
            }
        }
    }

    public class Desktop : Platform
    {
        public override void SwipeCheck()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                currentPosition = Input.mousePosition;
                Vector2 distance = currentPosition - startPosition;
                if (distance.magnitude >= initialSwipeThreshold)
                {
                    if (distance.magnitude >= swipeRange)
                    {
                        direction = new Vector3(distance.x, 0, distance.y).normalized;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                direction = Vector3.zero;
            }
        }
    }
}
