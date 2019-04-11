using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Assets")]
    public RawImage jetOutline;
    
    private Color UIColor;
    public Color UIStartColor;
    public Color UIEndColor;
    
    [Header("References")]
    public PlayerController playerController;
    public GameManager GM;
    public RectTransform gunScaler;
    public RectTransform missileScaler;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public float defaultGunYScale;
    public float defaultMissileYScale;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        UIColor = Color.Lerp(UIStartColor, UIEndColor, (float)playerController.damage / 100);
        damageText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, UIColor);
        damageText.text = "DAMAGE " + playerController.damage + "%";
        damageText.color = jetOutline.color = UIColor;
        gunScaler.sizeDelta = new Vector3(gunScaler.rect.width, GM.gunAmmo * defaultGunYScale / 100);
        missileScaler.sizeDelta = new Vector3(missileScaler.rect.width, GM.missileAmmo * defaultMissileYScale / 100);
        scoreText.text = GM.score.ToString();
        highScoreText.text = GM.highScore.ToString();
    }
}