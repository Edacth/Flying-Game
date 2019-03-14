using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public PlayerController playerController;
    public float gunAmmo;
    public float missileAmmo;
    public RawImage jetOutline;
    public TextMeshProUGUI damageText;
    private Color UIColor;
    public Color UIStartColor;
    public Color UIEndColor;
    
    void Update()
    {
        UIColor = Color.Lerp(UIStartColor, UIEndColor, (float)playerController.damage / 100);
        damageText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, UIColor);
        damageText.text = "DAMAGE " + playerController.damage + "%";
        damageText.color = jetOutline.color = UIColor;
    }
}