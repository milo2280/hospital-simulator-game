
public enum Scene
{
    Connect,
    Game,
    Test,
}

#region UI
public enum UIPanel
{
    None,
    Connecting,
    CreateJoinRoom,
    Game,
    PlayerInformation,
}

public enum UIPopup
{
    None,
    AdditionalInformation,
    BaseInformation,
    CreateRoom,
    JoinRoom,
    Settings,
    Talk,
}
#endregion

#region Player Information
public enum PlayerRole
{
    None,
    Patient,
    Doctor,
    PatientRelative,
}

public enum Gender
{
    None,
    Male,
    Female,
    Other,
}

public enum MaritalStatus
{
    None,
    Single,
    Dating,
    Married,
    Divorced,
}

public enum Interest
{
    Game,
    Film,
    Sport,
    Music,
    Cooking,
    Finance,
    Fashion,
    Technology,
}
#endregion
