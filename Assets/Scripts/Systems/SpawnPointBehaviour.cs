using System.Collections;
using UnityEngine;

public class SpawnPointBehaviour : MonoBehaviour
{
    [SerializeField] private SpawnPointTypes spawnPointType = SpawnPointTypes.Fluid;
    private void Awake()
    {
        StartCoroutine(InitializeWithSingleton());
    }

    private IEnumerator InitializeWithSingleton()
    {
        while (!ObjectSpawnManager.IsReady)
        {
            yield return null;
        }

        Debug.Log("Spawnpoint Added");
        ObjectSpawnManager.instance.AddSpawnPoint(spawnPointType, gameObject);
    }
}
