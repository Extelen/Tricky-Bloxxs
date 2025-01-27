using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public enum DialogueInputState { None, CanSkip, Skipped, Waiting, Pressed };
    public abstract class DialogueComponent : AnimablePanel
    {
        // Variables
        protected DialogueCommonValues CommonValues { get; private set; }
        protected DialogueInputState InputState { get; set; }

        // Methods
        public virtual void Initialize(DialogueCommonValues values)
        {
            CommonValues = values;
        }

        public void HandleInputTap()
        {
            switch (InputState)
            {
                case DialogueInputState.CanSkip:
                    InputState = DialogueInputState.Skipped;
                    break;

                case DialogueInputState.Waiting:
                    InputState = DialogueInputState.Pressed;
                    break;
            }
        }

        public abstract Coroutine Execute(Dialogue dialogue);
    }
}