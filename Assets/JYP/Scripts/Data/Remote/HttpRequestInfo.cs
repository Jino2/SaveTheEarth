using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class HttpRequestInfo<T, TR>
{
    public string url;
    public T requestBody;
    public Action<TR> onSuccess;
    public Action onError;
    public string contentType;
}