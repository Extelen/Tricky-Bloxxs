using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

/// <summary>
/// Not working, probably.
/// </summary>

public class TextAnimation : MonoBehaviour
{
    // Structs
    private struct CharacterAnimation
    {
        // Variables
        private VertSettings m_settings;
        private TMP_CharacterInfo m_charInfo;
        private Vector3[] m_savedVertices;

        // Constructor
        public CharacterAnimation(VertSettings settings, TMP_CharacterInfo charInfo, TMP_MeshInfo meshInfo)
        {
            m_settings = settings;
            m_charInfo = charInfo;

            m_savedVertices = new Vector3[4];

            for (int i = 0; i < m_savedVertices.Length; i++)
            {
                int vertIndex = m_charInfo.vertexIndex + i;
                m_savedVertices[i] = meshInfo.vertices[vertIndex];
            }
        }

        // Methods
        public void Update(float value, ref TMP_MeshInfo meshInfo)
        {
            Vector3 vertOffset = Vector3.zero;
            vertOffset.y = Mathf.Lerp(m_settings.offset, 0, value);

            for (int i = 0; i < 4; i++)
            {
                int vertIndex = m_charInfo.vertexIndex + i;

                Color color = meshInfo.colors32[vertIndex];
                color.a = value;

                meshInfo.colors32[vertIndex] = color;
                meshInfo.vertices[vertIndex] = m_savedVertices[i] + vertOffset;
            }
        }
    }

    [System.Serializable]
    public class VertSettings
    {
        // Variables
        [SerializeField] private float m_offset = 20;

        // Properties
        public float offset => m_offset;
    }

    // Variables
    [Header("Settings")]
    [SerializeField] private VertSettings m_vertSettings;
    [SerializeField] private float m_speed = 2;
    [SerializeField] private float m_delay = 0.2f;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI m_textRenderer;

    public bool OnAnimation { get; private set; }

    // Methods
    public void UseRendererText()
    {
        Set(m_textRenderer.text);
    }

    public void Set(string text)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(DoAnimation(text));
    }

    // Coroutines
    private IEnumerator DoAnimation(string text)
    {
        OnAnimation = true;
        m_textRenderer.text = text;
        m_textRenderer.ForceMeshUpdate();

        var textInfo = m_textRenderer.textInfo;

        float time = 0;
        int characterCount = textInfo.characterCount;

        CharacterAnimation[] characterAnimations = new CharacterAnimation[characterCount];

        for (int i = 0; i < characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            int materialIndex = charInfo.materialReferenceIndex;
            var meshInfo = textInfo.meshInfo[materialIndex];

            characterAnimations[i] = new CharacterAnimation(m_vertSettings, charInfo, meshInfo);
        }

        while (true)
        {
            time += Time.unscaledDeltaTime * m_speed;

            for (int i = 0; i < characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                var meshInfo = textInfo.meshInfo[materialIndex];

                float value = time - (i * m_delay);
                value = Mathf.Clamp01(value);

                characterAnimations[i].Update(value, ref meshInfo);

                textInfo.meshInfo[materialIndex].mesh.vertices = meshInfo.vertices;
                textInfo.meshInfo[materialIndex].mesh.colors32 = meshInfo.colors32;

                if (i == characterCount - 1 && value == 1)
                {
                    OnAnimation = false;
                    yield break;
                }
            }

            for (int i = 0; i < characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                var meshInfo = textInfo.meshInfo[materialIndex];

                characterAnimations[i].Update(1, ref meshInfo);

                textInfo.meshInfo[materialIndex].mesh.vertices = meshInfo.vertices;
                textInfo.meshInfo[materialIndex].mesh.colors32 = meshInfo.colors32;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                m_textRenderer.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            m_textRenderer.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return null;
        }
    }
}
