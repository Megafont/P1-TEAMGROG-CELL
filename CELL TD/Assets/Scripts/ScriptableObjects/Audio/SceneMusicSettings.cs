using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "New SceneMusicSettings Asset", menuName = "Scene Music Settings/New SceneMusicSettings Asset")]
public class SceneMusicSettings : ScriptableObject
{
    [Tooltip("The scene this music settings object will be applied to.")]
    public SceneAsset Scene;

    [Tooltip("Allows you to adjust the volume for the music in this scene. NOTE: This option should only be changed if the music in this one scene is too loud.")]
    [Range(0.0f, 1.0f)]
    public float VolumeAdjustment = 1.0f;

    [Tooltip("Determines whether the music for this scene will fade in or just start instantly like the main menu music.")]
    public bool EnableFadeIn = true;

    [Tooltip("If true, all tracks in the list are played simulateonusly, but all are muted but one. This way we can seamlessly fade into a higher intensity version of the currently playing track.")]
    public bool SyncTracks;

    [Tooltip("The music clips that will be played in the scene specified above.")]
    public List<AudioClip> MusicClips;
}
