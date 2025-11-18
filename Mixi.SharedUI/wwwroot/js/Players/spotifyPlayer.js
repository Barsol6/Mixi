window.spotifyPlayer = null;

window.initSpotifyPlayer =  (token, dotnetHelper) => {
    const script  = document.createElement('script');
    script.src = `https://sdk.scdn.co/spotify-player.js`;
    script.async = true;
    document.body.appendChild(script);
    
    window.onSpotifyWebPlaybackSDKReady = () => {
        const player = new Spotify.Player({
            name: 'Mixi Web Player',
            getOAuthToken: cb => { cb(token); },
            volume: 0.5
        });
        
        player.addListener('player_state_changed', state=>
        {
            if (state && state.paused && state.position===0 && state.restrictions.disallow_resuming_reasons && state.restrictions.disallow_resuming_reasons[0] === 'not paused')
            {
                dotnetHelper.invokeMethodAsync('OnTrackEnded');
            }
        });
        
        player.connect();
        window.spotifyPlayer = player;
    };
};

window.playSpotifyTrack = (spotifyUri) =>
{
 fetch('https://api.spotify.com/v1/me/player/play',
     {
         method: 'PUT',
         body: JSON.stringify({ uris: [spotifyUri] }),
         headers: { 
             'Content-Type': 'application/json',
             'Authorization': `Bearer ${token}`
          }
     })   
}