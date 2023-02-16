//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//using Photon.Pun;
//using Photon.Realtime;

//public class PlayerManager : MonoBehaviour
//{
//    public float mentalHealth = 100f;
//    public float recoveryIndex = 100f;
//    public bool isMoving;

//    [SerializeField] private PhotonView photonView;
//    [SerializeField] private Camera mainCamera;
//    [SerializeField] private BoolGameEvent mouseLookEvent;

//    [Header("Invisible")]
//    [SerializeField] private SkinnedMeshRenderer[] modelMeshRenderers;

//    private PhotonView otherPV;
//    private PlayerManager otherPlayerManager;

//    private Mouse mouse;
//    private bool isInFreshAirZone;
//    private float moveTimeCount;
//    private float stopTimeCount;

//    private Coroutine dropRICoroutine;
//    private Coroutine increaseRICoroutine;

//    private void Start()
//    {
//        if (photonView == null) photonView = GetComponent<PhotonView>();
//        if (photonView.IsMine == false) return;

//        mouse = Mouse.current;
//        UIM.Instance.panelGame.OnInstantiatePlayer(this);
//        photonView.Owner.NickName = DataManager.Instance.data.info.name;
//        mouseLookEvent.Raise(true);
//    }

//    private void Update()
//    {
//        if (photonView.IsMine == false) return;

//        //if (mouse.leftButton.wasPressedThisFrame && mouseLookEvent.State)
//        //{
//        //    InteractWithOtherObject();
//        //}

//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            OpenSettingsPopup();
//        }

//        if (isMoving)
//        {
//            IncreaseRI();
//        }
//        else
//        {
//            DropRI();
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("FreshArea"))
//        {
//            isInFreshAirZone = !isInFreshAirZone;
//            Debug.Log("throw fresh area");
//        }
//    }

//    private void IncreaseRI()
//    {
//        moveTimeCount += Time.deltaTime;
//        if (moveTimeCount < 5f) return;
//        moveTimeCount = 0f;
//        if (isInFreshAirZone)
//        {
//            if (recoveryIndex < 100f) recoveryIndex += 1f;
//        }
//        else
//        {
//            if (recoveryIndex < 80f) recoveryIndex += 1;
//        }
//    }

//    private void DropRI()
//    {
//        stopTimeCount += Time.deltaTime;
//        if (stopTimeCount < 5f) return;
//        stopTimeCount = 0f;
//        if (!isInFreshAirZone)
//        {
//            if (recoveryIndex > 1f) recoveryIndex -= 1f;
//        }
//    }

//    private void InteractWithOtherObject()
//    {
//        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
//        if (Physics.Raycast(ray, out RaycastHit hit, Constants.InteractableLayer))
//        {
//            if (hit.collider == null) return;
//            PhotonView otherPV = Cache<PhotonView>.Get(hit.collider);

//            if (otherPV == null) return;
//            if ((transform.position - otherPV.transform.position).sqrMagnitude < 15)
//            {
//                otherPlayerManager = (otherPV.Owner.TagObject as GameObject).GetComponentInChildren<PlayerManager>();
//                UIM.Instance.panelGame.ShowButtonRequestToTalk();
//                mouseLookEvent.Raise(false);
//                this.otherPV = otherPV;
//            }
//        }
//    }

//    private void OpenSettingsPopup()
//    {
//        mouseLookEvent.Raise(false);
//        UIM.Instance.panelGame.CrosshairObj.SetActive(false);
//        //UIM.Instance.OpenPopup(UIPopup.Settings);
//    }

//    public void CalculateMH()
//    {
//        PlayerInfo myInfo = DataManager.Instance.data.info;
//        PlayerInfo otherInfo = DataManager.Instance.friendDict[otherPV.ViewID].info;

//        float percent = 0.8f;
//        if (otherInfo.gender != Gender.None) 
//            percent = myInfo.gender == otherInfo.gender ? percent + 0.05f : percent - 0.05f;
//        if (otherInfo.age > 0)
//            percent = Mathf.Abs(myInfo.age - otherInfo.age) <= 10 ? percent + 0.05f : percent - 0.05f;
//        if (otherInfo.ms != MaritalStatus.None)
//            percent = myInfo.ms == otherInfo.ms ? percent + 0.05f : percent - 0.05f;

