using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Generated.PropertyProviders;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ThirdPerson : MonoBehaviour
{
    public CharacterController controller;
    public Rigidbody rb;
    public float speed = 6f;
    public float RegenRate = 1.0f;
    public AudioSource _Thud, _ThudHard, _Jump;
    public GameObject Dropdown_Effect, Dropdamage_Effect;
    public int Health, MaxHealth, JumpForce;
    public bool IsGrounded = false;
    [SerializeField] bool _TakingDamage = false;
    

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
        float delaysum = 0;
        delaysum += Time.deltaTime;
        if (delaysum > 1.0f)
        {
            Debug.Log("Five Seconds have passed");
        }

        

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (IsGrounded == false)
            {
                Debug.Log(collision.relativeVelocity.magnitude);
                IsGrounded = true;
                ContactPoint contact = collision.GetContact(0);
                Vector3 NewOrientation = new Vector3(0, 0, -90);
                if (collision.relativeVelocity.magnitude >= 18)
                {
                    _ThudHard.Play();
                    GameObject _VFX = Instantiate(Dropdamage_Effect, contact.point, Quaternion.identity);
                    _VFX.transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal + NewOrientation);
                    TakeDamage(collision.relativeVelocity.magnitude);

                }
                else if(collision.relativeVelocity.magnitude >= 7)
                {
                    _Thud.Play();
                    GameObject _VFX = Instantiate(Dropdown_Effect, contact.point, Quaternion.identity);
                    _VFX.transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal + NewOrientation);
                }
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
    public void TakeDamage(float Force)
    {
        if (_TakingDamage == false)
        {
            _TakingDamage = true;
            int Damage = Mathf.RoundToInt(Force / 3);
            Health -= Damage * Damage;
            Debug.Log(Damage);
            if (Health <= 0)
            {
                SceneManager.LoadScene(0);
            }
            _TakingDamage = false;
        }
        
    }
    public void Heal(int amount)
    {
        if (_TakingDamage == false && Health <= MaxHealth)
        {
            if (amount != 0) //If its not regenerative healing
            {
                Health += amount;
                if (Health <= MaxHealth)
                {
                    Health = MaxHealth;
                }
            }
        }
    }
}
