// Init Source
function initSource(myPlayerElementId, publishedUrl, viewToken, videoId) {
    // Init your AMP instance
    let myPlayer = amp(myPlayerElementId, { /* Options */
        "nativeControlsForTouch": false,
        autoplay: true,
        controls: true,
        width: "640",
        height: "360",
        poster: ""
    }, function () {

    });

    myPlayer.addEventListener("pause", function (event) {
        console.log("paused");
    });
    // Paste the streaming endpoint and the viewToken. you can find them at GetVideoIndex response
    // (https://api-portal.videoindexer.ai/docs/services/operations/operations/Get-Video-Index)
    myPlayer.src([
        {
            // get the src (streaming endpoint) from GetVideoIndex.videos[0].publishedUrl
            "src": publishedUrl,
            "type": "video/mp4",
            "protectionInfo": [{
                type: "AES",
                authenticationToken: viewToken
            }]
        }
    ], [
        { src: `/api/video/${videoId}/captions/en`, srclang: "en", kind: "subtitles", label: "English" },
        { src: `/api/video/${videoId}/captions/es`, srclang: "es", kind: "subtitles", label: "Spanish" }
    ]);
}