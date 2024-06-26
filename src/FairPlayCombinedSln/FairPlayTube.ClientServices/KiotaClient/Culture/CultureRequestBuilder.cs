// <auto-generated/>
using FairPlayTube.ClientServices.KiotaClient.Culture.Set;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace FairPlayTube.ClientServices.KiotaClient.Culture {
    /// <summary>
    /// Builds and executes requests for operations under \Culture
    /// </summary>
    public class CultureRequestBuilder : BaseRequestBuilder {
        /// <summary>The Set property</summary>
        public SetRequestBuilder Set { get =>
            new SetRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new CultureRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public CultureRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/Culture", pathParameters) {
        }
        /// <summary>
        /// Instantiates a new CultureRequestBuilder and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public CultureRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/Culture", rawUrl) {
        }
    }
}
