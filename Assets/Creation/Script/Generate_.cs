using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Generate_ : MonoBehaviour
{
    public GameObject Platform;
    public int Increment, _platlim;
    [SerializeField] Vector3 OriginPoint;
    [SerializeField] private List<GameObject> platforms = new List<GameObject>();
    [SerializeField] private List<GameObject> SpawnItems = new List<GameObject>();
    [SerializeField] private GameObject Spawnplatform;
    [SerializeField] private bool IsGenerating = false;
    public GameObject TXTIncrement, TXTLimit, TRIGGER;
    Vector3 SpawnHeight;
    // Start is called before the first frame update
    void Start()
    {
        OriginPoint = new Vector3(0, 0, 0);
        SpawnHeight = Spawnplatform.transform.position - OriginPoint;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void CustomizeIncrement()
    {
        string temp = TXTIncrement.GetComponent<TMP_InputField>().text;
        Increment = int.Parse(temp);
    }
    public void CustomizeLimit()
    {
        string temp = TXTLimit.GetComponent<TMP_InputField>().text;
        _platlim = int.Parse(temp);
    }
    public void Generate()
    {
        for (int PlatformsY = 0; PlatformsY < _platlim; PlatformsY++)
        {
            for (int PlatformsZ = 0; PlatformsZ < _platlim; PlatformsZ++)
            {
                for (int PlatformsX = 0; PlatformsX < _platlim; PlatformsX++)
                {
                    int RNG = UnityEngine.Random.Range(1, 10);
                    if (RNG != 7)
                    {
                        int WhatPlatform = UnityEngine.Random.Range(0, 6);
                        Vector3 SpawnpointX = new Vector3(OriginPoint.x + Increment * PlatformsX, OriginPoint.y - Increment * PlatformsY, OriginPoint.z + Increment * PlatformsZ);
                        GameObject SpawnedObject = Instantiate(platforms[WhatPlatform], SpawnpointX, Quaternion.identity);
                        Destroy(SpawnedObject, 30f);
                    }
                }
            }
        }
        OriginPoint = new Vector3(OriginPoint.x, OriginPoint.y - Increment * _platlim, OriginPoint.z);
        Vector3 Triggerpoint = new Vector3(OriginPoint.x - Increment * _platlim, OriginPoint.y + Increment + 1, OriginPoint.z - Increment * _platlim);
        GameObject TriG = Instantiate(TRIGGER, Triggerpoint, Quaternion.identity);
        TriG.transform.localScale = new Vector3(Increment * _platlim * 3, 1, Increment * _platlim * 3);
        TriG.transform.eulerAngles = new Vector3(0, 90, 0);
        OriginPoint = new Vector3(OriginPoint.x, OriginPoint.y - Increment / 2, OriginPoint.z);
    }
    public void Checkpoint(int Level)
    {
        Spawnplatform.transform.position = new Vector3(Spawnplatform.transform.position.x, OriginPoint.y + SpawnHeight.y, Spawnplatform.transform.position.z);
        GameObject Medkit = Instantiate(SpawnItems[0], Spawnplatform.transform.Find("Spawn").position, Quaternion.identity);
        Destroy(Medkit, 10f);
    }
}

///Vector3 Spawnpoint = new Vector3(OriginPoint.x + Increment * Platformnum, OriginPoint.y, OriginPoint.z);
//Platformnum++;
//Instantiate(Platform, Spawnpoint, Quaternion.identity);;

