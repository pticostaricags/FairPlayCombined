namespace FairPlayCombined.Common.FairPlaySocial
{
    public static class Constants
    {
        public static class Hubs
        {
            public const string HomeFeedHub = $"/{nameof(HomeFeedHub)}";
            public const string UserMessageHub = $"/{nameof(UserMessageHub)}";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string SendMessage = "SendMessage";
        }
    }
}
