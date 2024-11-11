using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.FairPlayTube.VideoInfographic;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Collections.Concurrent;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class VideoInfographic
    {
        [Parameter]
        public long? VideoInfoId { get; set; }
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IOpenAIService? OpenAIService { get; set; }
        [Inject] IVideoCaptionsService? VideoCaptionsService { get; set; }
        [Inject] IPromptGeneratorService? PromptGeneratorService { get; set; }
        [Inject] IVideoInfographicService? VideoInfographicService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IPhotoService? PhotoService { get; set; }
        [Inject] HttpClient? HttpClient { get; set; }
        [Inject] NavigationManager? NavigationManager { get; set; }
        [Inject] IStringLocalizer<VideoInfographic>? Localizer { get; set; }
        private VideoInfoModel? VideoInfoModel { get; set; }
        private bool IsBusy { get; set; }
        private PaginationOfT<VideoInfographicModel>? Items;
        private readonly ConcurrentDictionary<long, string> PagePhotos = new();
        private readonly PaginationState paginationState = new()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };
        private readonly PaginationRequest paginationRequest = new()
        {
            PageSize = Constants.Pagination.PageSize,
            StartIndex = 0,
            SortingItems =
                        [
                        new SortingItem()
                        {
                            PropertyName = nameof(VideoInfographicModel.PhotoId),
                            SortType = SortType.Descending
                        }
                        ]
        };
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private string? GeneratedInfographicUri { get; set; }
        private string? RevisedPrompt { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                this.VideoInfoModel = await this.VideoInfoService!.GetVideoInfoByIdAsync(this.VideoInfoId!.Value, this.cancellationTokenSource.Token);
                var items = await this.VideoInfographicService!.GetPaginatedVideoInfographicByVideoInfoIdAsync(
                    this.VideoInfoId!.Value, paginationRequest, this.cancellationTokenSource.Token);
                this.Items = items;
                await this.paginationState.SetTotalItemCountAsync(items.TotalItems);
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
            finally
            {
                this.IsBusy = false;
                this.PagePhotos.Clear();
                StateHasChanged();
                await LoadImagesBase64Async();
            }
        }

        private async Task LoadImagesBase64Async()
        {
            if (this.Items?.Items?.Length > 0)
            {
                foreach (var singlePhotoId in this.Items.Items.Select(p => p.PhotoId))
                {
                    string requestUrl = $"{NavigationManager!.BaseUri}api/photo/{singlePhotoId}";
                    var result = await HttpClient!.GetByteArrayAsync(requestUrl, this.cancellationTokenSource.Token);
                    string base64String = Convert.ToBase64String(result);
                    this.PagePhotos.TryAdd(singlePhotoId, $"data:image/png;base64, {base64String}");
                    StateHasChanged();
                }
            }
        }

        private async Task OnCreateNewInfographicClickedAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                var promptInfo = await this.PromptGeneratorService!.GetPromptCompleteInfoAsync(promptName:
                Constants.PromptsNames.CreateVideoInfographic, cancellationToken: this.cancellationTokenSource.Token);
                var videoInfoModel = await this.VideoInfoService!.GetVideoInfoByIdAsync(id: this.VideoInfoId!.Value,
                cancellationToken: this.cancellationTokenSource.Token);
                var englishCaptions = await this.VideoCaptionsService!
                .GetVideoCaptionsByVideoInfoIdAndLanguageAsync(videoInfoId: videoInfoModel.VideoInfoId,
                language: "en-US", cancellationToken: this.cancellationTokenSource.Token);
                string prompt = $"{promptInfo!.BaseText}. Video Title: {videoInfoModel.Description}. Video Captions: {englishCaptions}";
                if (prompt.Length > 4000)
                    prompt = prompt[..4000];
                var result = await this.OpenAIService!.GenerateDallE3ImageAsync(prompt, this.cancellationTokenSource.Token);
                if (result != null)
                {
                    this.GeneratedInfographicUri = result!.data![0]!.url!;
                    this.RevisedPrompt = result!.data[0]!.revised_prompt;
                }
                var photoId = await PhotoService!.CreatePhotoAsync(
                new FairPlayCombined.Models.Common.Photo.CreatePhotoModel()
                {
                    Filename = $"Video-{this.VideoInfoId}-Infographic.jpg",
                    Name = $"Video-{this.VideoInfoId}-Infographic",
                    PhotoBytes = await this.HttpClient!.GetByteArrayAsync(this.GeneratedInfographicUri,
                                                                                    this.cancellationTokenSource.Token)
                }, this.cancellationTokenSource.Token);
                await this.VideoInfographicService!.CreateVideoInfographicAsync(
                    new CreateVideoInfographicModel()
                    {
                        PhotoId = photoId,
                        VideoInfoId = this.VideoInfoId.Value
                    }, this.cancellationTokenSource.Token
                );
                this.paginationRequest.StartIndex = 0;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
            finally
            {
                this.IsBusy = false;
                StateHasChanged();
            }
        }

        private async Task OnCurrentPageIndexChangedAsync(int pageNumber)
        {
            this.paginationRequest.StartIndex = pageNumber * this.paginationRequest.PageSize;
            await LoadDataAsync();
        }

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "Video Infographic")]
        public const string VideoInfographicTextKey = "VideoInfographicText";
        [ResourceKey(defaultValue: "Create New Infographic")]
        public const string CreateNewInfographicTextKey = "CreateNewInfographicText";
        #endregion Resource Keys
    }
}