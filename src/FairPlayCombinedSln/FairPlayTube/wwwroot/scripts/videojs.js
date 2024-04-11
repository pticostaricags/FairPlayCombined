let timer = null;
let totalTime = 0;
let lastPlayerTime = null;
let player = null;
function initializeVideoJsPlayer(playerElementId) {
    debugger;
    player = videojs(playerElementId, {});
    lastPlayerTime = player.currentTime();
    player.on('play', startPlaying);
    player.on('pause', pausePlaying);
}

function startPlaying() {
    console.log('played');
    timer = window.setInterval(function () {
        if (lastPlayerTime != player.currentTime()) {
            totalTime += 1;
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