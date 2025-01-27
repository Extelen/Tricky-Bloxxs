using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

using Eflatun.SceneReference;

public class SceneManager : Singleton<SceneManager>
{
    public enum State { None, Loading, Loaded, Pressed, Faded }

    // Events
    public static event LoadCallback OnStateChange;

    // Properties
    public static bool IsLoading { get; private set; }
    protected override bool Persistent => true;

    // Variables
    [Header("Settings")]
    [SerializeField] private float m_holdTime = 0.5f;

    [Header("References")]
    [SerializeField] private LoadingScreen m_loadingScreen = null;

    // Methods
    /// <summary>
    /// Remove all the current opened scenes and fade to another one using fade settings.
    /// </summary>
    public void LoadMainScene(SceneReference scene, bool showLoading = true)
    {
        if (IsLoading) return;
        StartCoroutine(DoLoadMainSceneAsync(scene.BuildIndex, showLoading));
    }

    /// <summary>
    /// Remove all the current opened scenes and fade to another one using fade settings.
    /// </summary>
    public void LoadMainScene(int index, bool showLoading = true)
    {
        if (IsLoading) return;
        StartCoroutine(DoLoadMainSceneAsync(index, showLoading));
    }

    private IEnumerator DoLoadMainSceneAsync(int index, bool showLoading)
    {
        OnStateChange?.Invoke(State.Loading);
        IsLoading = true;

        if (showLoading)
            yield return m_loadingScreen.Show();

        // Start async operations.
        AsyncOperation asyncOperation = UnitySceneManager.LoadSceneAsync(index);

        if (showLoading)
            yield return new WaitForSeconds(m_holdTime);

        while (!asyncOperation.isDone)
            yield return null;

        OnStateChange?.Invoke(State.Loaded);

        if (showLoading)
            yield return m_loadingScreen.Hide();

        OnStateChange?.Invoke(State.Faded);
        IsLoading = false;
    }

    // Delegates
    public delegate void LoadCallback(State state);
}
