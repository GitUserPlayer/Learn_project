using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.VersionControl;
using UnityEngine;
// WARNING!!! THIS SCRIPT IS NOT FIXED YET
// WARNING!!! THIS SCRIPT IS NOT FIXED YET
// WARNING!!! THIS SCRIPT IS NOT FIXED YET
// WARNING!!! THIS SCRIPT IS NOT FIXED YET
// WARNING!!! THIS SCRIPT IS NOT FIXED YET
// WARNING!!! THIS SCRIPT IS NOT FIXED YET
// WARNING!!! THIS SCRIPT IS NOT FIXED YET
public class Generate_ : MonoBehaviour
{
    public GameObject Platform;
    public int Increment, _platlim;
    [SerializeField] Vector3 OriginPoint;
    [SerializeField] private List<GameObject> platforms = new List<GameObject>();
    [SerializeField] private GameObject Spawnplatform;
    // Start is called before the first frame update
    void Start()
    {
        OriginPoint = new Vector3 (0, 0, 0);
        Vector3 Startpoint = new Vector3(20, 10, 20);
        Instantiate(Spawnplatform, Startpoint, Quaternion.identity);
        Generate();
        
    }

    // Update is called once per frame
    void Update()
    {

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
                        int WhatPlatform = UnityEngine.Random.Range(0, 3);
                        Vector3 SpawnpointX = new Vector3(OriginPoint.x + Increment * PlatformsX, OriginPoint.y - Increment * PlatformsY, OriginPoint.z + Increment * PlatformsZ);
                        Instantiate(platforms[WhatPlatform], SpawnpointX, Quaternion.identity);
                        Debug.Log("Spawning Type" + platforms[WhatPlatform].name);
                    }
                }
            }
        }
    }
}

///Vector3 Spawnpoint = new Vector3(OriginPoint.x + Increment * Platformnum, OriginPoint.y, OriginPoint.z);
//Platformnum++;
//Instantiate(Platform, Spawnpoint, Quaternion.identity);

