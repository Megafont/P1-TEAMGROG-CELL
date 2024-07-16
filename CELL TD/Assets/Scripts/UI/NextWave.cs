using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextWave : MonoBehaviour
{
    [SerializeField]
    Button _btn;
    public static NextWave Instance;

    [SerializeField]
    private AudioSource _audioPlayer;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void StartNextWave()
    {
        WaveManager.Instance.StartNextWave();
        _btn.enabled = false;
        _btn.transform.localScale = Vector3.zero;
    }

    public void EnableButton()
    {
        _audioPlayer.Play();
        _btn.enabled = true;
        _btn.transform.localScale = Vector3.one;
    }
}