//        int same = 0;
//        for (int i = 0; i < otherInfo.interests.Count; i++)
//        {
//            if (myInfo.interests.Contains(otherInfo.interests[i])) same++;
//        }

//        if (same > 0) percent += same * 0.02f;
//        Debug.Log("Increase percent: " + percent.ToString());

//        float random = Random.Range(0f, 100f);
//        if (random >= 0f && random <= percent * 100f)
//        {
//            Debug.Log("Increase");
//            mentalHealth += 1f;
//            if (mentalHealth > 100f) mentalHealth = 100f;
//        }
//        else
//        {
//            Debug.Log("Decrease");
//            mentalHealth -= 1f;
//            if (mentalHealth < 0f) mentalHealth = 0f;
//        }
//    }

//    #region RPC
//    public void SendTalkRequest()
//    {
//        otherPlayerManager.ReceiveTalkRequest(photonView.Owner);
//    }

//    private void ReceiveTalkRequest(Player sender)
//    {
//        photonView.RPC(nameof(PunRPCReceiveTalkRequest), photonView.Owner, sender);
//    }

//    [PunRPC]
//    private void PunRPCReceiveTalkRequest(Player sender)
//    {
//        Debug.Log("Receive request");

//        otherPV = (sender.TagObject as GameObject).GetComponentInChildren<PhotonView>();
//        otherPlayerManager = (sender.TagObject as GameObject).GetComponentInChildren<PlayerManager>();

//        UIM.Instance.panelGame.ShowButtonAcceptTalkRequest();

//        if (DataManager.Instance.friendDict.ContainsKey(otherPV.ViewID))
//        {

//        }
//    }

//    public void AcceptTalkRequest()
//    {
//        otherPlayerManager.StartTalk();
//    }

//    private void StartTalk()
//    {
//        photonView.RPC(nameof(PunRPCStartTalk), photonView.Owner);
//    }

//    [PunRPC]
//    private void PunRPCStartTalk()
//    {
//        OpenTalkPopup();
//        UIM.Instance.panelGame.CrosshairObj.SetActive(false);
//    }

//    public void OpenTalkPopup()
//    {
//        mouseLookEvent.Raise(false);
//        //UIM.Instance.OpenPopup(UIPopup.Talk);
//        //(UIM.Instance.PopupDict[UIPopup.Talk] as TalkPopup).OnOpen(this, photonView.Owner.NickName, otherPV.Owner.NickName);
//        //(UIM.Instance.PopupDict[UIPopup.Talk] as TalkPopup).ShowOtherPlayerInformation(otherPV);
//        //(UIM.Instance.PopupDict[UIPopup.Talk] as TalkPopup).StartAutoChat();
//    }

//    public void SendChatMessage(string message)
//    {
//        otherPlayerManager.ReceiveMessage(message);
//    }

//    private void ReceiveMessage(string message)
//    {
//        photonView.RPC(nameof(PunRPCReceiveMessage), photonView.Owner, message);
//    }

//    [PunRPC]
//    private void PunRPCReceiveMessage(string message)
//    {
//        //(UIM.Instance.PopupDict[UIPopup.Talk] as TalkPopup).ReceiveMessage(message);
//    }

//    public void StopTalking()
//    {
//        mouseLookEvent.Raise(true);
//        UIM.Instance.panelGame.CrosshairObj.SetActive(true);
//        otherPlayerManager.ReceiveStopTalking();
//    }

//    private void ReceiveStopTalking()
//    {
//        photonView.RPC(nameof(PunRPCReceiveStopTalking), photonView.Owner);
//    }

//    [PunRPC]
//    private void PunRPCReceiveStopTalking()
//    {
//        //UIM.Instance.ClosePopup(UIPopup.Talk);
//        mouseLookEvent.Raise(true);
//        UIM.Instance.panelGame.CrosshairObj.SetActive(true);
//        CalculateMH();
//    }
//    #endregion
//}
