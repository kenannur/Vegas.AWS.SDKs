using System;
using System.Net;
using Amazon.Runtime;

namespace Vegas.AWS.Common.Extensions
{
    public static class AmazonWebServiceExtensions
    {
        public static void EnsureSuccessStatusCode(this AmazonWebServiceResponse response)
        {
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.HttpStatusCode.ToString());
            }
        }
    }
}
