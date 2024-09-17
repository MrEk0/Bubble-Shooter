using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;

namespace Utils
{
    public static class HttpHelper
    {
        public static string HttpGet(string url, string contentType)
        {
            var headers = new Dictionary<HttpRequestHeader, string> { { HttpRequestHeader.ContentType, contentType } };
            return HttpGet(url, headers);
        }

        private static string HttpGet(string url, Dictionary<HttpRequestHeader, string> headers)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Get;

                foreach (var pair in headers.ToList())
                {
                    switch (pair.Key)
                    {
                        case HttpRequestHeader.ContentType:
                        case HttpRequestHeader.Accept:
                        case HttpRequestHeader.Referer:
                        case HttpRequestHeader.UserAgent:
                            request.Accept = pair.Value;
                            headers.Remove(pair.Key);
                            break;
                    }
                }

                foreach (var pair in headers)
                    request.Headers.Add(pair.Key, pair.Value);

                using var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using var stream = response.GetResponseStream();
                    if (stream == null)
                        return string.Empty;

                    using var read = new StreamReader(stream);
                    return read.ReadToEnd();
                }

                Debug.Log($"{url}: {response.StatusCode}, {response.StatusDescription}");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"{url}: {ex}");
            }

            return string.Empty;
        }
    }
}
