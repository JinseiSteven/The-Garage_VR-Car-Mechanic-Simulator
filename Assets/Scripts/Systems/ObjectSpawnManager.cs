using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SpawnPointTypes
{
    Fluid,
    Jumpstarter,
    Wheel,
    Key,
    ClothRag
}


public class ObjectSpawnManager : MonoBehaviour
{
    public static ObjectSpawnManager instance;
    public static bool IsReady = false;
    void Awake()
    {
        CreateSingleton();
        IsReady = true;
    }

    [Header("Designated Spawn Points")]
    public List<GameObject> fluidSpawnPoints = new List<GameObject>();
    public List<GameObject> jumpstarterSpawnPoints = new List<GameObject>();
    public List<GameObject> wheelSpawnPoints = new List<GameObject>();
    public List<GameObject> keySpawnPoints = new List<GameObject>();
    public List<GameObject> clothRagSpawnPoints = new List<GameObject>();

    [Header("Required GameObjects")]
    [SerializeField] GameObject kruissleutel;
    [SerializeField] GameObject jumpstarter;
    [SerializeField] GameObject clothRag;
    [SerializeField] GameObject wheel;
    [SerializeField] List<GameObject> fluids = new List<GameObject>();

    private void Start() {
        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(0.1f);
        SpawnAllObjects();
    }

    public void AddSpawnPoint(SpawnPointTypes type, GameObject spawnPoint)
    {
        switch (type)
        {
            case SpawnPointTypes.Fluid:
                fluidSpawnPoints.Add(spawnPoint);
                break;
            case SpawnPointTypes.Jumpstarter:
                jumpstarterSpawnPoints.Add(spawnPoint);
                break;
            case SpawnPointTypes.Wheel:
                wheelSpawnPoints.Add(spawnPoint);
                break;
            case SpawnPointTypes.Key:
                keySpawnPoints.Add(spawnPoint);
                break;
            case SpawnPointTypes.ClothRag:
                clothRagSpawnPoints.Add(spawnPoint);
                break;
        }
    }

    public void SpawnFluidObjects()
    {
        if (fluidSpawnPoints.Count < fluids.Count)
        {
            Debug.LogError("Not enough spawnpoints for the amount of fluids!");
            return;
        }

        if (clothRag == null || fluids.Count < 1)
        {
            Debug.LogError("ClothRag has not been assigned to the object spawn manager! Or no fluids have been assigned!");
            return;
        }

        List<int> indices = Enumerable.Range(0, fluidSpawnPoints.Count).ToList();

        foreach (GameObject fluid in fluids)
        {
            int index = Random.Range(0, indices.Count);
            GameObject spawnPoint = fluidSpawnPoints[indices[index]];
            fluid.transform.position = spawnPoint.transform.position;
            fluid.transform.rotation = spawnPoint.transform.rotation;

            indices.RemoveAt(index);
        }

        RandomizePosition(ref clothRagSpawnPoints, ref clothRag);
    }

    public void SpawnWheelObjects()
    {
        if (wheel == null || kruissleutel == null)
        {
            Debug.LogError("Wheel or kruissleutel has not been assigned to the object spawn manager!");
        }
        RandomizePosition(ref wheelSpawnPoints, ref wheel);
        RandomizePosition(ref keySpawnPoints, ref kruissleutel);
    }
    public void SpawnJumpstarterObjects()
    {
        if (jumpstarter == null)
        {
            Debug.LogError("Jumpstarter has not been assigned to the object spawn manager!");
        }
        RandomizePosition(ref jumpstarterSpawnPoints, ref jumpstarter);
    }

    public void SpawnAllObjects()
    {
        SpawnFluidObjects();
        SpawnWheelObjects();
        SpawnJumpstarterObjects();
    }

    private void RandomizePosition(ref List<GameObject> spawnObjects, ref GameObject target)
    {
        if (spawnObjects.Count <= 0)
        {
            Debug.LogError("No valid spawnlocations found for " + target);
            return;
        }
        int index = Random.Range(0, spawnObjects.Count);
        target.transform.position = spawnObjects[index].transform.position;
        target.transform.rotation = spawnObjects[index].transform.rotation;
    }

    void CreateSingleton()
    {
        if (instance == null)
            instance = this;
        else

            // we gotta destroy any other objects with this script since we want it to be a singleton
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
