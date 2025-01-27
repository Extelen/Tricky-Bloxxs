using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace DialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        // Variables
        [Header("Common Settings")]
        [SerializeField] private Openable m_baseOpenable;
        [SerializeField] private DialogueCommonValues m_commonValues;
        [SerializeField] private PanelsManager m_panelsManager;
        [Space]
        [SerializeField] private bool m_reopenOnNext;
        [SerializeField] private float m_timeToReopen = 0.25f;

        [Header("Components")]
        [SerializeField] private DialogueBoxController m_boxController;
        [SerializeField] private DialoguePictureController m_pictureController;

        [Header("Input")]
        [SerializeField] private InputActionReference m_onNextDialogue;

        // Properties
        private Coroutine m_currentCoroutine;
        private DialogueComponent m_currentBehaviour;

        // Methods
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        public void Initialize()
        {
            gameObject.SetActive(false);
            m_boxController.Initialize(m_commonValues);
            m_pictureController.Initialize(m_commonValues);

            m_baseOpenable.Set(false);
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            m_onNextDialogue.action.Enable();
            m_onNextDialogue.action.canceled += Next;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            m_onNextDialogue.action.Disable();
            m_onNextDialogue.action.canceled -= Next;
        }

        private void Next(InputAction.CallbackContext context)
        {
            if (m_currentBehaviour == null) return;
            m_currentBehaviour.HandleInputTap();
        }

        public void SetDialogue(Dialogue[] dialogues, UnityEvent onEnd)
        {
            gameObject.SetActive(true);

            m_baseOpenable.Set(false);

            if (m_currentCoroutine != null) StopCoroutine(m_currentCoroutine);
            m_currentCoroutine = StartCoroutine(DoBehaviour(dialogues, onEnd));
        }

        private Coroutine HandleDialogue(Dialogue dialogue)
        {
            if (dialogue.ShowPicture)
            {
                m_currentBehaviour = m_pictureController;
                return m_pictureController.Execute(dialogue);
            }
            else
            {
                m_currentBehaviour = m_boxController;
                return m_boxController.Execute(dialogue);
            }
        }

        private void HandleOpen(Dialogue current, ref State currentState)
        {
            if (currentState == State.None || m_reopenOnNext)
            {
                if (current.ShowPicture)
                {
                    m_panelsManager.SwitchPanel(m_pictureController.Identifier);
                    currentState = State.Picture;
                }
                else
                {
                    m_panelsManager.SwitchPanel(m_boxController.Identifier);
                    currentState = State.Text;
                }
            }
        }

        private void HandleClose(Dialogue current, Dialogue next, ref State currentState)
        {
            if (m_reopenOnNext)
                m_panelsManager.SwitchPanel("");

            else
            {
                if (next == null)
                    m_panelsManager.SwitchPanel("");

                else
                {
                    if (currentState == State.Text && next.ShowPicture)
                    {
                        m_panelsManager.SwitchPanel("");
                        currentState = State.None;
                    }

                    else if (currentState == State.Picture && !next.ShowPicture)
                    {
                        m_panelsManager.SwitchPanel("");
                        currentState = State.None;
                    }
                }
            }
        }

        // Coroutines
        enum State { None, Text, Picture }
        private IEnumerator DoBehaviour(Dialogue[] dialogues, UnityEvent onEnd)
        {
            yield return m_baseOpenable.Open();

            State currentState = State.None;

            for (int i = 0; i < dialogues.Length; i++)
            {
                Dialogue current = dialogues[i];
                Dialogue next = i == dialogues.Length - 1 ? null : dialogues[i + 1];

                HandleOpen(current, ref currentState);
                yield return HandleDialogue(current);

                m_commonValues.DialogueSource.Stop();

                HandleClose(current, next, ref currentState);

                if (m_reopenOnNext)
                    yield return new WaitForSeconds(m_timeToReopen);
            }

            yield return m_baseOpenable.Close();
            onEnd?.Invoke();
        }
    }
}