using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPManager : MonoBehaviour
{
    private static HTTPManager instance;

    // ReSharper disable Unity.PerformanceAnalysis
    public static HTTPManager GetInstance()
    {
        if (!instance)
        {
            var go = new GameObject("HTTPManager");
            var manager = go.AddComponent<HTTPManager>();
        }

        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Test()
    {
        var testUrl = "http://localhost:8000/hello/test";
        var request = UnityWebRequest.Get(testUrl);
        var asyncOperation = request.SendWebRequest();
        asyncOperation.completed += operation => { Debug.Log($"{request.downloadHandler.text}"); };
    }

    public void Get<T, R>(HttpRequestInfo<T, R> requestInfo)
    {
        StartCoroutine(GetAsync(requestInfo));
    }

    public IEnumerator GetAsync<T, R>(HttpRequestInfo<T, R> requestInfo)
    {
        using var request = UnityWebRequest.Get(requestInfo.url);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<R>(request.downloadHandler.text);
            requestInfo.onSuccess(response);
        }
        else
        {
        }
    }

    public void Post<T, R>(HttpRequestInfo<T, R> requestInfo)
    {
        StartCoroutine(PostAsync(requestInfo));
    }

    private IEnumerator PostAsync<T, R>(HttpRequestInfo<T, R> requestInfo)
    {
        string bodyJson = JsonUtility.ToJson(requestInfo.requestBody);
        using var request = UnityWebRequest.PostWwwForm(requestInfo.url, bodyJson);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<R>(request.downloadHandler.text);
            requestInfo.onSuccess(response);
        }
        else
        {
            requestInfo.onError();
        }
    }
}