using System.Net.Http;
using System.Net;
using System;
using System.Security.Policy;

namespace RaspredeleniyeDutyaApp.Helpers
{
    public static class RequestHelper
    {
        private static readonly string Host = Environment.GetEnvironmentVariable("HostAppAddress")!;

        private static HttpStatusCode lastStatusCode = HttpStatusCode.OK;
        public static HttpStatusCode LastStatusCode { get { return lastStatusCode; } }

        private static HttpClient PrepareClient(Uri uri, IRequestCookieCollection? cookies)
        {
            HttpClient client = new();

            if (cookies != null && cookies.Count > 0)
            {
                CookieContainer cookieContainer = new();
                foreach (var cookie in cookies)
                {
                    cookieContainer.Add(uri, new Cookie(cookie.Key, cookie.Value));
                }
                client.DefaultRequestHeaders.Add("cookie", cookieContainer.GetCookieHeader(uri));
            }
            return client;
        }

        public static async Task<T?> RequestGet<T>(string url, IRequestCookieCollection? cookies = null)
        {
            Uri uri = new(Host + url);
            HttpResponseMessage response = await PrepareClient(uri, cookies).GetAsync(uri);
            HttpContent content = response.Content;
            lastStatusCode = response.StatusCode;
            //if (response.StatusCode == HttpStatusCode.BadRequest)
            //    throw new Exception("Bad Request: " + await response.Content.ReadAsStringAsync());
            if (response.StatusCode != HttpStatusCode.OK)
                return default;

            try
            {
                return (T?)await content.ReadFromJsonAsync(typeof(T), new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception)
            {
                string contentString = await content.ReadAsStringAsync();
                throw new Exception("JsonReaderException was thrown, JSON content: " + contentString);
            }
        }

        public static async Task<HttpStatusCode> RequestGetStatus(string url, IRequestCookieCollection? cookies = null)
        {
            Uri uri = new(Host + url);
            HttpResponseMessage response = await PrepareClient(uri, cookies).GetAsync(uri);
            HttpContent content = response.Content;
            lastStatusCode = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception("Bad Request: " + await response.Content.ReadAsStringAsync());
            return response.StatusCode;
        }

        public static async Task RequestGetNoReturn(string url, IRequestCookieCollection? cookies = null)
        {
            Uri uri = new(Host + url);
            HttpResponseMessage response = await PrepareClient(uri, cookies).GetAsync(uri);
            lastStatusCode = response.StatusCode;
        }

        public static async Task<T?> RequestPost<T>(string url, object data, IRequestCookieCollection? cookies = null)
        {
            Uri uri = new(Host + url);
            HttpResponseMessage response = await PrepareClient(uri, cookies).PostAsJsonAsync(uri, data);
            HttpContent content = response.Content;
            lastStatusCode = response.StatusCode;
            if (response.StatusCode != HttpStatusCode.OK)
                return default;

            return (T?)await content.ReadFromJsonAsync(typeof(T), new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public static async Task<HttpStatusCode> RequestPostStatus(string url, object data, IRequestCookieCollection? cookies = null)
        {
            Uri uri = new(Host + url);
            HttpResponseMessage response = await PrepareClient(uri, cookies).PostAsJsonAsync(uri, data);
            lastStatusCode = response.StatusCode;
            if (response.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception("Bad Request: " + await response.Content.ReadAsStringAsync());
            return response.StatusCode;
        }

        public static async Task<bool> RequestIsUserAdminAsync(IRequestCookieCollection cookies)
            => await RequestGet<bool>("/server/isUserAdmin", cookies);

        public static bool RequestIsUserAdmin(IRequestCookieCollection cookies)
        {
            var task = RequestIsUserAdminAsync(cookies);
            task.Wait();
            return task.Result;
        }
    }
}
