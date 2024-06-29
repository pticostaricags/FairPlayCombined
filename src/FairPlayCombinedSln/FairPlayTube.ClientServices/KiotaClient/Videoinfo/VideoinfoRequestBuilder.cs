// <auto-generated/>
using FairPlayTube.ClientServices.KiotaClient.Videoinfo.GetPaginatedCompletedVideoInfoAsync;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace FairPlayTube.ClientServices.KiotaClient.Videoinfo {
    /// <summary>
    /// Builds and executes requests for operations under \videoinfo
    /// </summary>
    public class VideoinfoRequestBuilder : BaseRequestBuilder {
        /// <summary>The GetPaginatedCompletedVideoInfoAsync property</summary>
        public GetPaginatedCompletedVideoInfoAsyncRequestBuilder GetPaginatedCompletedVideoInfoAsync { get =>
            new GetPaginatedCompletedVideoInfoAsyncRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new VideoinfoRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public VideoinfoRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/videoinfo", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new VideoinfoRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public VideoinfoRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/videoinfo", rawUrl) {
        }
    }
}
