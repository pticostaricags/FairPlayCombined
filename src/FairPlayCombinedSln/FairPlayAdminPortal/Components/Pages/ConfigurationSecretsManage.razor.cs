using FairPlayCombined.Common;
using FairPlayCombined.Models.Common.ConfigurationSecret;
using FairPlayCombined.Services.Common;
using Microsoft.AspNetCore.Components;

namespace FairPlayAdminPortal.Components.Pages
{
    public partial class ConfigurationSecretsManage : IDisposable
    {
        [SupplyParameterFromForm]
        private List<ConfigurationSecretModel> allConfigurationSecrets { get; set; } = new();

        private ConfigurationSecretModel? openAIConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.OPENAI_KEY);

        private ConfigurationSecretModel? generateDall3ImageConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.GENERATE_DALL3_IMAGE_URL_KEY);

        private ConfigurationSecretModel? openAIChatCompletionConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.OPENAI_CHAT_COMPLETION_URL_KEY);

        private ConfigurationSecretModel? openAITextGenerationModelConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.OPENAI_TEXT_GENERATION_MODEL_KEY);

        private ConfigurationSecretModel? azureOpenAIEndpointConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_OPENAI_ENDPOINT_KEY);

        private ConfigurationSecretModel? azureOpenAIKeyConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_OPENAI_KEY_KEY);

        private ConfigurationSecretModel? azureOpenAIDeploymentNameConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_OPENAI_DEPLOYMENT_NAME_KEY);

        private ConfigurationSecretModel? azureVideoIndexerAccountIdConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_ACCOUNT_ID_KEY);

        private ConfigurationSecretModel? azureVideoIndexerLocationConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_LOCATION_KEY);

        private ConfigurationSecretModel? azureVideoIndexerResourceGroupConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_GROUP_KEY);

        private ConfigurationSecretModel? azureVideoIndexerResourceNameConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY);

        private ConfigurationSecretModel? azureVideoIndexerSubscriptionIdConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY);

        private ConfigurationSecretModel? googleGeminiKeyConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.GOOGLE_GEMINI_KEY_KEY);

        private ConfigurationSecretModel? azureContentSafetyEndpointConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_ENDPOINT_KEY);

        private ConfigurationSecretModel? azureContentSafetyKeyConfiguration => allConfigurationSecrets?
        .SingleOrDefault(p => p.Name == FairPlayCombined.Common.Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_KEY_KEY);


        private ExternalServicesHealthModel[]? externalServicesHealthModels;
        private bool disposedValue;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private bool IsBusy { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.allConfigurationSecrets = (await this.configurationSecretService.GetAllConfigurationSecretAsync(
            cancellationToken: this.cancellationTokenSource.Token)).ToList();
            if (this.allConfigurationSecrets?.Count == 0)
                this.allConfigurationSecrets = new List<ConfigurationSecretModel>();
            InitializeOpenAIKeyFields();
            InitializeDalle3ImageUrlFields();
            InitializeOpenAIChatCompletionKeyFields();
            InitializeOpenAITextGenerationModelFields();
            InitializeAzureOpenAIEndpontFields();
            InitializeAzureOpenAIKeyFields();
            InitializeAzureOpenAIDeploymentNameFields();
            if (this.azureVideoIndexerAccountIdConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_ACCOUNT_ID_KEY
                });
            }
            if (this.azureVideoIndexerLocationConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_LOCATION_KEY
                });
            }
            if (this.azureVideoIndexerResourceGroupConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_GROUP_KEY
                });
            }
            if (this.azureVideoIndexerResourceNameConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY
                });
            }
            if (this.azureVideoIndexerSubscriptionIdConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY
                });
            }
            if (this.googleGeminiKeyConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.GOOGLE_GEMINI_KEY_KEY
                });
            }
            if (this.azureContentSafetyEndpointConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_ENDPOINT_KEY
                });
            }
            if (this.azureContentSafetyKeyConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_KEY_KEY
                });
            }
            StateHasChanged();
        }

        private void InitializeAzureOpenAIDeploymentNameFields()
        {
            if (this.azureOpenAIDeploymentNameConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_OPENAI_DEPLOYMENT_NAME_KEY
                });
            }
        }

        private void InitializeAzureOpenAIKeyFields()
        {
            if (this.azureOpenAIKeyConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_OPENAI_KEY_KEY
                });
            }
        }

        private void InitializeAzureOpenAIEndpontFields()
        {
            if (this.azureOpenAIEndpointConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.AZURE_OPENAI_ENDPOINT_KEY
                });
            }
        }

        private void InitializeOpenAITextGenerationModelFields()
        {
            if (this.openAITextGenerationModelConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.OPENAI_TEXT_GENERATION_MODEL_KEY
                });
            }
        }

        private void InitializeOpenAIChatCompletionKeyFields()
        {
            if (this.openAIChatCompletionConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.OPENAI_CHAT_COMPLETION_URL_KEY
                });
            }
        }

        private void InitializeDalle3ImageUrlFields()
        {
            if (this.generateDall3ImageConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.GENERATE_DALL3_IMAGE_URL_KEY
                });
            }
        }

        private void InitializeOpenAIKeyFields()
        {
            if (this.openAIConfiguration is null)
            {
                this.allConfigurationSecrets!.Add(new ConfigurationSecretModel()
                {
                    Name = Constants.ConfigurationSecretsKeys.OPENAI_KEY
                });
            }
        }

        private async Task OnValidSubmitAsync()
        {
            this.IsBusy = true;
            StateHasChanged();
            ExternalServicesConfigurationModel externalServicesConfigurationModel = new()
            {
                AzureOpenDeploymentName = this.azureOpenAIDeploymentNameConfiguration!.Value,
                OpenAIClient = new Azure.AI.OpenAI.OpenAIClient(openAIApiKey: this.azureOpenAIKeyConfiguration!.Value)
            };
            ExternalServicesHealthService externalServicesHealthService = new(externalServicesConfigurationModel);
            this.externalServicesHealthModels = await externalServicesHealthService.CheckServicesHealthAsync(this.cancellationTokenSource.Token);
            await this.configurationSecretService.UpdateAllConfigurationSecretAsync(this.allConfigurationSecrets,
            this.cancellationTokenSource.Token);
            this.IsBusy = false;
            StateHasChanged();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.cancellationTokenSource.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
