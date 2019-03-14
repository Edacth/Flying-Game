using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public int damage;
    public RawImage jetOutline;
    public TextMeshProUGUI damageText;
    Color UIColor;
    public Color UIStartColor;
    public Color UIEndColor;

    void Update()
    {
        UIColor = Color.Lerp(UIStartColor, UIEndColor, (float)damage / 100);
        damageText.text = "DAMAGE " + damage + "%";
        damageText.color = jetOutline.color = UIColor;
    }
}