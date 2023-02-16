using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent<T> : ScriptableObject
{
    protected List<IGameEventListener<T>> listeners = new List<IGameEventListener<T>>();

    public virtual void Raise(T t)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(t);
        }
    }

    public void RegisterListener(IGameEventListener<T> listener)
    {
        if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    public void UnregisterListener(IGameEventListener<T> listener)
    {
        if (listeners.Contains(listener)) listeners.Remove(listener);
    }
}
