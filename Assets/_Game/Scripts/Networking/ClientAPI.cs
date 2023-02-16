//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//public class ClientAPI : MonoBehaviour
//{
//    public string url;
//    public string postUrl = "localhost:3000/enemy/create";

//    private void Start()
//    {
//        //StartCoroutine(Get(url));
//        var enemy = new EnemyEntity()
//        {
//            name = "haaa",
//            health = 300,
//            attack = 30
//        };

//        StartCoroutine(Post(postUrl, enemy));
//    }

//    public IEnumerator Get(string url)
//    {
//        using (UnityWebRequest request = UnityWebRequest.Get(url))
//        {
//            yield return request.SendWebRequest();

//            if (request.result == UnityWebRequest.Result.ConnectionError)
//            {
//                Debug.Log(request.error);
//            }
//            else
//            {
//                if (request.isDone)
//                {
//                    var result = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);

//                    EnemyEntity enemy = JsonUtility.FromJson<EnemyEntity>(result);
//                    Debug.Log(enemy.name);
//                }
//                else
//                {
//                    Debug.Log("Error! Couldn't get data");
//                }
//            }
//        }
//    }

//    public IEnumerator Post(string url, EnemyEntity enemy)
//    {
//        var jsonData = JsonUtility.ToJson(enemy);
//        Debug.Log(jsonData);

//        using (UnityWebRequest request = UnityWebRequest.Post(url, jsonData))
//        {
//            request.SetRequestHeader("content-type", "application/json");
//            request.uploadHandler.contentType = "application/json";
//            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));

//            yield return request.SendWebRequest();

//            if (request.result == UnityWebRequest.Result.ConnectionError)
//            {
//                Debug.Log(request.error);
//            }
//            else
//            {
//                if (request.isDone)
//                {
//                    string result = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
//                    result = "{\"result\":" + result + "}";
//                    Debug.Log(result);

//                    List<EnemyEntity> resultEnemyList = JsonHelper.FromJson<EnemyEntity>(result);
//                    foreach (EnemyEntity item in resultEnemyList)
//                    {
//                        Debug.Log(item.name);
//                    }
//                }
//                else
//                {
//                    Debug.Log("Error!");
//                }
//            }
//        }
//    }
//}
