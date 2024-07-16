using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;

public class WaveCount : MonoBehaviour
{
    public TextMeshProUGUI waveCountText;
    public int currentWave = 0;

    private void Update()
    {
        WaveCounter(this, EventArgs.Empty);
    }

    public void WaveCounter(object Sender, EventArgs a)
    {
        if (WaveManager.Instance)
        {
            waveCountText.text = "" + WaveManager.Instance.WaveNumber;
        }
    }
}
