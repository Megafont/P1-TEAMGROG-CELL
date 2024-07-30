using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "New SceneMusicSettings Asset", menuName = "Scene Music Settings/New SceneMusicSettings Asset")]
public class SceneMusicSettings : ScriptableObject
{
    /// <summary>
    /// The SceneAsset can only be used in the editor since it is an Editor-only thing.
    /// So we conditionally compile to make sure it is ignored in a build.
    /// Otherwise, the build will fail with an error that it can't find the type SceneAsset.
    /// </summary>
#if UNITY_EDITOR
    [Tooltip("The scene this music settings object will be applied to.")]
    public SceneAsset Scene;
#endif


    [Tooltip("Allows you to adjust the volume for the music in this scene. NOTE: This option should only be changed if the music in this one scene is too loud.")]
    [Range(0.0f, 1.0f)]
    public float VolumeAdjustment = 1.0f;

    [Tooltip("Determines whether the music for this scene will fade in or just start instantly like the main menu music.")]
    public bool EnableFadeIn = true;

    [Tooltip("If true, all tracks in the list are played simulateonusly, but all are muted but one. This way we can seamlessly fade into a higher intensity version of the currently playing track.")]
    public bool SyncTracks;

    [Tooltip("The music clips that will be played in the scene specified above.")]
    public List<AudioClip> MusicClips;

    [SerializeField, HideInInspector]
    private string _SceneName;



    private void OnValidate()
    {
        // We have to conditionally compile this for the same reason as the Scene field above.
        // SceneAsset is an Editor-only type, so this makes sure this code doesn't get included in builds.
#if UNITY_EDITOR
        // If the Scene setting was changed, automatically update the name property.
        // We need this, because we can't use SceneAsset in a build since it is an Editor-only thing.
        if (Scene != null)
        {
            SceneName = Scene.name;
            Debug.Log($"SAVED: {SceneName}");
        }
        else
            SceneName = "";
#endif

    }



    public string SceneName 
    {
        get { return _SceneName; }
        private set { _SceneName = value; } 
    }
}
