using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PopupSettings : PopupBase
{
    #region UI Callbacks
    public void ButtonLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void ButtonBack()
    {
        Close();
        Spawner.PM.input.SetActiveLook(true);
        UIM.Instance.panelGame.objCrosshair.SetActive(true);
    }
    #endregion
}
