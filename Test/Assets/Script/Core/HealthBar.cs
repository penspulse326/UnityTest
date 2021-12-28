using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject rootCanvas;
    [SerializeField] private Image foreground;
    [Range(0,1)]
    [SerializeField] float changeHealthRatio = 0.1f;

    void Update()
    {
        //血量百分比約等於0或1時不顯示
        if(Mathf.Approximately(health.GetHealthRatio(),0) ||
        Mathf.Approximately(health.GetHealthRatio(), 1))
        {
            rootCanvas.SetActive(false);
            return;
        }

        rootCanvas.SetActive(true);
        rootCanvas.transform.LookAt(Camera.main.transform);

        foreground.fillAmount = Mathf.Lerp(foreground.fillAmount,health.GetHealthRatio(),changeHealthRatio);
    }
}
