using UnityEngine;

using Photon.Pun;

public class TestConnection : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected To Master");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }
}
