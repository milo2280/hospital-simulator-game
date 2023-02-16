using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class PhotonManager : PersistentPUNSingleton<PhotonManager>
{
    [SerializeField] private List<RoomInfo> listRoomInfo;

    public UnityAction<List<RoomInfo>> UpdateRoomListAction;

    public List<RoomInfo> ListRoomInfo { get => listRoomInfo; }

    private void Start()
    {
        Debug.Log("Connecting to master");
        PhotonNetwork.ConnectUsingSettings();
    }

    #region Callbacks
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.InLobby == true) return;

        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        UIM.Instance.panelConnect.OnConnected();
        UIM.Instance.ShowInfo();
    }

    public override void OnJoinedRoom()
    {
        GameManager.Instance.PhotonLoadLevel(Scene.Game);
        //GameManager.Instance.PhotonLoadLevel(Scene.Test);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed");
        Debug.Log("Code: " + returnCode + ". " + message);

        // Handle join room failed
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left room");
        GameManager.Instance.OnLeftRoom(Scene.Connect);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = roomList.Count - 1; i >= 0; i--)
        {
            if (roomList[i].RemovedFromList == true) roomList.RemoveAt(i);
        }

        listRoomInfo = roomList;
        UpdateRoomListAction?.Invoke(listRoomInfo);
    }
    #endregion

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}

