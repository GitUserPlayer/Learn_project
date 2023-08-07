using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    RectTransform rt;
    TextMeshProUGUI txt;
    TextMeshProUGUI txtLV;
    TextMeshProUGUI _Timer;
    UnityEngine.UI.Image hpbar;
    Vector3 Origin = new Vector3(0, 0, 0);
    float originWidth;    
    public int RETURNTIMER;
    [Tooltip("HP BAR SPRITE")]
    public GameObject GUI;
    [Tooltip("HEALTH IN NUMBER DISPLAY")]
    public GameObject HP;
    [Tooltip("This is obvious bro")]
    public GameObject LVL;
    public GameObject Clock;
    public GameObject gameover;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown());
        rt = GUI.GetComponent<RectTransform>();
        hpbar = GUI.GetComponent<UnityEngine.UI.Image>();
        txt = HP.GetComponent<TextMeshProUGUI>();
        txtLV = LVL.GetComponent<TextMeshProUGUI>();
        _Timer = Clock.GetComponent<TextMeshProUGUI>();
        originWidth = rt.sizeDelta.x;

    }

    // Update is called once per frame

    public void HP_Update(int Health, int MaxHealth)
    {
        if (Health <= 0)
        {
            txt.text = "DEAD LOL";
            rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);
        }
        else if (Health <= 25)
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
    public void LevelUI(int Level)
    {
        txtLV.text = "LEVEL " + Level;
        StopAllCoroutines();
        StartCoroutine(Countdown());

    }
    public IEnumerator Countdown()
    {
        int Timer = 30;
        while (Timer > 0)
        {
            Timer--;
            RETURNTIMER = Timer;
            yield return new WaitForSeconds(1);
            _Timer.text = Timer + " !!!";
            //float Color = Mathf.Lerp(255, 0, 1 - (Timer / 30f));
            //Debug.Log(Color);
            _Timer.color = new Color(1, Timer/30f, Timer / 30f, 1);
        }
    }
    public void GameOver()
    {
        StopAllCoroutines();
        gameover.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(false);
    }
    public void GameOut()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
