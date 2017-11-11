using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketInfoPanel : MonoBehaviour
{
    public static RocketInfoPanel Instance;

    public GameObject RocketInfoPrefab;

    public RocketInfo[] RocketInfos { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var rocketInfoList = new List<RocketInfo>();
        for (int i = 0; i < MainManager.Instance.NumRockets; i++)
        {
            var rocketInfo = Instantiate(RocketInfoPrefab, transform).GetComponent<RocketInfo>();
            rocketInfoList.Add(rocketInfo);
        }

        RocketInfos = rocketInfoList.ToArray();
    }
}
