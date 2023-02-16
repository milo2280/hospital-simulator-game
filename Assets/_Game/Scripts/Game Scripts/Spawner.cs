using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Spawner : MonoBehaviour
{
    public static PlayerManager PM;
    [SerializeField] private GameObject playerPack;

    private void Start()
    {
        PM = PhotonNetwork.Instantiate(playerPack.name, transform.position, Quaternion.identity, 0, 
            DataManager.Instance.data.info.ToObjs()).GetComponentInChildren<PlayerManager>();
    }
}
