using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Security.Cryptography;

namespace SuT.PMAPI.Generic
{
    public class HashAuthenticator : IAuthenticator
    {
        private readonly string _cid;
        private readonly string _uid;
        private readonly string _hash;

        public HashAuthenticator(int uid, int cid, string hash)
        {
            _uid = uid.ToString();
            _cid = cid.ToString();
            _hash = hash;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            string nonce = CreateSHA1Nonce();
            string dateNow = DateTime.Now.ToUniversalTime().ToString("R");

            request.AddParameter("Date", dateNow, ParameterType.HttpHeader);
            request.AddParameter("X-SuT-UID", _uid, ParameterType.HttpHeader);
            request.AddParameter("X-SuT-CID", _cid, ParameterType.HttpHeader);
            request.AddParameter("X-SuT-Nonce", nonce, ParameterType.HttpHeader);

            string signature = string.Format(
                "{0} /{1}\r\nDate: {2}\r\nX-SuT-CID: {3}\r\nX-SuT-UID: {4}\r\nX-SuT-Nonce: {5}\r\n{6}",
                request.Method,
                request.Resource,
                dateNow,
                _cid,
                _uid,
                nonce,
                _hash
            );

            request.AddParameter("Authorization", "SuTHash signature=\"" + toCompatibleSha1(signature) + "\"", ParameterType.HttpHeader);
        }

        private string toCompatibleSha1(string s)
        {
            return BitConverter.ToString(SHA1CryptoServiceProvider.Create().ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(s))).Replace("-", "").ToLower();
        }

        public string CreateSHA1Nonce()
        {
            string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            var nonce = new StringBuilder();
            for (int i = 0; i < 50; i++)
            {
                nonce.Append(validChars[random.Next(0, validChars.Length - 1)]);
            }

            return toCompatibleSha1(nonce.ToString());
        }
    }
}
