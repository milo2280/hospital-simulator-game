using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IGameEventListener<T>
{
    void OnEventRaised(T t);
}

public abstract class GameEventListener<TParam, TGameEvent, TUnityEvent> : MonoBehaviour, IGameEventListener<TParam> 
    where TGameEvent : GameEvent<TParam>
    where TUnityEvent : UnityEvent<TParam>
{
    [SerializeField] public TGameEvent Event;
    [SerializeField] public TUnityEvent Response;

    protected void OnEnable()
    {
        Event.RegisterListener(this);
    }

    protected void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(TParam t)
    {
        Response.Invoke(t);
    }
}
