using System.Collections.Generic;
using UnityEngine;

public class Randomize : MonoBehaviour
{
    public GameObject Spawnpoint;
    [SerializeField] private List<GameObject> SpawnItems = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        int drng = Random.Range(1, 100);
        switch (drng)
        {
            case > 85:
                Destroy(Instantiate(SpawnItems[2], Spawnpoint.transform.position, Quaternion.identity), 30);
                break;
            case >= 75:
                Destroy(Instantiate(SpawnItems[3], Spawnpoint.transform.position, Quaternion.identity), 30);
                break;
            case >= 50:
                Destroy(Instantiate(SpawnItems[0], Spawnpoint.transform.position, Quaternion.identity), 30);
                break;
            case <= 25:
                for (int i = 0; i < Random.Range(1, 5); i++)
                {
                    GameObject lego = Instantiate(SpawnItems[1], Spawnpoint.transform.position, Quaternion.identity);
                    lego.GetComponent<Rigidbody>().AddForce(Random.Range(100, 500), Random.Range(200, 500), Random.Range(100, 500));
                }
                break;
            default: break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
//Instantiate(SpawnItems[0],Spawnpoint.transform.position,Quaternion.identity);
//GameObject lego = Instantiate(SpawnItems[1], Spawnpoint.transform.position, Quaternion.identity);
//lego.GetComponent<Rigidbody>().AddForce(Random.Range(100, 500), Random.Range(200, 500), Random.Range(100, 500));