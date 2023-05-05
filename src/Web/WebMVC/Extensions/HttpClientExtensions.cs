﻿namespace Microsoft.eShopOnContainers.WebMVC.Extensions;

public static class HttpClientExtensions
{
    public static void SetBasicAuthentication(this HttpClient client, string userName, string password) =>
        client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(userName, password);

    public static void SetToken(this HttpClient client, string scheme, string token) =>
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);

    public static void SetBearerToken(this HttpClient client, string token) =>
        client.SetToken(JwtConstants.TokenType, token);
}

public class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
{
    public BasicAuthenticationHeaderValue(string userName, string password)
        : base("Basic", EncodeCredential(userName, password))
    { }

    private static string EncodeCredential(string userName, string password)
    {
        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        string credential = string.Format("{0}:{1}", userName, password);

        return Convert.ToBase64String(encoding.GetBytes(credential));
    }
}
