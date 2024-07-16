using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// This class automatically loads in all Wave assets from the Resources folder,
/// which contain the data on each wave.
/// </summary>
public class WaveCollection : MonoBehaviour
{
    private List<Waves> _WaveList = new List<Waves>();



    private void Awake()
    {
        // Load all of the WaveInfo scriptable objects in the project, even if they
        // aren't inside the EnemyInfo folder.
        _WaveList = Resources.LoadAll<Waves>("").ToList();

        SyncWavesWithPrefabs();
    }

    /// <summary>
    /// This function copies necessary values from the wave manager, so the UI stays up-to-date even
    /// when the wave manager is modified.
    /// 
    /// This function is just like TowerInfoCollection.SyncTowerInfoWithPrefabs(), but not currently
    /// being used. I just kept this here in case it is needed later.
    /// 
    /// NOTE: We might want to change this later by making it so all info is pulled from the
    ///       Wave scriptable objects rather than having some stored on the prefabs.
    /// </summary>
    private void SyncWavesWithPrefabs()
    {
    }

    public List<SpawnInfo> GetEnemySpawnInfo(int index)
    {
        return _WaveList[0].WaveInfo[index].Enemies;
    }

    public Wave GetWaveInfo(int index)
    {
        return _WaveList[0].WaveInfo[index];
    }



    public int Count { get { return _WaveList.Count; } }
}