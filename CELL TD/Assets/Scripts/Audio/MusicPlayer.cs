using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }


    [Tooltip("Sets the length of cross fades from one music track to another.")]
    [SerializeField]
    private float _CrossFadeDuration = 1.5f;

    [Tooltip("Sets how many audio sources the music player will start with.")]
    [Min(1)]
    [SerializeField]
    private const int _NumberOfAudioSourcesToStartWith = 3;

    [Tooltip("This should be set to the Music group of the audio mixer.")]
    [SerializeField]
    private AudioMixerGroup _MusicMixerGroup;



    /// <summary>
    /// This list specifies the music settings for each scene.
    /// </summary>
    private List<SceneMusicSettings> _SceneMusicSettings;

    /// <summary>
    /// Holds the audio sources that are playing the current music.
    /// </summary>
    private List<AudioSource> _CurrentMusicAudioSources = new List<AudioSource>();
    /// <summary>
    /// Holds the audio sources that were playing the last music.
    /// </summary>
    private List<AudioSource> _LastMusicAudioSources = new List<AudioSource>();

    /// <summary>
    /// The index of the music settings of the currently playing music.
    /// </summary>
    private int _CurrentMusicSettingsIndex = 0;
    /// <summary>
    /// The index of the music settings for the last music that was playing.
    /// </summary>
    private int _LastMusicSettingsIndex = -1;


    /// <summary>
    /// Whether or not a cross fade between tracks is currently in progress.
    /// </summary>
    private bool _IsCrossFadeInProgress;

    /// <summary>
    /// True if any music is playing.
    /// </summary>
    private bool _IsPlaying;



    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is already an instance of MusicPlayer in this scene. This one will now self-destruct to avoid problems.");
            Destroy(gameObject);
            return;
        }


        Instance = this;


        // Load in all of the SceneMusicSettings assets.
        _SceneMusicSettings = Resources.LoadAll<SceneMusicSettings>("").ToList();
            

        InitMusicPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(string sceneName)
    {        
        int newMusicIndex = FindMusicSettingsIndexForScene(sceneName);
        if (newMusicIndex < 0)
        {
            Debug.LogError($"Cannot play music because there is no SceneMusicSettings asset for the scene \"{sceneName}\" in Assets/Resources/SceneMusicSettings!");
            return;
        }

        // If the SceneMusicSettings asset we found is the same one whose music is currently playing, then just return.
        // There's no need to do anything, and it would be silly to restart or cross fade into the same music that's already playing.
        if (_IsPlaying && newMusicIndex == _CurrentMusicSettingsIndex)
            return;

        // Does this music need to be faded in?
        if (_SceneMusicSettings[newMusicIndex].EnableFadeIn)
        {
            if (_IsCrossFadeInProgress)
            {
                Debug.LogWarning($"Cannot begin cross fading to music for scene \"{_SceneMusicSettings[newMusicIndex]}\" because a music cross fade is already in progress!");
                return;
            }


            _LastMusicSettingsIndex = _CurrentMusicSettingsIndex;
            _CurrentMusicSettingsIndex = newMusicIndex;

            StartCoroutine(DoCrossFadeBetweenTrackGroups());
        }
        else
        {
            _LastMusicSettingsIndex = _CurrentMusicSettingsIndex;
            _CurrentMusicSettingsIndex = newMusicIndex;

            PlayInstantly();
        }
    }

    public void StopMusic()
    {
        StopMusicTrackGroup(_CurrentMusicAudioSources);
        StopMusicTrackGroup(_LastMusicAudioSources);

        _IsCrossFadeInProgress = false;
        _IsPlaying = false;
    }

    private void StartMusicTrackGroup(List<AudioSource> audioSources, int firstTrackIndex = 0)
    {
        bool syncTracks = _SceneMusicSettings[_CurrentMusicSettingsIndex].SyncTracks;
        float maxVolume = _SceneMusicSettings[_CurrentMusicSettingsIndex].VolumeAdjustment;


        for (int i = 0; i < audioSources.Count; i++)
        {
            // If we are syncing all tracks, then they will play simulataneously, but all but the starting track will have their volume set to 0.
            // This way we can seamlessly fade into a higher intensity music track.
            if (syncTracks)
            {
                audioSources[i].Play();
                if (i == firstTrackIndex)
                    audioSources[i].volume = maxVolume;
                else
                    audioSources[i].volume = 0f;
            }
            else
            {
                if (i == firstTrackIndex)
                {
                    audioSources[i].Play();
                    audioSources[i].volume = maxVolume;
                }
                else
                {
                    audioSources[i].Stop();
                    audioSources[i].volume = 0f;
                }
            }

        } // end for i
    }

    private void StopMusicTrackGroup(List<AudioSource> audioSources)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].Stop();
        }


        CheckIfMusicIsPlaying();
    }

    private void PlayInstantly()
    {
        // First stop all playing music tracks.
        StopMusic();

        // Swap the audio source groups, and then assign the appropriate tracks to the new current group.
        SwapAudioSourceLists();
        AssignTracksToCurrentAudioSources(); // This function also sets volume to 0 in prep for the fade in.

        // Set the music volume based on the music settings scriptable object for this scene.
        // That setting should generally only be used if an individual scene's music is too load.
        SetMusicVolume(_SceneMusicSettings[_CurrentMusicSettingsIndex].VolumeAdjustment, _CurrentMusicAudioSources);
       
        // Start the new music playing.
        StartMusicTrackGroup(_CurrentMusicAudioSources);
    }

    private IEnumerator DoCrossFadeBetweenTrackGroups()
    {
        // If a cross fade is already in progress, then simply end this coroutine.
        if (_IsCrossFadeInProgress)
        {
            yield break;
        }


        _IsCrossFadeInProgress = true;
        _IsPlaying = true;


        // Swap the audio source groups, and then assign the appropriate tracks to the new current group.
        SwapAudioSourceLists();
        AssignTracksToCurrentAudioSources(); // This function also sets volume to 0 in prep for the fade in.

        // Start the new music playing.
        StartMusicTrackGroup(_CurrentMusicAudioSources);

        float timer = 0f;
        float percent = 0;
        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / _CrossFadeDuration;


            // Fade in the audio sources playing the current scene music.
            SetMusicVolume(_SceneMusicSettings[_CurrentMusicSettingsIndex].VolumeAdjustment * percent, 
                           _CurrentMusicAudioSources);


            if (_IsPlaying)
            {
                // Fade out the audio sources playing the previous scene music.
                SetMusicVolume(_SceneMusicSettings[_LastMusicSettingsIndex].VolumeAdjustment * (1 - percent), 
                               _LastMusicAudioSources);
            }


            // Wait one frame.
            yield return null;

        } // end while


        StopMusicTrackGroup(_LastMusicAudioSources);

        _IsCrossFadeInProgress = false;
    }

    public void AdvanceToNextTrackInGroup()
    {
        int curTrackIndex = -1;

        // Find the currently playing track.
        for (int i = 0; i < _CurrentMusicAudioSources.Count; i++)
        {
            if (_CurrentMusicAudioSources[i].isPlaying)
            {
                curTrackIndex = i;
                break;
            }

        } // end for i


        int nextTrackIndex = curTrackIndex + 1;
        if (nextTrackIndex >= _CurrentMusicAudioSources.Count)
        {
            // We are already on the highest intensity track, so just leave it on that one.
            return;
        }

        StartCoroutine(DoCrossFade(_CurrentMusicAudioSources[curTrackIndex], 
                                   _CurrentMusicAudioSources[nextTrackIndex]));

    }

    private IEnumerator DoCrossFade(AudioSource current, AudioSource next)
    {
        // If a cross fade is already in progress, then simply end this coroutine.
        if (_IsCrossFadeInProgress)
        {
            yield break;
        }


        _IsCrossFadeInProgress = true;
        _IsPlaying = true;


        // Only call next.Play() if we are not syncing tracks. This is because when the tracks are being synced, this track is already playing but with a volume of 0. This way both tracks are playing and stay synced but you only hear one.
        if (!_SceneMusicSettings[_CurrentMusicSettingsIndex].SyncTracks)            
            next.Play();


        float timer = 0f;
        float percent = 0;
        while (percent < 1)
        {
            timer += Time.deltaTime;
            percent = timer / _CrossFadeDuration;

            // Fade in the audio sources playing the current scene music.
            next.volume = percent;

            // Fade out the audio source playing the previous music track.
            current.volume = 1 - percent;


            // Wait one frame.
            yield return null;

        } // end while


        _IsCrossFadeInProgress = false;
    }


    private void AssignTracksToCurrentAudioSources()
    {
        for (int i = 0; i < _SceneMusicSettings[_CurrentMusicSettingsIndex].MusicClips.Count; i++)
        {
            _CurrentMusicAudioSources[i].volume = 0;
            _CurrentMusicAudioSources[i].clip = _SceneMusicSettings[_CurrentMusicSettingsIndex].MusicClips[i];
            _CurrentMusicAudioSources[i].playOnAwake = false;
        }
    }

    private void SetMusicVolume(float volume, List<AudioSource> audioSources)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].volume = Mathf.Clamp01(volume);
        }
    }       

    private void SwapAudioSourceLists()
    {
        List<AudioSource> temp = _CurrentMusicAudioSources;
        _CurrentMusicAudioSources = _LastMusicAudioSources;
        _LastMusicAudioSources = temp;
    }

    private int FindMusicSettingsIndexForScene(string sceneName)
    {
        if (sceneName.ToLower().StartsWith("level") && sceneName.ToLower() != "levelselector")
        {
            // The loaded scene appears to be a level, so find the SceneMusicSettings object named "SceneMusicSettings_InGame".
            // We can't just search by scene name, since we'd have to add all level scenes to the list in that settings object.
            // Instead, it just lets you specifu one scene since most of the music settings assets will only be applied to one scene.
            // In this one special case, we want to use this SceneMusicSettings asset for all scenes that are levels.
            for (int i = 0; i < _SceneMusicSettings.Count; i++)
            {
                if (_SceneMusicSettings[i].name == "SceneMusicSettings_InGame")
                    return i;
            }
        }
        else
        {
            // The loaded scene does not appear to be a level, so find the SceneMusicSettings object for the loaded scene.
            for (int i = 0; i < _SceneMusicSettings.Count; i++)
            {
                if (_SceneMusicSettings[i].SceneName == sceneName)
                {
                    return i;
                }
            }
        }

        // We failed to find music settings for the requested scene, so return -1 to indicate an error.
        return -1;
    }

    private void InitMusicPlayer()
    {
        // Create the first group of audio source objects.
        for (int i = 0; i < _NumberOfAudioSourcesToStartWith; i++)
        {
            // Create a new AudioSource for both lists.
            _CurrentMusicAudioSources.Add(CreateNewAudioSource($"Audio Source A{i + 1}"));
        }

        // Create the second group of audio source objects.
        for (int i = 0; i < _NumberOfAudioSourcesToStartWith; i++)
        {
            // Create a new AudioSource for both lists.
            _LastMusicAudioSources.Add(CreateNewAudioSource($"Audio Source B{(i + 1)}"));
        }
    }

    /// <summary>
    /// Checks if music is currently playing, and updates _IsPlaying.
    /// </summary>
    /// <returns>True if music is playing, or false otherwise.</returns>
    private bool CheckIfMusicIsPlaying()
    {
        for (int i = 0; i < _CurrentMusicAudioSources.Count; i++)
        {
            if (_CurrentMusicAudioSources[i].isPlaying ||
                _LastMusicAudioSources[i].isPlaying)
            {
                _IsPlaying = true;
                return true;
            }

        } // end for i


        _IsCrossFadeInProgress = false;
        _IsPlaying = false;

        return false;
    }

    private AudioSource CreateNewAudioSource(string name)
    {
        // Create a new GameObject.
        GameObject newObj = new GameObject(name);
        // Parent it to this Music Player object.
        newObj.transform.SetParent(transform);
        // Add an AudioSource component to it.
        AudioSource audioSource =  newObj.AddComponent<AudioSource>();
        // Set the audio source to be part of the music audio mixer group.
        audioSource.outputAudioMixerGroup = _MusicMixerGroup;
        // Disable PlayOnAwake.
        audioSource.playOnAwake = false;
        // Make it loop the music.
        audioSource.loop = true;

        return audioSource;
    }



    /// <summary>
    /// Returns true if any music is playing.
    /// </summary>
    public bool IsPlaying { get { return _IsPlaying; } }
}
