using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class PanelGame : PanelBase
{
    public GameObject objCrosshair;
    [SerializeField] private TMP_Text nameTMP;

    [Header("Talk")]
    [SerializeField] private Transform btnRequestTF;
    [SerializeField] private Transform btnAcceptTF;

    [Header("Stat")]
    [SerializeField] private Image fillImageMH;
    [SerializeField] private Image fillImageRI;
    [SerializeField] private TMP_Text currentMHTMP;
    [SerializeField] private TMP_Text currentRITMP;

    private Tweener btnRequestTweener;
    private Tweener btnAcceptTweener;

    private void Start()
    {
        nameTMP.text = DataManager.Instance.data.info.name;
    }

    public void ShowButtonRequestToTalk()
    {
        btnRequestTF.gameObject.SetActive(true);
        if (btnRequestTweener == null)
        {
            btnRequestTweener = btnRequestTF.DOScale(1.2f, 0.75f);
            btnRequestTweener.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            btnRequestTweener.SetAutoKill(false);
        }
        else
        {
            btnRequestTweener.Restart();
        }
    }

    public void ShowButtonAcceptTalkRequest()
    {
        btnAcceptTF.gameObject.SetActive(true);
        if (btnAcceptTweener == null)
        {
            btnAcceptTweener = btnAcceptTF.DOScale(1.2f, 0.75f);
            btnAcceptTweener.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            btnAcceptTweener.SetAutoKill(false);
        }
        else
        {
            btnAcceptTweener.Restart();
        }
    }

    public void UpdateMH(int currentMH)
    {
        fillImageMH.fillAmount = (float)currentMH / 100f;
        currentMHTMP.text = currentMH.ToString() + "/100";
    }

    public void UpdateRI(int currentRI)
    {
        fillImageRI.fillAmount = (float)currentRI / 100f;
        currentRITMP.text = currentRI.ToString() + "/100";
    }

    #region UI callbacks
    public void ButtonMouseLook()
    {
        Spawner.PM.input.SetActiveLook(true);
        objCrosshair.SetActive(true);
    }

    public void ButtonRequestToTalk()
    {
        Spawner.PM.SendTalkRequest();
        btnRequestTweener.Rewind();
        btnRequestTF.gameObject.SetActive(false);
    }

    public void ButtonAcceptTalkRequest()
    {
        Spawner.PM.AcceptTalkRequest();
        objCrosshair.SetActive(false);
        Spawner.PM.OpenTalkPopup();
        btnAcceptTweener.Rewind();
        btnAcceptTF.gameObject.SetActive(false);
    }

    //public void ButtonAccept()
    //{

    //}

    //public void ButtonSendMessage()
    //{
    //    playerController.SendMess(messIF.text);
    //    TMP_Text text = Instantiate(textPrefab, content);
    //    text.text = messIF.text;
    //}

    //public void ReceiveMess(string mess)
    //{
    //    TMP_Text text = Instantiate(textPrefab, content);
    //    text.text = mess;
    //}

    //public void ReceiveInfo(string info)
    //{
    //    TMP_Text text = Instantiate(textPrefab, informationContent);
    //    text.text = info;
    //}
    #endregion
}
