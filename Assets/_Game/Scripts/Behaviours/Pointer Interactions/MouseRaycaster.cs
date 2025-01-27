using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseRaycaster : MonoBehaviour
{
    // Enums
    private enum State { None, Pressing, Released }

    // Variables
    [Header("Input")]
    [SerializeField] private InputActionReference m_tapAction;
    [SerializeField] private InputActionReference m_positionAction;

    private Vector2 m_startMousePos;
    private State m_state;

    private Collider2D[] m_draggables;
    private IDraggable m_currentDraggable;

    private Collider2D[] m_hovers;
    private IHoverHandler m_currentHover;

    private Camera m_camera;

    // Methods
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        m_hovers = new Collider2D[5];
        m_draggables = new Collider2D[5];

        m_camera = Camera.main;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        m_positionAction.action.Enable();
        m_positionAction.action.started += OnPointerMove;
        m_positionAction.action.performed += OnPointerMove;
        m_positionAction.action.canceled += OnPointerMove;

        m_tapAction.action.Enable();
        m_tapAction.action.started += OnTap;
        m_tapAction.action.canceled += OnTap;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        m_positionAction.action.Disable();
        m_positionAction.action.started -= OnPointerMove;
        m_positionAction.action.performed -= OnPointerMove;
        m_positionAction.action.canceled -= OnPointerMove;

        m_tapAction.action.Disable();
        m_tapAction.action.started -= OnTap;
        m_tapAction.action.canceled -= OnTap;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        // Tap Registration
        if (m_state == State.Pressing)
        {
            Vector2 mousePosition = m_positionAction.action.ReadValue<Vector2>();
            if (Vector2.Distance(mousePosition, m_startMousePos) > 40)
            {
                m_state = State.None;
            }
        }

        // Tap Confirmation
        if (m_state == State.Released)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                m_state = State.None;
                return;
            }

            Vector2 worldPosition = GetWorldMousePosition();
            Collider2D[] colliders = Physics2D.OverlapPointAll(worldPosition);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out ITapEventHandler handler))
                {
                    handler.OnTap();
                    break;
                }
            }

            m_state = State.None;
        }
    }

    /// <summary>
    /// Handle tap.
    /// </summary>
    private void OnTap(InputAction.CallbackContext context)
    {
        Vector2 mousePos = m_positionAction.action.ReadValue<Vector2>();
        Vector2 worldPosition = GetWorldMousePosition(mousePos);

        if (context.started)
        {
            if (TryGetDraggableOnPoint(worldPosition, out m_currentDraggable))
                m_currentDraggable.OnDragStart(worldPosition);

            m_startMousePos = mousePos;
            m_state = State.Pressing;
        }

        else
        {
            if (m_currentDraggable != null)
            {
                m_currentDraggable.OnDragRelease(worldPosition);
                m_currentDraggable = null;
            }

            if (m_state == State.Pressing)
            {
                m_state = State.Released;
            }
        }

    }

    private void OnPointerMove(InputAction.CallbackContext context)
    {
        Vector2 worldPosition = GetWorldMousePosition();
        int count = Physics2D.OverlapPointNonAlloc(worldPosition, m_hovers);

        if (m_currentDraggable != null)
            m_currentDraggable.OnDragMove(worldPosition);

        if (m_currentHover != null)
        {
            bool hoverExit = true;

            for (int i = 0; i < count; i++)
            {
                Collider2D collider = m_hovers[i];
                if (collider.TryGetComponent(out IHoverHandler handler))
                {
                    hoverExit = false;
                    break;
                }
            }

            if (hoverExit)
            {
                m_currentHover.OnHoverExit();
                m_currentHover = null;
            }
        }

        for (int i = 0; i < count; i++)
        {
            Collider2D collider = m_hovers[i];
            if (collider.TryGetComponent(out IHoverHandler handler))
            {
                if (handler == m_currentHover)
                    break;

                handler.OnHoverEnter();
                m_currentHover = handler;
                break;
            }
        }
    }

    private bool TryGetDraggableOnPoint(Vector2 screenPosition, out IDraggable draggable)
    {
        int count = Physics2D.OverlapPointNonAlloc(screenPosition, m_draggables);
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (m_hovers[i].TryGetComponent(out draggable))
                    return true;
            }
        }

        draggable = null;
        return false;
    }

    private Vector2 GetWorldMousePosition()
    {
        Vector2 mousePosition = m_positionAction.action.ReadValue<Vector2>();
        return m_camera.ScreenToWorldPoint(mousePosition);
    }

    private Vector2 GetWorldMousePosition(Vector2 screenPos)
    {
        return m_camera.ScreenToWorldPoint(screenPos);
    }
}
