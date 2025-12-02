window.youtubePlayer = null;
window.youTubeApiReady = false;

window.initYouTubeApi = () => {
    var tag = document.createElement('script');
    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
}

window.onYouTubeIframeAPIReady = () => {
    console.log("Youtube API ready");
    window.youTubeApiReady = true;
}

window.loadVideoYouTube = (videoId, dotnetHelper) => {
    if (!window.youTubeApiReady) {
        console.warn("YT API not ready yet, retrying…");
        setTimeout(() => window.loadVideoYouTube(videoId, dotnetHelper), 100);
        return;
    }
    if (window.youtubePlayer && typeof window.youtubePlayer.loadVideoById === 'function') {
        window.youtubePlayer.loadVideoById(videoId);
        window.youtubePlayer.setVolume(100);
    } else {
        window.youtubePlayer = new YT.Player('youtube-player-iframe',
            {
                height: '100%',
                width: '100%',
                videoId: videoId,
                playerVars: {
                    autoplay: 1,
                    origin: window.location.origin,
                    playsInline: 1
                },
                events:
                    {
                        'onReady': (event) => {
                            event.target.setVolume(100);
                            event.target.playVideo();
                        },
                        'onStateChange': (event) => {
                            onPlayerStateChange(event, dotnetHelper)
                        },
                        'onError': (event) => {
                            console.error("YT Error: " + event.data)
                        }
                    }
            });
    }
}

function onPlayerStateChange(event, dotnetHelper) {
    if (event.data === YT.PlayerState.ENDED) {
        dotnetHelper.invokeMethodAsync('OnTrackEnded')
    }
}

window.stopYouTube = () => {
    if (window.youtubePlayer) {
        window.youtubePlayer.stopVideo();
    };
    
}

window.pauseYouTube = () => {
    if (window.youtubePlayer && window.youtubePlayer.pauseVideo) {
        window.youtubePlayer.pauseVideo();
    }
};

window.resumeYouTube = () => {
    if (window.youtubePlayer && window.youtubePlayer.playVideo) {
        window.youtubePlayer.playVideo();
    }
};
window.getYoutubePosition = () => {
    if (window.youtubePlayer && window.youtubePlayer.getCurrentTime) {
        return window.youtubePlayer.getCurrentTime();
    }
    return 0;
};

window.getYoutubeDuration = () => {
    if (window.youtubePlayer && window.youtubePlayer.getDuration) {
        return window.youtubePlayer.getDuration();
    }
    return 0;
};

window.destroyYouTube = () => {
    if (window.youtubePlayer) {
        window.youtubePlayer.destroy();
    }
};

window.youtubePlayer = null;
window.youTubeApiReady = false;