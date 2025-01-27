using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enums
public enum SFX
{
    ButtonPress,
    InventoryOpen,
    InventoryClose,
    Transition,
    Correct,
    Incorrect,
}

public class AudioManager : Singleton<AudioManager>
{
    // Properties
    protected override bool Persistent => true;

    // Structs
    [System.Serializable]
    private struct Clip
    {
        // Variables
        [SerializeField] private SFX m_identifier;
        [SerializeField] private AudioClip m_clip;

        // Methods
        public void TryPlay(SFX identifier, AudioSource source)
        {
            if (identifier == m_identifier)
            {
                source.PlayOneShot(m_clip);
            }
        }
    }

    // Variables
    [Header("Settings")]
    [SerializeField] private Clip[] m_sfxs;

    [Header("References")]
    [SerializeField] private AudioSource m_sfxSource;

    // Methods
    public void PlaySFX(SFX identifier)
    {
        foreach (var sfx in m_sfxs)
        {
            sfx.TryPlay(identifier, m_sfxSource);
        }
    }
}
