using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DraggableBehaviour : MonoBehaviour, IDraggable
{
    // Variables
    [Header("Movement")]
    [SerializeField] private float m_dampSmoothTime = 0.1f;

    [Header("Drag End")]
    [SerializeField] private bool m_resetPositionOnDragEnd = true;
    // [SerializeField] private bool m_useLocalPosition = true;
    [SerializeField] private float m_stopThreshold = 0.05f;

    [Header("Events")]
    [SerializeField] private UnityEvent<Vector2> m_onDragStart;
    [SerializeField] private UnityEvent<Vector2> m_onDragEnd;

    private Coroutine m_dragBehaviour;

    private bool m_dragging;

    private Vector2 m_startPosition;
    private Vector2 m_targetPosition;
    private Vector2 m_dampVelocity;

    // Methods
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected virtual void Start()
    {
        // m_startPosition = m_useLocalPosition ? transform.localPosition : transform.position;
        m_startPosition = transform.position;
    }

    public virtual void OnDragStart(Vector2 worldPosition)
    {
        m_dragging = true;
        m_targetPosition = worldPosition;
        m_onDragStart?.Invoke(worldPosition);

        this.RestartCoroutine(ref m_dragBehaviour, DoMovement());
    }

    public virtual void OnDragMove(Vector2 worldPosition)
    {
        m_targetPosition = worldPosition;
    }

    public virtual void OnDragRelease(Vector2 worldPosition)
    {
        m_onDragEnd?.Invoke(worldPosition);
        m_dragging = false;

        if (m_resetPositionOnDragEnd)
            m_targetPosition = m_startPosition;

        else
            m_targetPosition = worldPosition;
    }

    // Coroutines
    private IEnumerator DoMovement()
    {
        while (true)
        {
            transform.position = Vector2.SmoothDamp(transform.position, m_targetPosition, ref m_dampVelocity, m_dampSmoothTime);

            if (!m_dragging && Vector2.Distance(transform.position, m_targetPosition) < m_stopThreshold)
                break;

            else
                yield return null;
        }

        transform.position = m_targetPosition;
    }
}
