
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gameObject.name)
            {
                case "Medkit(Clone)":
                    other.gameObject.GetComponent<ThirdPerson>().Heal(Random.Range(25, 75), false);
                    Destroy(gameObject);
                    break;
                case "Godkit(Clone)":
                    other.gameObject.GetComponent<ThirdPerson>().Heal(Random.Range(25, 50), true);
                    Destroy(gameObject);
                    break;
                default:
                    Debug.Log("Invalid Item request");
                    break;
            }
        }
    }
}