using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialoguePictureController : DialogueComponent
    {
        // Variables
        [Header("Picture Settings")]
        [SerializeField] private Image m_pictureRenderer;

        // Methods
        public override Coroutine Execute(Dialogue dialogue)
        {
            return StartCoroutine(DoShowPicture(dialogue));
        }

        // Coroutines
        private IEnumerator DoShowPicture(Dialogue dialogue)
        {
            m_pictureRenderer.sprite = dialogue.Picture;

            dialogue.TryPlayVoice(CommonValues.DialogueSource);
            dialogue.TryExecuteStartEvent();

            while (CurrentState != State.Opened)
                yield return null;

            InputState = DialogueInputState.None;

            while (true)
            {
                if (InputState == DialogueInputState.None)
                {
                    if (dialogue.HasVoice && dialogue.WaitUntilVoiceEnd)
                    {
                        while (CommonValues.DialogueSource.isPlaying)
                            yield return null;
                    }

                    if (dialogue.HoldTime > 0)
                        yield return new WaitForSeconds(dialogue.HoldTime);

                    InputState = DialogueInputState.Waiting;
                }

                if (dialogue.TriggerNextOnTime > 0)
                {
                    yield return new WaitForSeconds(dialogue.TriggerNextOnTime);
                    break;
                }

                else
                {
                    if (InputState == DialogueInputState.Pressed)
                        break;
                }

                yield return null;
            }

            dialogue.TryExecuteEndEvent();
        }
    }
}