using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapRotator : TapEventBehaviour
{
    // Variables
    [Header("Rotation")]
    [SerializeField] private float m_rotationAddition;
    [SerializeField] private float m_rotationTime;
    [SerializeField] private Easing.Type m_rotationEasing;
    [SerializeField] private bool m_lockUntilEnd = true;

    private bool m_isPlaying;
    private float m_rawRotation;
    private Coroutine m_rotationBehaviour;

    // Methods
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        m_rawRotation = transform.eulerAngles.z;
    }

    public override void OnTap()
    {
        if (!CheckTap())
            return;

        base.OnTap();
        this.RestartCoroutine(ref m_rotationBehaviour, DoRotate());
    }

    protected virtual bool CheckTap()
    {
        if (m_lockUntilEnd && m_isPlaying)
            return false;

        return true;
    }

    // Coroutines
    private IEnumerator DoRotate()
    {
        m_isPlaying = true;
        float a = transform.eulerAngles.z;
        float b = m_rawRotation + m_rotationAddition;

        m_rawRotation = b;

        for (float i = 0; i < m_rotationTime; i += Time.deltaTime)
        {
            float t = Easing.Evaluate(i / m_rotationTime, m_rotationEasing);
            transform.eulerAngles = Vector3.forward * Mathf.Lerp(a, b, t);
            yield return null;
        }

        transform.eulerAngles = Vector3.forward * b;
        m_isPlaying = false;
    }
}
