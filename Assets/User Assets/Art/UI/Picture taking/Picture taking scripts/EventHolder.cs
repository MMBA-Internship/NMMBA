using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventHolder", menuName = "Scriptable Objects/EventHolder")]
public class EventHolder : ScriptableObject
{
    private event Action OnEventTriggered;

    public void Invoke()
    {
        OnEventTriggered?.Invoke();
    }

    public void Subscribe(Action listener)
    {
        OnEventTriggered += listener;
    }

    public void Unsubscribe(Action listener)
    {
        OnEventTriggered -= listener;
    }
}
