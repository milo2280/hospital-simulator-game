using UnityEngine;

public static class Constants
{
    // material
    public const string Emission = "_EMISSION";

    // SO game events path
    public const string BoolGameEventPath = "Assets/_Game/Resources/Game Events/Bool/";

    // Layer
    //public const string PlayerLayer = "Player";
    public static readonly LayerMask InteractableLayer = LayerMask.NameToLayer("Interactable");
}
