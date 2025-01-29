using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TouchManager : MonoBehaviour
{

    private PlayerInput playerInput;

    private InputAction touchPressAction;
    private InputAction touchPositionAction;
    private Vector2 previousTouchPosition;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private float touchDuration;
    private bool isTouching;
    private bool isDragging;

    [SerializeField] private float dragThreshold = 0.5f; // Time in seconds, if drag ends before dragThreshold it's considered a tap
    [SerializeField] private float swipeDistanceThreshold = 50f; // Distance a drag must exceed to be considered a swipe
    [SerializeField] private float swipeTimeThreshold = 0.5f; // Time in seconds a swipe must be completed in to be considered a swipe

    public UnityEvent<Vector2> OnTap;
    public UnityEvent<Vector2> OnDrag; // Vector2 is position relative to the previous frame, not absolute
    public UnityEvent<SwipeDirection> OnSwipe;

    public enum SwipeDirection { Up, Down, Left, Right }



    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        touchPressAction = playerInput.actions["Pointer Press"];
        touchPositionAction = playerInput.actions["Pointer Position"];
    }

    private void OnEnable()
    {
        touchPressAction.Enable();
        touchPressAction.performed += TouchBegin;
        touchPressAction.canceled += TouchEnd;
        touchPositionAction.Enable();
    }

    private void OnDisable()
    {
        touchPressAction.Disable();
        touchPressAction.performed -= TouchBegin;
        touchPressAction.canceled -= TouchEnd;
        touchPositionAction.Disable();
    }

    void Update()
    {
        if (isTouching)
        {
            currentTouchPosition = touchPositionAction.ReadValue<Vector2>();
            touchDuration += Time.deltaTime;

            if (isDragging)
            {
                Vector2 touchDelta = currentTouchPosition - previousTouchPosition;
                OnDrag?.Invoke(touchDelta);
                //Debug.Log("Drag " + touchDelta);
            }
            else if (touchDuration >= dragThreshold && Vector2.Distance(startTouchPosition, currentTouchPosition) > 0f)
            {
                isDragging = true;
            }

            previousTouchPosition = currentTouchPosition;
        }
    }

    private void TouchBegin(InputAction.CallbackContext context)
    {
        if (isTouching) return;

        isTouching = true;
        isDragging = false;
        touchDuration = 0f;
        startTouchPosition = touchPositionAction.ReadValue<Vector2>();
        previousTouchPosition = startTouchPosition;
    }

    private void TouchEnd(InputAction.CallbackContext context)
    {
        if (!isTouching) return;

        isTouching = false;

        if (isDragging)
        {
            isDragging = false;
            //return;
        }

        currentTouchPosition = touchPositionAction.ReadValue<Vector2>();

        // Swipe
        if (Vector2.Distance(startTouchPosition, currentTouchPosition) >= swipeDistanceThreshold && touchDuration <= swipeTimeThreshold)
        {
            OnSwipe?.Invoke(HandleSwipe(startTouchPosition, currentTouchPosition));
            //Debug.Log("Swipe " + startTouchPosition + " " + currentTouchPosition);
        }
        // Tap
        else if (touchDuration < dragThreshold)
        {
            OnTap?.Invoke(currentTouchPosition);
            //Debug.Log("Tap " + currentTouchPosition);
        }
    }

    private SwipeDirection HandleSwipe(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            return direction.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }
    }
}
