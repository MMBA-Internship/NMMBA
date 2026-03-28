using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CooldownTextCounter : MonoBehaviour
{
    [SerializeField] TMP_Text cooldownText;
    int timer = 3;

    private void OnEnable()
    {
        cooldownText.enabled = false;
        GameEvents.OnPhotoInputPressed += StartCooldown;
        GameEvents.OnPhotoCooldownEnded += StopCooldown;
    }

    private void OnDisable()
    {
        GameEvents.OnPhotoInputPressed -= StartCooldown;
        GameEvents.OnPhotoCooldownEnded -= StopCooldown;
    }

    private void StartCooldown()
    {
        cooldownText.enabled = true;
        timer = 3;
        cooldownText.text = timer.ToString();
        StartCoroutine(TextUpdater());
    }

    private void StopCooldown()
    {
        cooldownText.enabled = false;
    }


    IEnumerator TextUpdater()
    {
        while (timer > 0)
        {
            cooldownText.text = timer.ToString();
            timer--;
            yield return new WaitForSeconds(1);
        }
    }
}
