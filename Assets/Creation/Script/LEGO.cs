using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LEGO : MonoBehaviour
{
    [SerializeField] private AudioSource lol;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            other.gameObject.GetComponent<ThirdPerson>().TakeDamage(Random.Range(1, 36), false);
            lol.Play();
        }
    }
}
