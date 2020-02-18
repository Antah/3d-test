using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Slider gunReload;
    [SerializeField]
    private TextMeshProUGUI hpText, speedText, gunText, scoreText, carryText;
    [SerializeField]
    private CharacterControls playerControls;

    private void Start()
    {
        playerControls.elapsedGun.Where(x => x == 0f).Subscribe(_ => { ReloadGun(); });
        playerControls.speedBoostTimer.Subscribe(x => { SetSpeedText(x); });
        playerControls.gunBoostTimer.Subscribe(x => { SetGunText(x); });
        playerControls.carriedObject.Subscribe(x => { SetCarryText(x); });
        playerControls.score.Subscribe(x => { SetScoreText(x); });
    }

    private void SetSpeedText(float x)
    {
        if (x > 0f)
        {
            speedText.enabled = true;
            speedText.text = "Speed: " + (int)x;
        }
        else
            speedText.enabled = false;
    }

    private void SetGunText(float x)
    {
        if (x > 0f)
        {
            gunText.enabled = true;
            gunText.text = "Damage: " + (int)x;
        }
        else
            gunText.enabled = false;
    }

    private void SetScoreText(int x)
    {
        scoreText.text = "Score: " + x;
    }

    private void SetCarryText(PickupCollider x)
    {
        if (x != null)
            carryText.text = "Carrying: " + x.itemName;
        else
            carryText.text = "";
    }

    private void ReloadGun()
    {
        gunReload.value = 0f;
        gunReload.gameObject.SetActive(true);
        StartCoroutine(FillSlider(gunReload, playerControls.shootDelayGun));
    }

    IEnumerator FillSlider(Slider slider, float time)
    {
        float ti = 0.0f;
        float rateInit = 1f / time;
        while (ti < 1f)
        {
            ti += Time.deltaTime * rateInit;
            slider.value = Mathf.Lerp(0f, 1f, ti);
            yield return null;
        }
        slider.gameObject.SetActive(false);
    }
}
