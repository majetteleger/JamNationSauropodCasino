using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketContainer : MonoBehaviour
{
    public GameObject RocketPrefab;
    public GameObject RocketSpawnPointPrefab;
    public float Width;

    public Transform[] RocketSpawnPoints { get; set; }
    
    private void Start()
    {
        var rocketSpawnPointsList = new List<Transform>();
        for (int i = 0; i < MainManager.Instance.NumRockets; i++)
        {
            var rocketSpawnPoint = Instantiate(RocketSpawnPointPrefab, transform).transform;
            rocketSpawnPoint.localPosition = new Vector3((-Width / 2) + ((Width / (MainManager.Instance.NumRockets - 1)) * i), rocketSpawnPoint.localPosition.y, rocketSpawnPoint.localPosition.z);

            rocketSpawnPointsList.Add(rocketSpawnPoint);
        }

        RocketSpawnPoints = rocketSpawnPointsList.ToArray();
    }

    public Rocket TrySpawnRocket()
    {
        var availableIndicesList = new List<int>();

        for(int i = 0; i < RocketSpawnPoints.Length; i++)
        {
            var childRocket = RocketSpawnPoints[i].GetComponentInChildren<Rocket>();

            if (childRocket == null)
            {
                availableIndicesList.Add(i);
            }
        }

        var availableIndices = availableIndicesList.ToArray();

        if(availableIndices.Length > 0)
        {
            var randomIndex = availableIndices[Random.Range(0, availableIndices.Length)];
            return SpawnRocket(randomIndex);
        }

        return null;
    }

    private Rocket SpawnRocket(int spawnPointIndex)
    {
        var rocket = Instantiate(RocketPrefab, RocketSpawnPoints[spawnPointIndex]).GetComponent<Rocket>();
        rocket.RocketInfo = RocketInfoPanel.Instance.RocketInfos[spawnPointIndex];

        return rocket;
    }
}
