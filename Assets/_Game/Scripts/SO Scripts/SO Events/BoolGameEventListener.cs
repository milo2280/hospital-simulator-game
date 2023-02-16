using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameEventBool : GameEvent<bool> { }
[System.Serializable]
public class UnityEventBool : UnityEvent<bool> { }

public class BoolGameEventListener : GameEventListener<bool, GameEventBool, UnityEventBool>
{

}
