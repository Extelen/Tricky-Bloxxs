using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Eflatun.SceneReference;

public class SwitchScene : MonoBehaviour
{
    // Variables
    [Header("Settings")]
    [SerializeField] private SceneReference m_scene;
    [SerializeField] private bool m_showLoading = true;

    // Methods
    /// <summary>
    /// Switch to the other scene.
    /// </summary>
    public void Load()
    {
        SceneManager.Instance.LoadMainScene(m_scene.BuildIndex, m_showLoading);
    }
}