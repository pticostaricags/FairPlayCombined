// <auto-generated/>
using FairPlayTube.ClientServices.KiotaClient.Account;
using FairPlayTube.ClientServices.KiotaClient.Api;
using FairPlayTube.ClientServices.KiotaClient.ConfirmEmail;
using FairPlayTube.ClientServices.KiotaClient.Culture;
using FairPlayTube.ClientServices.KiotaClient.ForgotPassword;
using FairPlayTube.ClientServices.KiotaClient.Identity;
using FairPlayTube.ClientServices.KiotaClient.Localization;
using FairPlayTube.ClientServices.KiotaClient.Login;
using FairPlayTube.ClientServices.KiotaClient.Manage;
using FairPlayTube.ClientServices.KiotaClient.Refresh;
using FairPlayTube.ClientServices.KiotaClient.Register;
using FairPlayTube.ClientServices.KiotaClient.ResendConfirmationEmail;
using FairPlayTube.ClientServices.KiotaClient.ResetPassword;
using FairPlayTube.ClientServices.KiotaClient.Videoinfo;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Serialization.Form;
using Microsoft.Kiota.Serialization.Json;
using Microsoft.Kiota.Serialization.Multipart;
using Microsoft.Kiota.Serialization.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace FairPlayTube.ClientServices.KiotaClient {
    /// <summary>
    /// The main entry point of the SDK, exposes the configuration and the fluent API.
    /// </summary>
    public class ApiClient : BaseRequestBuilder {
        /// <summary>The Account property</summary>
        public AccountRequestBuilder Account { get =>
            new AccountRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The api property</summary>
        public ApiRequestBuilder Api { get =>
            new ApiRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The confirmEmail property</summary>
        public ConfirmEmailRequestBuilder ConfirmEmail { get =>
            new ConfirmEmailRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The Culture property</summary>
        public CultureRequestBuilder Culture { get =>
            new CultureRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The forgotPassword property</summary>
        public ForgotPasswordRequestBuilder ForgotPassword { get =>
            new ForgotPasswordRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The identity property</summary>
        public IdentityRequestBuilder Identity { get =>
            new IdentityRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The localization property</summary>
        public LocalizationRequestBuilder Localization { get =>
            new LocalizationRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The login property</summary>
        public LoginRequestBuilder Login { get =>
            new LoginRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The manage property</summary>
        public ManageRequestBuilder Manage { get =>
            new ManageRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The refresh property</summary>
        public RefreshRequestBuilder Refresh { get =>
            new RefreshRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The register property</summary>
        public RegisterRequestBuilder Register { get =>
            new RegisterRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The resendConfirmationEmail property</summary>
        public ResendConfirmationEmailRequestBuilder ResendConfirmationEmail { get =>
            new ResendConfirmationEmailRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The resetPassword property</summary>
        public ResetPasswordRequestBuilder ResetPassword { get =>
            new ResetPasswordRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The videoinfo property</summary>
        public VideoinfoRequestBuilder Videoinfo { get =>
            new VideoinfoRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new ApiClient and sets the default values.
        /// </summary>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ApiClient(IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}", new Dictionary<string, object>()) {
            ApiClientBuilder.RegisterDefaultSerializer<JsonSerializationWriterFactory>();
            ApiClientBuilder.RegisterDefaultSerializer<TextSerializationWriterFactory>();
            ApiClientBuilder.RegisterDefaultSerializer<FormSerializationWriterFactory>();
            ApiClientBuilder.RegisterDefaultSerializer<MultipartSerializationWriterFactory>();
            ApiClientBuilder.RegisterDefaultDeserializer<JsonParseNodeFactory>();
            ApiClientBuilder.RegisterDefaultDeserializer<TextParseNodeFactory>();
            ApiClientBuilder.RegisterDefaultDeserializer<FormParseNodeFactory>();
        }
    }
}
