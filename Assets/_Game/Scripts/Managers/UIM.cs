using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIM : Singleton<UIM>
{
    [Header("Dependencies")]
    [SerializeField] private GameObject blocker;
    [SerializeField] private Transform panelCanvasTF;
    [SerializeField] private Transform popupCanvasTF;

    [Header("Panels")]
    public PanelConnect panelConnect;
    public PanelGame panelGame;

    [Header("Popups")]
    [SerializeField] private PopupInfo prefabInfo;
    [SerializeField] private PopupDetail prefabDetail;
    [SerializeField] private PopupJoin prefabJoin;
    [SerializeField] private PopupSettings prefabSettings;
    [SerializeField] private PopupTalk prefabTalk;
    // Cache
    [HideInInspector] public PopupInfo PopupInfo;
    [HideInInspector] public PopupDetail PopupDetail;
    [HideInInspector] public PopupJoin PopupJoin;
    [HideInInspector] public PopupSettings PopupSettings;
    [HideInInspector] public PopupTalk PopupTalk;

    public void ShowInfo()
    {
        if (PopupInfo == null) PopupInfo = Instantiate(prefabInfo, popupCanvasTF);
        PopupInfo.Show();
    }

    public void ShowDetail()
    {
        if (PopupDetail == null) PopupDetail = Instantiate(prefabDetail, popupCanvasTF);
        PopupDetail.Show();
    }

    public void ShowJoin()
    {
        if (PopupJoin == null) PopupJoin = Instantiate(prefabJoin, popupCanvasTF);
        PopupJoin.Show();
    }

    public void ShowSettings()
    {
        if (PopupSettings == null) PopupSettings = Instantiate(prefabSettings, popupCanvasTF);
        PopupSettings.Show();
    }

    public void ShowTalk()
    {
        if (PopupTalk == null) PopupTalk = Instantiate(prefabTalk, popupCanvasTF);
        PopupTalk.Show();
    }

    public void SetActiveBlockInput(bool isActive)
    {
        blocker.SetActive(isActive);
    }
}
