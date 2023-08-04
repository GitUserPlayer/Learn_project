using System.Collections;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    public UI ui;
    public Generate_ Generation;
    public CharacterController controller;
    public Rigidbody rb;
    Renderer mat;
    [Range(0, 6)] public float speed = 6f;
    public float RegenRate = 1.0f;
    //private float delaysum = 0;
    public AudioSource _Thud, _ThudHard, _Jump, _Pop;
    public GameObject Dropdown_Effect, Dropdamage_Effect, Jump_Effect, _Death, Res, GUI, HP;
    public int Health, MaxHealth, JumpForce;
    public int Levels = 1;
    public bool IsGrounded, IsJumping, _Dead, _Gamestarted = false;
    [SerializeField] GameObject SpawnPlatform;
    [SerializeField] bool _TakingDamage = false;


    void Start()
    {
        Camera.main.fieldOfView = 120f;
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

        if (TotalVelocity.magnitude >= 0.1f && _Dead == false)
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
                int Damage = Mathf.RoundToInt(Force / 3);
                Health -= Damage * Damage;
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
                if (Health >= MaxHealth)
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
    }
}
