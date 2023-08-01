using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Generated.PropertyProviders;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ThirdPerson : MonoBehaviour
{
    public CharacterController controller;
    public Rigidbody rb;
    Renderer mat;
    public float speed = 6f;
    public float RegenRate = 1.0f;
    //private float delaysum = 0;
    public AudioSource _Thud, _ThudHard, _Jump, _Pop;
    public GameObject Dropdown_Effect, Dropdamage_Effect, Jump_Effect, _Death,Res,GUI,HP;
    public int Health, MaxHealth, JumpForce;
    public bool IsGrounded ,IsJumping, _Dead, _Gamestarted= false;
    [SerializeField] GameObject SpawnPlatform;
    [SerializeField] bool _TakingDamage = false;
    RectTransform rt;
    TextMeshProUGUI txt;
    UnityEngine.UI.Image hpbar;
    Vector3 Origin = new Vector3(0,0,0);
    float originWidth;
    

    void Start()
    {
        Camera.main.fieldOfView = 120f;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>();
        rt = GUI.GetComponent<RectTransform>();
        hpbar = GUI.GetComponent<UnityEngine.UI.Image>();
        txt = HP.GetComponent<TextMeshProUGUI>();
        originWidth = rt.sizeDelta.x;
        Application.targetFrameRate = 144;
        

    }
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
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
            rb.position = new Vector3(14.47f, 11.28f, 5.68f);
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (UnityEngine.Cursor.lockState == CursorLockMode.Locked)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
        }
        if (rb.velocity.y < -20)
        {
            TakeDamage(1,false);
        }
        HP_Update(Health);



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
                Debug.Log("Lerped");
                if (Health <= 0)
                {
                    Health = 0;
                    StartCoroutine(Death());
                }
            }
            else
            {
                int Damage = Mathf.RoundToInt(Force / 3);
                Health -= Damage * Damage;
                Debug.Log("Falldamage");
                Debug.Log(Damage);
                Debug.Log("Lerped");
                if (Health <= 0)
                {
                    Health = 0;
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
        rb.isKinematic = true;
        float start = mat.material.GetFloat("_Visibility");
        float finish = 1;
        float lerp = 0;
        Instantiate(_Death, rb.position, Quaternion.identity);
        _Pop.Play();
        while (lerp < 1)
        {
            mat.material.SetFloat("_Visibility", Mathf.Lerp(start, finish, lerp));
            lerp += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(Respawn());
        
    }
    private IEnumerator Respawn()
    {
        if (_Dead == true)
        {
            rb.position = new Vector3(14.47f, 11.28f, 5.68f);
            rb.isKinematic = false;
            float start = mat.material.GetFloat("_Visibility");
            float finish = 1;
            float lerp = 0;
            while (lerp < 1)
            {
                mat.material.SetFloat("_Visibility", 1-Mathf.Lerp(start, finish, lerp));
                lerp += Time.deltaTime;
                yield return null;
            }
            Health = MaxHealth;
            Vector3 Particle = new Vector3(14.51f, 9.53f, 5.47f);
            Vector3 NewOrientation = new Vector3(0, 0, -90);
            GameObject VFX_ =Instantiate(Res, Particle,Quaternion.identity);
            VFX_.transform.rotation = Quaternion.FromToRotation(Vector3.up,NewOrientation);
            txt.text = "HP("+Health+"/"+MaxHealth+")";
            _Dead = false;
        }
    }
    public void HP_Update(int Health)
    {
        if (Health <= 0)
        {
            txt.text = "DEAD LOL";
            rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);
            Debug.Log(rt.sizeDelta.x);
        }
        else if(Health <= 25)
        {
            float lerp = (float)Health / (float)MaxHealth;
            hpbar.color = Color.red;
            rt.sizeDelta = new Vector2(Mathf.Lerp(0, originWidth, lerp), rt.sizeDelta.y);
            txt.text = "HP(" + Health + "/" + MaxHealth + ")";
            txt.color = hpbar.color;
        }
        else if (Health <= 75)
        {
            float lerp = (float)Health / (float)MaxHealth;
            hpbar.color = Color.yellow;
            rt.sizeDelta = new Vector2(Mathf.Lerp(0, originWidth, lerp), rt.sizeDelta.y);
            txt.text = "HP(" + Health + "/" + MaxHealth + ")";
            txt.color = hpbar.color;
        }
        else
        {
            float lerp = (float)Health / (float)MaxHealth;
            hpbar.color = new Color32(130, 222, 122, 255);
            rt.sizeDelta = new Vector2(Mathf.Lerp(0, originWidth, lerp), rt.sizeDelta.y);
            txt.text = "HP(" + Health + "/" + MaxHealth + ")";
            txt.color = hpbar.color;
        }
    }
}
