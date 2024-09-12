using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        if(requestInfo.contentType == null || requestInfo.contentType == "")
        {
            request.SetRequestHeader("Content-Type","application/json");

        }
        else
        {
            request.SetRequestHeader("Content-Type",requestInfo.contentType);

        }
        byte[] jsonToSend = new UTF8Encoding().GetBytes(bodyJson);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<R>(request.downloadHandler.text);
            requestInfo.onSuccess(response);
        }
        else
        {
            Debug.LogError($"{request.error} - {request.result} - {request.downloadHandler.text}");
            requestInfo.onError();
        }
    }

    public void Delete<T, R>(HttpRequestInfo<T, R> requestInfo)
    {
        StartCoroutine(DeleteAsync(requestInfo));
    }

    private IEnumerator DeleteAsync<T, R>(HttpRequestInfo<T, R> requestInfo)
    {
        string bodyJson = JsonUtility.ToJson(requestInfo.requestBody);
        using var request = UnityWebRequest.Delete(requestInfo.url);
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyJson));
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

    public void UploadMultipart<R>(HttpRequestInfo<List<IMultipartFormSection>, R> requestInfo)
    {
        StartCoroutine(UploadMultipartAsync(requestInfo));
    }

    private IEnumerator UploadMultipartAsync<R>(HttpRequestInfo<List<IMultipartFormSection>, R> requestInfo)
    {
        print($"{requestInfo.requestBody[0].contentType} - {requestInfo.requestBody[0].fileName}");
        using var request = UnityWebRequest.Post(requestInfo.url, requestInfo.requestBody);


        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            print(request.downloadHandler.text);
            var response = JsonUtility.FromJson<R>(request.downloadHandler.text);
            requestInfo.onSuccess(response);
        }
        else
        {
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
            requestInfo.onError();
        }
    }
}