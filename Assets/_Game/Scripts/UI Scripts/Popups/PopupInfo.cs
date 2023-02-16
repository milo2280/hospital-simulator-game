using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PopupInfo : PopupBase
{
    [SerializeField] private TMP_InputField nameIF;
    [SerializeField] private TMP_Dropdown roleDropdown;

    public override void Show()
    {
        base.Show();
        nameIF.text = DataManager.Instance.data.info.name;
        roleDropdown.value = (int)DataManager.Instance.data.info.role - 1;
    }

    #region UI Callbacks
    public void OnNameChanged()
    {
        DataManager.Instance.data.info.name = nameIF.text;
    }

    public void OnRoleChanged()
    {
        DataManager.Instance.data.info.role = (PlayerRole)(roleDropdown.value + 1);
    }

    public void ButtonNext()
    {
        Close();
        UIM.Instance.ShowDetail();
    }
    #endregion
}
