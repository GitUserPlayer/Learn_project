using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
    //private float delaysum = 0;
    public AudioSource _Thud, _ThudHard, _Jump;
    public GameObject Dropdown_Effect, Dropdamage_Effect, Jump_Effect, WHAM;
    public int Health, MaxHealth, JumpForce;
    public bool IsGrounded ,IsJumping, _Dead= false;
    [SerializeField] bool _TakingDamage = false;
    

    // Update is called once per frame
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        CharacterController controller = GetComponent<CharacterController>();
    }
    void FixedUpdate()
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

        if (TotalVelocity.magnitude >= 0.1f && _Dead == false)
        {
         rb.velocity = new Vector3(TotalVelocity.x*speed, rb.velocity.y, TotalVelocity.z*speed);    
        }
        if (Input.GetKey(KeyCode.Space) & IsJumping == false)
        {
            StartCoroutine(Jumping(10, 1)); //Jumping(FORCE,Cooldown)

        }
        if (Input.GetKey(KeyCode.R))
        {
            rb.position = new Vector3(0,0,0);
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
                    GameObject _WHAM = Instantiate(WHAM, rb.position, Quaternion.identity);
                    _VFX.transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal + NewOrientation);
                    TakeDamage(collision.relativeVelocity.magnitude,true);

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

    IEnumerator Jumping(float Force ,int cooldown)
    {
        if (IsGrounded == true && IsJumping == false)
        {
            IsJumping = true;
            _Jump.Play();
            Instantiate(Jump_Effect,rb.position, Quaternion.identity);
            rb.AddForce(0, Force * JumpForce, 0);
            yield return new WaitForSeconds(cooldown);
            IsJumping = false;
        }
    }
    public void TakeDamage(float Force,bool IsFallDamage)
    {
        if (_TakingDamage == false)
        {
            _TakingDamage = true;
            if (IsFallDamage == false)
            {
                int TrueDamage = Mathf.RoundToInt(Force);
                Health -= TrueDamage;
                Debug.Log("Truedamage");
                if (Health <= 0)
                {
                    StartCoroutine(Death());
                }
            }
            else
            {
                int Damage = Mathf.RoundToInt(Force / 3);
                Health -= Damage * Damage;
                Debug.Log("Falldamage");
                Debug.Log(Damage);
                if (Health <= 0)
                {
                    StartCoroutine(Death());
                }
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
    private IEnumerator Death()
    {
        _Dead = true;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
