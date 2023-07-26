using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using UnityEngine;
public class ThirdPerson : MonoBehaviour
{
    public CharacterController controller;
    public Rigidbody rb;
    public float speed = 6f;
    public bool IsGrounded = false;
    public AudioSource _Thud;
    public AudioSource _Jump;
    public int JumpForce;
    public GameObject Dropdown_Effect;
    

    // Update is called once per frame
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        CharacterController controller = GetComponent<CharacterController>();
    }
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float jump = Input.GetAxis("Jump");
        Vector3 CameraFoward = Camera.main.transform.forward;//Getting Camera Angle
        Vector3 CameraRight = Camera.main.transform.right;
        CameraFoward.y = 0;//Prevents flying
        CameraFoward = CameraFoward.normalized;//Normalize camera angle to unit vector(length of 1)
        CameraRight.y = 0;//Same as above
        CameraRight = CameraRight.normalized;
        Vector3 PlayerFVelocity = CameraFoward * vertical;
        Vector3 PlayerHVelocity = CameraRight * horizontal;
        Vector3 TotalVelocity = PlayerFVelocity + PlayerHVelocity;

        if (TotalVelocity.magnitude >= 0.1f)
        {
         rb.velocity = new Vector3(TotalVelocity.x*speed, rb.velocity.y, TotalVelocity.z*speed);    
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Jumping(jump);
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (IsGrounded == false)
            {
                IsGrounded = true;
                _Thud.Play();
                ContactPoint contact = collision.GetContact(0);
                Vector3 NewOrientation = new Vector3(0, 0, -90);
                GameObject _VFX = Instantiate(Dropdown_Effect, contact.point, Quaternion.identity);
                _VFX.transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal+NewOrientation);
            }
        }

    }
    void OnCollisionExit(Collision collision)
    {
        IsGrounded = false;
    }

    void Jumping(float Force)
    {
        if (IsGrounded == true)
        {
            _Jump.Play();
            rb.AddForce(0, Force * JumpForce, 0);
        }
    }

}
