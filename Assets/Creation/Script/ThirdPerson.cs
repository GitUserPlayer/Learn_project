using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ThirdPerson : MonoBehaviour
{
    public UI ui;
    public Generate_ Generation;
    public CharacterController controller;
    public Rigidbody rb;
    Renderer mat;
    public float speed = 6f;
    public float RegenRate = 1.0f;
    //private float delaysum = 0;
    public AudioSource _Thud, _ThudHard, _Jump, _Pop, _Heal,lol;
    public GameObject Dropdown_Effect, Dropdamage_Effect, Jump_Effect, _Death, Res, GUI, HP,_HealUp,CAM,Electric;
    public int Health, MaxHealth, JumpForce;
    public int Levels = 1;
    public bool IsGrounded, IsJumping, _Dead, _Gamestarted = false;
    [SerializeField] GameObject SpawnPlatform;
    [SerializeField] bool _TakingDamage,gameover = false;


    void Start()
    {
        
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>();
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

        if (TotalVelocity.magnitude >= 0.1f && _Dead == false && gameover == false)
        {
            rb.velocity = new Vector3(TotalVelocity.x * speed, rb.velocity.y, TotalVelocity.z * speed);
        }
        if (Input.GetKey(KeyCode.Space) & IsJumping == false)
        {
            StartCoroutine(Jumping(10, 1)); //Jumping(FORCE,Cooldown)

        }
        if (Input.GetKey(KeyCode.R))
        {
            rb.velocity = new Vector3(0, 0, 0);
            rb.position = SpawnPlatform.transform.Find("Spawn").position;
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
            TakeDamage(1, false);
        }
        ui.HP_Update(Health, MaxHealth);
        if (ui.RETURNTIMER <= 0 && gameover == false)
        {
            CAM.SetActive(false);
            StartCoroutine(Death());
            lol.Play();
            gameover = true;
            ui.GameOver();
        }



    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (IsGrounded == false)
            {
                IsGrounded = true;
                ContactPoint contact = collision.GetContact(0);
                Vector3 NewOrientation = new Vector3(0, 0, -90);
                if (collision.relativeVelocity.magnitude >= 18)
                {
                    _ThudHard.Play();
                    GameObject _VFX = Instantiate(Dropdamage_Effect, contact.point, Quaternion.identity);
                    _VFX.transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal + NewOrientation);
                    TakeDamage(collision.relativeVelocity.magnitude, true);

                }
                else if (collision.relativeVelocity.magnitude >= 7)
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

    IEnumerator Jumping(float Force, int cooldown)
    {
        if (IsGrounded == true && IsJumping == false)
        {
            IsJumping = true;
            _Jump.Play();
            Instantiate(Jump_Effect, rb.position, Quaternion.identity);
            rb.AddForce(0, Force * JumpForce, 0);
            yield return new WaitForSeconds(cooldown);
            IsJumping = false;
        }
    }
    public void TakeDamage(float Force, bool IsFallDamage)
    {
        if (_TakingDamage == false)
        {
            _TakingDamage = true;
            if (IsFallDamage == false)
            {
                int TrueDamage = Mathf.RoundToInt(Force);
                Health -= TrueDamage;
                if (Health <= 0)
                {
                    Health = 0;
                    StartCoroutine(Death());
                }
            }
            else
            {
                float Damage = Force/3f;
                Damage = Damage* (1f + Levels/5f);
                Health -= Mathf.RoundToInt(Damage);
                if (Health <= 0)
                {
                    Health = 0;
                    StartCoroutine(Death());
                }
            }
            _TakingDamage = false;
        }
    }
    public void Heal(int amount,bool Overdrive)
    {
        if (_TakingDamage == false && Health <= MaxHealth)
        {
            if (Overdrive==false)
            {
                if (amount != 0) //If its not regenerative healing
                {
                    Health += amount;
                    _Heal.Play();
                    Instantiate(_HealUp, rb.position, Quaternion.identity);
                    if (Health >= MaxHealth)
                    {
                        Health = MaxHealth;
                    }
                }
            }
            else
            {
                if (amount != 0)
                {
                    MaxHealth += amount;
                    _Heal.Play();
                    Instantiate(_HealUp, rb.position, Quaternion.identity);
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
        if (_Dead == true && gameover == false)
        {
            rb.position = SpawnPlatform.transform.Find("Spawn").position;
            rb.isKinematic = false;
            float start = mat.material.GetFloat("_Visibility");
            float finish = 1;
            float lerp = 0;
            while (lerp < 1)
            {
                mat.material.SetFloat("_Visibility", 1 - Mathf.Lerp(start, finish, lerp));
                lerp += Time.deltaTime;
                yield return null;
            }
            Health = MaxHealth;
            Vector3 Particle = SpawnPlatform.transform.Find("Spawn").position;
            Vector3 NewOrientation = new Vector3(0, 0, -90);
            GameObject VFX_ = Instantiate(Res, Particle, Quaternion.identity);
            VFX_.transform.rotation = Quaternion.FromToRotation(Vector3.up, NewOrientation);
            ui.HP_Update(Health, MaxHealth);
            _Dead = false;
        }
    }
    public void LevelUp()
    {
        Levels++;
        ui.LevelUI(Levels);
        Generation.Checkpoint(Levels);
        if (Levels == 10)
        {
            ui.Warning();
        }
    }

   public void SpeedBoost()
    {
        speed += 1.5f;
        GameObject VFX = Instantiate(Electric,rb.position, Quaternion.identity);
        VFX.transform.SetParent(rb.transform, true);
    }
}
