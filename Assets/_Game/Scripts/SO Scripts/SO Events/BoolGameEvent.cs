using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Game Event/Bool")]
public class BoolGameEvent : GameEvent<bool>
{
    [SerializeField] private bool state = true;
    public bool State { get => state; }

    public override void Raise(bool t)
    {
        base.Raise(t);
        state = t;
    }
}
