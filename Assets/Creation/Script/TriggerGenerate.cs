using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TriggerGenerate : MonoBehaviour
{
    [SerializeField] private bool NotGenerating = true;
    public GameObject Seed;
    public UI ui;
    public ThirdPerson TP;
    [SerializeField] private int Levels = 1;
    //public GameObject Platform;
   // public int Increment, _platlim;
    //[SerializeField] Vector3 OriginPoint;
    //[SerializeField] private List<GameObject> platforms = new List<GameObject>();
    //[SerializeField] private GameObject Spawnplatform;
    //public GameObject TXTIncrement, TXTLimit, TRIGGER;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && NotGenerating == true)
        {
            NotGenerating = false;
            TP.LevelUp();
            Seed.GetComponent<Generate_>().Generate();
            Destroy(gameObject);
            
        }
        else if (other.CompareTag("Debris"))
        {
            Destroy(other.gameObject);
        }

    }
}
