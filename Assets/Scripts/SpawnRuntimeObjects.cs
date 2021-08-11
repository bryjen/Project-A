using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRuntimeObjects : MonoBehaviour
{
    private static SpawnRuntimeObjects _instance;
    public static SpawnRuntimeObjects Instance { get { return _instance;  } }

    public GameObject spawnedEnemyParent { get; private set; }
    public GameObject spawnedSentinelParent { get; private set; }
    public GameObject previewSentinelParent { get; private set; }
    public GameObject spawnedProjectilesParent { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        
        spawnedEnemyParent = new GameObject("---- [Runtine] Spawned Enemies ----");
        spawnedSentinelParent = new GameObject("---- [Runtime] Spawned Sentinels ----");
        previewSentinelParent = new GameObject("---- [Runtime] Previews ----");
        spawnedProjectilesParent = new GameObject("---- [Runtime] Projectiles ----");
    }
}
