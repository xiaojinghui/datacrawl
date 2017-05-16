// Decompiled with JetBrains decompiler
// Type: DataAccess.HttpHelper
// Assembly: DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 69F1BCE6-BC8D-4256-8EDD-4050A5FAB4AA
// Assembly location: D:\BaiduYunDownload\Data\DataAccess.dll

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace DataAccess
{
  public class HttpHelper
  {
    private string m_URL = string.Empty;
    private string m_Method = string.Empty;
    private string m_ContentType = string.Empty;
    private int m_Timeout = 100000;
    private byte[] m_PostedDatas;

    public byte[] PostedDatas
    {
      get
      {
        return this.m_PostedDatas;
      }
      set
      {
        this.m_PostedDatas = value;
      }
    }

    public string URL
    {
      get
      {
        return this.m_URL;
      }
    }

    public HttpHelper(string url)
      : this(url, (byte[]) null)
    {
    }

    public HttpHelper(string url, byte[] datas)
      : this(url, datas, "POST")
    {
    }

    public HttpHelper(string url, byte[] datas, string method)
      : this(url, datas, method, "application/x-www-form-urlencoded")
    {
    }

    public HttpHelper(string url, byte[] datas, string method, string contentType)
      : this(url, datas, method, contentType, 900000)
    {
    }

    public HttpHelper(string url, byte[] datas, string method, string contentType, int timeout)
    {
      if (string.IsNullOrEmpty(url))
        throw new ArgumentException("Parameter url is null or empty, please check it!");
      if (datas == null)
        datas = Encoding.ASCII.GetBytes(string.Empty);
      this.m_PostedDatas = datas;
      this.m_URL = url;
      this.m_Method = !string.IsNullOrEmpty(method) ? method : "POST";
      this.m_ContentType = !string.IsNullOrEmpty(contentType) ? contentType : "application/x-www-form-urlencoded";
      if (timeout <= 0)
        return;
      this.m_Timeout = timeout;
    }

    public string GetResponseString()
    {
      string responseString = string.Empty;
      this.RetrieveResponse(this.m_URL, (HttpHelper.RetrieveDatas) (responseStream =>
      {
        Encoding encoding = Encoding.GetEncoding("gb2312");
        using (StreamReader streamReader = new StreamReader(responseStream, encoding))
          responseString = streamReader.ReadToEnd();
      }), (HttpCookieCollection) null);
      return responseString;
    }

    public byte[] GetResponseBytes(HttpCookieCollection cookies)
    {
      byte[] responseDatas = (byte[]) null;
      this.RetrieveResponse(this.m_URL, (HttpHelper.RetrieveDatas) (responseStream =>
      {
        using (BinaryReader binaryReader = new BinaryReader(responseStream))
          responseDatas = binaryReader.ReadBytes(20000000);
      }), cookies);
      return responseDatas;
    }

    public IAsyncResult BeginRequest(HttpCookieCollection cookies, AsyncCallback callback)
    {
      HttpWebRequest httpWebRequest = this.PrepareWebRequest(this.m_URL, cookies);
      return httpWebRequest.BeginGetResponse(callback, (object) httpWebRequest);
    }

    private void RetrieveResponse(string url, HttpHelper.RetrieveDatas retrievingData, HttpCookieCollection cookies)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) null;
      try
      {
        httpWebRequest = this.PrepareWebRequest(url, cookies);
        using (WebResponse response = httpWebRequest.GetResponse())
        {
          using (Stream responseStream = response.GetResponseStream())
            retrievingData(responseStream);
        }
      }
      catch (Exception ex)
      {
        if (!(ex is WebException) || ((WebException) ex).Status != WebExceptionStatus.ProtocolError)
          return;
        using (Stream responseStream = ((WebException) ex).Response.GetResponseStream())
          retrievingData(responseStream);
      }
      finally
      {
        if (httpWebRequest != null)
          httpWebRequest.Abort();
      }
    }

    private HttpWebRequest PrepareWebRequest(string url, HttpCookieCollection cookies)
    {
      HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
      httpWebRequest.Method = this.m_Method;
      httpWebRequest.KeepAlive = false;
      httpWebRequest.ContentType = this.m_ContentType;
      httpWebRequest.Timeout = this.m_Timeout;
      if (cookies != null && cookies.Count > 0)
      {
        httpWebRequest.CookieContainer = new CookieContainer();
        for (int index = 0; index < cookies.Count; ++index)
          httpWebRequest.CookieContainer.Add(new Cookie()
          {
            Domain = httpWebRequest.RequestUri.Host,
            Expires = cookies[index].Expires,
            Name = cookies[index].Name,
            Path = cookies[index].Path,
            Secure = cookies[index].Secure,
            Value = cookies[index].Value
          });
      }
      if (this.m_Method == "POST" || this.m_Method == "PUT")
      {
        httpWebRequest.ContentLength = (long) this.m_PostedDatas.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
          requestStream.Write(this.m_PostedDatas, 0, this.m_PostedDatas.Length);
      }
      return httpWebRequest;
    }

    private delegate void RetrieveDatas(Stream responseStream);
  }
}
