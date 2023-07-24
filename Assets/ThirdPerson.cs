using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    public CharacterController controller;
    public Rigidbody rb;

    public float speed = 6f;

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 Position = GameObject.Find("Player").transform.position;
        Vector3 direction = new Vector3(horizontal, 0, vertical);   

        if (direction.magnitude >= 0.1f)
        {
         rb.velocity = direction*speed;
        }

    }
}
