using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IOBase : MonoBehaviour
{
    // Properties
    public abstract float AnimationTime { get; }

    // Methods
    public void Open() => Open(false, null);
    public void Open(bool instant) => Open(instant, null);
    public void Open(Callback OnEnd) => Open(false, OnEnd);
    public abstract void Open(bool instant, Callback OnEnd);

    public void Close() => Close(false, null);
    public void Close(bool instant) => Close(instant, null);
    public void Close(Callback OnEnd) => Close(false, OnEnd);
    public abstract void Close(bool instant, Callback OnEnd);

    // Delegates
    public delegate void Callback();

}
