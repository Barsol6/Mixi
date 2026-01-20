window.currentHowl = null;

window.howlerPlayer = {
    play: (url, dotnetHelper) => {
        if (window.currentHowl) {
            window.currentHowl.stop();
            window.currentHowl.unload();
        }

        console.log("Trying to play: " + url);

        window.currentHowl = new Howl({
            src: [url],
            html5: true,
            volume: 0.5,
            onend: function () {
                console.log("Track ended");
                dotnetHelper.invokeMethodAsync('OnTrackEnded');
            }
        });

        window.currentHowl.play();
    },
    stop: () => {
        if (window.currentHowl) {
            window.currentHowl.stop();
            window.currentHowl.unload();
            window.currentHowl = null;
        }
    },
    pause: () => {
        if (window.currentHowl) window.currentHowl.pause();
    },
    resume: () => {
        if (window.currentHowl) window.currentHowl.play();
    },
    setVolume: (volume)=>{
        if (window.currentHowl) window.currentHowl.volume(volume);
    },
    getPosition: () => {
        if (window.currentHowl) return window.currentHowl.seek();
        return 0;
    },
    getDuration: () => {
        if (window.currentHowl) return window.currentHowl.duration();
        return 0;
    }
    
};