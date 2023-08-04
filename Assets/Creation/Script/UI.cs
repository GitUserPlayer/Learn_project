using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    RectTransform rt;
    TextMeshProUGUI txt;
    TextMeshProUGUI txtLV;
    UnityEngine.UI.Image hpbar;
    Vector3 Origin = new Vector3(0, 0, 0);
    float originWidth;
    [Tooltip("HP BAR SPRITE")]
    public GameObject GUI;
    [Tooltip("HEALTH IN NUMBER DISPLAY")]
    public GameObject HP;
    [Tooltip("This is obvious bro")]
    public GameObject LVL;
    // Start is called before the first frame update
    void Start()
    {
        rt = GUI.GetComponent<RectTransform>();
        hpbar = GUI.GetComponent<UnityEngine.UI.Image>();
        txt = HP.GetComponent<TextMeshProUGUI>();
        txtLV = LVL.GetComponent<TextMeshProUGUI>();
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
    }
}
