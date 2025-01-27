using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class ReloadCurrentScene : MonoBehaviour
{
    // Variables
    [SerializeField] private bool m_showLoadingScreen = true;

    // Methods
    /// <summary>
    /// Switch to the other scene.
    /// </summary>
    public void Load()
    {
        int index = UnitySceneManager.GetActiveScene().buildIndex;
        SceneManager.Instance.LoadMainScene(index, m_showLoadingScreen);
    }
}