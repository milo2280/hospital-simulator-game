using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using DG.Tweening;

public class GameManager : PersistentSingleton<GameManager>
{
    public Scene currentScene;

    private void Start()
    {
        DataManager.Instance.LoadData();
    }

    public void PhotonLoadLevel(Scene scene)
    {
        if (scene == currentScene) return;
        DOTween.KillAll();
        PhotonNetwork.LoadLevel(scene.ToString());
        currentScene = scene;
    }

    public void ChangeScene(Scene scene)
    {
        if (scene == currentScene) return;
        DOTween.KillAll();
        SceneManager.LoadScene(scene.ToString());
        currentScene = scene;
    }

    public void OnLeftRoom(Scene scene)
    {
        if (scene == currentScene) return;
        DOTween.KillAll();
        currentScene = scene;
    }
}

