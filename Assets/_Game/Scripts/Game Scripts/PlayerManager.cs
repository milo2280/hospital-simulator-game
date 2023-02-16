using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    [Header("Health Stat")]
    public int mentalHealth = 100;
    public int recoveryIndex = 100;

    [Header("Move & Look")]
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private float topClamp = 89f;
    [SerializeField] private float botClamp = -89f;

    [Header("Components")]
    [SerializeField] private PhotonView view;
    [SerializeField] private Transform cachedTF;
    [SerializeField] private Transform eyesTF;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;
    public InputHandler input;

    [Header("Camera")]
    [SerializeField] private GameObject objMainCam;
    [SerializeField] private Camera mainCamera;

    [Header("Mesh Renderers")]
    [SerializeField] private Material transparent;
    [SerializeField] private SkinnedMeshRenderer[] renderers;

    private float targetPitch;
    private float countTimeMH;
    private float countTimeRI;
    private bool isStop;
    private bool isFreshAir;
    private PlayerAnimation currentAnimation;
    [HideInInspector] public PhotonView otherView;
    [HideInInspector] public PlayerManager otherPM;

    #region MonoBehaviour Callbacks
    private void Start()
    {
        if (view.IsMine == false) return;

        objMainCam.SetActive(true);
        for (int i = 0; i < renderers.Length; i++) 
            renderers[i].material = transparent;
        view.Owner.NickName = DataManager.Instance.data.info.name;
        UIM.Instance.panelGame.UpdateMH(mentalHealth);
        UIM.Instance.panelGame.UpdateRI(recoveryIndex);
    }

    private void Update()
    {
        if (view.IsMine == false) return;

        Move();

        if (Mouse.current.leftButton.wasPressedThisFrame && input.isLook)
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIM.Instance.ShowSettings();
            input.SetActiveLook(false);
            UIM.Instance.panelGame.objCrosshair.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (view.IsMine == false) return;

        Rotate();
    }

    private void OnMouseOver()
    {
        if (view.IsMine == true) return;

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.EnableKeyword(Constants.Emission);
    }

    private void OnMouseExit()
    {
        if (view.IsMine == true) return;

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.DisableKeyword(Constants.Emission);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FreshArea"))
        {
            isFreshAir = !isFreshAir;
            Debug.Log("throw fresh area");
        }
    }
    #endregion

    private void Move()
    {
        if (input.move != Vector2.zero)
        {
            Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;
            inputDirection = cachedTF.right * input.move.x + cachedTF.forward * input.move.y;
            controller.Move(inputDirection * moveSpeed * Time.deltaTime);
            ChangeAnimation(PlayerAnimation.Run);

            if (isStop)
            {
                countTimeMH = 0f;
                countTimeRI = 0f;
                isStop = false;
            }

            countTimeMH += Time.deltaTime;
            countTimeRI += Time.deltaTime;

            if (countTimeMH > 5f)
            {
                if (isFreshAir && recoveryIndex < 100) UIM.Instance.panelGame.UpdateRI(++recoveryIndex);
                else if (!isFreshAir && recoveryIndex < 80) UIM.Instance.panelGame.UpdateRI(++recoveryIndex);
                countTimeMH = 0f;
            }
        }
        else
        {
            ChangeAnimation(PlayerAnimation.Idle);

            if (!isStop)
            {
                countTimeMH = 0f;
                countTimeRI = 0f;
                isStop = true;
            }

            countTimeMH += Time.deltaTime;
            countTimeRI += Time.deltaTime;

            if (countTimeMH > 5f)
            {
                if (mentalHealth > 0) UIM.Instance.panelGame.UpdateMH(--mentalHealth);
                countTimeMH = 0f;
            }

            if (countTimeRI > 5f)
            {
                if (recoveryIndex > 0) UIM.Instance.panelGame.UpdateRI(--recoveryIndex);
                countTimeRI = 0f;
            }
        }
    }

    private void Rotate()
    {
        if (input.look.sqrMagnitude < 0.0001f) return;
        targetPitch += input.look.y * rotationSpeed;
        if (targetPitch < -360f) targetPitch += 360f;
        else if (targetPitch > 360f) targetPitch -= 360f;
        targetPitch = Mathf.Clamp(targetPitch, botClamp, topClamp);
        eyesTF.localRotation = Quaternion.Euler(targetPitch, 0f, 0f);
        cachedTF.Rotate(Vector3.up * input.look.x * rotationSpeed);
    }

    private void ChangeAnimation(PlayerAnimation nextAnimation)
    {
        if (nextAnimation == currentAnimation) return;
        animator.ResetTrigger(currentAnimation.ToString());
        animator.SetTrigger(nextAnimation.ToString());
        currentAnimation = nextAnimation;
    }

    private void Interact()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, Constants.InteractableLayer))
        {
            if (hit.collider == null) return;
            otherView = Cache<PhotonView>.Get(hit.collider);

            if (otherView == null) return;
            if ((transform.position - otherView.transform.position).sqrMagnitude < 15)
            {
                otherPM = (otherView.Owner.TagObject as GameObject).GetComponentInChildren<PlayerManager>();
                UIM.Instance.panelGame.ShowButtonRequestToTalk();
                input.SetActiveLook(false);
            }
            else
            {

            }
        }
    }

    public void CalculateMH()
    {
        PlayerInfo myInfo = DataManager.Instance.data.info;
        PlayerInfo otherInfo = DataManager.Instance.friendDict[otherView.ViewID].info;

        float percent = 0.8f;
        if (otherInfo.gender != Gender.None)
            percent = myInfo.gender == otherInfo.gender ? percent + 0.05f : percent - 0.05f;
        if (otherInfo.age > 0)
            percent = Mathf.Abs(myInfo.age - otherInfo.age) <= 10 ? percent + 0.05f : percent - 0.05f;
        if (otherInfo.ms != MaritalStatus.None)
            percent = myInfo.ms == otherInfo.ms ? percent + 0.05f : percent - 0.05f;

        int same = 0;
        for (int i = 0; i < otherInfo.interests.Count; i++)
        {
            if (myInfo.interests.Contains(otherInfo.interests[i])) same++;
        }

        if (same > 0) percent += same * 0.02f;
        Debug.Log("Increase percent: " + percent.ToString());

        float random = Random.Range(0f, 100f);
        if (random >= 0f && random <= percent * 100f)
        {
            Debug.Log("Increase");
            if (mentalHealth < 95) mentalHealth += 5;
        }
        else
        {
            Debug.Log("Decrease");
            if (mentalHealth > 5) mentalHealth -= 5;
        }
        UIM.Instance.panelGame.UpdateMH(mentalHealth);
    }

    #region RPC
    public void SendTalkRequest()
    {
        otherPM.ReceiveTalkRequest(view.Owner);
    }

    private void ReceiveTalkRequest(Player sender)
    {
        view.RPC(nameof(PunRPCReceiveTalkRequest), view.Owner, sender);
    }

    [PunRPC]
    private void PunRPCReceiveTalkRequest(Player sender)
    {
        Debug.Log("Receive request");

        otherView = (sender.TagObject as GameObject).GetComponentInChildren<PhotonView>();
        otherPM = (sender.TagObject as GameObject).GetComponentInChildren<PlayerManager>();

        UIM.Instance.panelGame.ShowButtonAcceptTalkRequest();
    }

    public void AcceptTalkRequest()
    {
        otherPM.StartTalk();
    }

    private void StartTalk()
    {
        view.RPC(nameof(PunRPCStartTalk), view.Owner);
    }

    [PunRPC]
    private void PunRPCStartTalk()
    {
        OpenTalkPopup();
        UIM.Instance.panelGame.objCrosshair.SetActive(false);
    }

    public void OpenTalkPopup()
    {
        input.SetActiveLook(false);
        UIM.Instance.ShowTalk();
        UIM.Instance.PopupTalk.ShowOtherInfo(otherView);
        UIM.Instance.PopupTalk.StartAutoChat();
        UIM.Instance.PopupSettings?.Close();
    }

    public void SendChatMessage(string message)
    {
        otherPM.ReceiveMessage(message);
    }

    private void ReceiveMessage(string message)
    {
        view.RPC(nameof(PunRPCReceiveMessage), view.Owner, message);
    }

    [PunRPC]
    private void PunRPCReceiveMessage(string message)
    {
        UIM.Instance.PopupTalk.ReceiveMessage(message);
    }

    public void StopTalking()
    {
        input.SetActiveLook(true);
        UIM.Instance.panelGame.objCrosshair.SetActive(true);
        otherPM.ReceiveStopTalking();
        CalculateMH();
    }

    private void ReceiveStopTalking()
    {
        view.RPC(nameof(PunRPCReceiveStopTalking), view.Owner);
    }

    [PunRPC]
    private void PunRPCReceiveStopTalking()
    {
        UIM.Instance.PopupTalk.Close();
        input.SetActiveLook(true);
        UIM.Instance.panelGame.objCrosshair.SetActive(true);
        CalculateMH();
    }
    #endregion
}

public enum PlayerAnimation
{
    Idle,
    Walk,
    Run,
    Jump,
}


