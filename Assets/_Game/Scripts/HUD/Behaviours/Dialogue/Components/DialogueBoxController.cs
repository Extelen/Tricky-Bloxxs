using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueBoxController : DialogueComponent
    {
        // Variables
        [Header("Text Settings")]
        [SerializeField] private TextMeshProUGUI m_titleRenderer;
        [SerializeField] private TextMeshProUGUI m_dialogueRenderer;
        [SerializeField] private float m_timeBetweenCharacters = 0.1f;

        // Methods
        public override Coroutine Execute(Dialogue dialogue)
        {
            return StartCoroutine(DoBehaviour(dialogue));
        }

        private void HandleTalk(char currentCharacter, bool hasVoice)
        {
            // Do talk animation
            bool talk = "aeiouáéíóú".Contains(char.ToLower(currentCharacter));
            Sprite sprite;

            if (talk)
                sprite = CommonValues.CharacterTalking;

            else
                sprite = CommonValues.CharacterIdle;

            CommonValues.CharacterRenderer.sprite = sprite;

            // Play sfx on every vocal
            if (talk && !hasVoice)
                CommonValues.DialogueSource.PlayOneShot(CommonValues.CharSound);
        }

        private bool SkipRichText(char character, ref bool skippingRich)
        {
            // Skip rich text
            if (character == '<')
            {
                skippingRich = true;
                return true;
            }

            if (skippingRich)
            {
                if (character == '>')
                    skippingRich = false;

                return true;
            }

            return false;
        }

        // Coroutines
        private IEnumerator DoBehaviour(Dialogue dialogue)
        {
            m_titleRenderer.text = "";
            m_dialogueRenderer.text = "";

            // Wait until panel is fully opened.
            while (CurrentState == State.Opening)
                yield return null;

            dialogue.TryExecuteStartEvent();
            dialogue.TryPlayVoice(CommonValues.DialogueSource);

            // Do name animation.
            yield return StartCoroutine(DoTitleAnimation(dialogue.Title));

            // Do text animation.
            yield return StartCoroutine(DoTextAnimation(dialogue));

            CommonValues.CharacterRenderer.sprite = CommonValues.CharacterIdle;

            // Wait until voice ends.
            if (dialogue.HasVoice && dialogue.WaitUntilVoiceEnd)
            {
                while (CommonValues.DialogueSource.isPlaying)
                    yield return null;
            }

            if (dialogue.HoldTime > 0)
                yield return new WaitForSeconds(dialogue.HoldTime);

            if (dialogue.TriggerNextOnTime > 0)
                yield return new WaitForSeconds(dialogue.TriggerNextOnTime);

            else
            {
                InputState = DialogueInputState.Waiting;

                while (InputState == DialogueInputState.Waiting)
                    yield return null;
            }

            dialogue.TryExecuteEndEvent();
            InputState = DialogueInputState.None;
        }

        private string HideSprites(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '<')
                {
                    int tries = 0;
                    int index = 0;

                    string search = "sprite";

                    while (true)
                    {
                        i++;

                        if (index < search.Length)
                        {
                            if (char.ToLower(text[i]) != search[index])
                                break;

                            index++;
                            continue;
                        }

                        if (text[i] == '>')
                        {
                            text = text.Insert(i, " color=#FFFFFF00");
                            break;
                        }

                        tries++;
                        if (tries == 100)
                            break;
                    }
                }
            }

            return text;
        }

        private void TryShowSprite(ref string text, int currentIndex)
        {
            int currentCharIndex = currentIndex;

            if (text[currentCharIndex] == '<')
            {
                int index = 0;
                string search = "sprite";

                while (true)
                {
                    currentCharIndex++;

                    if (index < search.Length)
                    {
                        if (char.ToLower(text[currentCharIndex]) != search[index])
                            break;

                        index++;
                        continue;
                    }

                    text = text.Replace("#FFFFFF00", "#FFFFFFFF");
                    break;
                }
            }
        }

        private IEnumerator DoTitleAnimation(string text)
        {
            WaitForSeconds waitTime = new WaitForSeconds(m_timeBetweenCharacters);
            bool skippingRichText = false;

            text = HideSprites(text);

            for (int i = 0; i < text.Length; i++)
            {
                if (SkipRichText(text[i], ref skippingRichText)) continue;

                // Add opacity to the next texts
                string processedText = text.Insert(i, "<color=#ffffff00>");
                m_titleRenderer.text = processedText;

                if (m_timeBetweenCharacters <= 0)
                    yield return null;

                else
                    yield return waitTime;
            }

            m_titleRenderer.text = text;
        }

        private IEnumerator DoTextAnimation(Dialogue dialogue)
        {
            WaitForSeconds waitTime = new WaitForSeconds(m_timeBetweenCharacters);
            bool skippingRichText = false;
            string text = dialogue.Text;
            text = HideSprites(text);

            InputState = DialogueInputState.CanSkip;

            for (int i = 0; i < text.Length; i++)
            {
                if (InputState == DialogueInputState.Skipped)
                    break;

                TryShowSprite(ref text, i);
                if (SkipRichText(text[i], ref skippingRichText)) continue;

                // Add opacity to the next texts
                string processedText = text.Insert(i, "<color=#ffffff00>");
                m_dialogueRenderer.text = processedText;

                HandleTalk(text[i], dialogue.HasVoice);

                if (m_timeBetweenCharacters <= 0)
                    yield return null;

                else
                    yield return waitTime;
            }

            m_dialogueRenderer.text = dialogue.Text;
        }
    }
}