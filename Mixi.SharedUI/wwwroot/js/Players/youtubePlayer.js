window.youtubePlayer = null;

window.initYouTubeApi = () =>
{
    var tag = document.createElement('script');
    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
}

window.onYoutubeIframeAPIReady = () =>
{
    console.log("Youtube API ready");
}

window.loadVideoYouTube = (videoId, dotnetHelper) =>
{
    if (window.youtubePlayer)
    {
        window.youtubePlayer.loadVideoById(videoId);
    }
    else
    {
        window.youtubePlayer = new YT.Player('youtube-player-iframe',
            {
                height: '0',
                width: '0',
                videoId: videoId,
                events:
                    {
                        'onStateChange': (event) => onPlayerStateChange(event, dotnetHelper)
                    }
            });
    }
}

function onPlayerStateChange(event, dotnetHelper)
{
    if (evet.data === YT.playerState.ENDED)
    {
        dotnetHelper.invokeMethodAsync('OnTrackEnded')
    }
}

window.stopYoutube = () =>
{
    if(window.youtubePlayer) window.youtubePlayer.stopVideo();
}