window.currentHowl = null;

window.howlerPlayer = {
    play: (url, dotnetHelper) => {
        if (window.currentHowl) {
            window.currentHowl.stop();
            window.currentHowl.unload();
        }
        
        console.log("Trying to play: "+url);
        
        window.currentHowl = new Howl({ 
            src: [url],
            html5: true,
            volume: 0.5,
            onend: function ()
            {
                console.log("Track ended");
                dotnetHelper.invokeMethodAsync('OnTrackEnded');
            },
            onloaderror: function (id, err)
            { 
                console.error("Track load error: ", err); 
            },
            onplayerror: function (id, err)
            { 
                console.error("Track play error: ", err); 
            }
        });
        
        window.currentHowl.play();
    },
    stop: () =>
    {
        if (window.currentHowl)
        {
            window.currentHowl.stop();
            window.currentHowl.unload();
            window.currentHowl = null;
        }
    },
    pause: ()=> 
    {
        if (window.currentHowl) window.currentHowl.pause();
    },
    resume: ()=>
    {
        if (window.currentHowl) window.currentHowl.play();
    },
    setVolume (volume)
    {
        if (window.currentHowl) window.currentHowl.volume(volume);
    }
};