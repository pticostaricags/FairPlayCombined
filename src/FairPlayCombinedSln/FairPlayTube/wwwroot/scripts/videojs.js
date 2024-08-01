//For Watch Time, check https://stackoverflow.com/questions/54878077/track-the-time-the-video-was-playing-in-the-web-page

let timer = null;
let totalTime = 0;
let lastPlayerTime = null;
let player = null;
let dotNetHelper = null;
let currentSessionGuid = null;

function disposePlayer(playerElementId) {
    let player = videojs.getPlayer(playerElementId);
    if (player) {
        player.dispose();
    }
}
function initializeVideoJsPlayer(playerElementId, sessionGuid, dotNetObjectReference,
    videoSrc, videoType, supportedLanguages, videoId) {
    timer = null;
    totalTime = 0;
    lastPlayerTime = null;
    player = null;
    dotNetHelper = null;
    currentSessionGuid = null

    dotNetHelper = dotNetObjectReference;

    disposePlayer(playerElementId);
    player = videojs(playerElementId,
        {
            controls: true,
            autoplay: true,
            responsive: true,
            sources: [
                {
                    src: videoSrc,
                    type: videoType
                }]
        });
    player.one("loadedmetadata", function () {
        supportedLanguages.forEach(language => {
            console.log(language);
            let captionsUrl = `/api/video/${videoId}/captions/${language.languageCode}`;
            player.addRemoteTextTrack(
                {
                    kind: 'captions',
                    src: captionsUrl,
                    srclang: language.languageCode,
                    label: language.name
                }, true);
        });
    });
    lastPlayerTime = player.currentTime();
    currentSessionGuid = sessionGuid;
    player.on('play', startPlaying);
    player.on('pause', pausePlaying);
}

function startPlaying() {
    console.log('played');
    timer = window.setInterval(function () {
        if (lastPlayerTime != player.currentTime()) {
            totalTime += 1;
            dotNetHelper.invokeMethodAsync('UpdateWatchTime', totalTime, currentSessionGuid);
        }
        lastPlayerTime = player.currentTime();
        console.log(lastPlayerTime);
        console.log(player.currentTime());
        console.log(totalTime);
    }, 1000);
}

function pausePlaying() {
    console.log('stopped');
    if (timer) clearInterval(timer);
}