using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBar : MonoBehaviour
{
    public Image healthBar;
    public Image healthLostBar;
    public Image powerBar;

    [Header("基本参数")] public float downHealthSpeed;

    private void Update()
    {
        if (healthLostBar.fillAmount > healthBar.fillAmount) healthLostBar.fillAmount -= Time.deltaTime * downHealthSpeed;
    }

    public void HealthChange(float heathPercent)
    {
        healthBar.fillAmount = heathPercent;
    }
    
    
}
