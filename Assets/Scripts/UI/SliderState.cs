using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderState : MonoBehaviour
{
    public Image hpsState;
    public Image hpsCurrent;
    private void Awake()
    {
        Transform parent = transform.GetChild(1);
        hpsState = parent.GetChild(0).GetComponent<Image>();
        hpsCurrent = parent.GetChild(1).GetComponent<Image>();
        hpsState.fillAmount = 1;
        hpsCurrent.fillAmount = 1;
    }
    public void refreshHps(float hpV, float spV)
    {
        //Debug.Log($"{hpV} {spV}");
        hpsCurrent.fillAmount = hpV;
    }
    public void ParticleHps(float time)
    {
        if (hpsState.fillAmount > hpsCurrent.fillAmount)
        {
            float diff = hpsState.fillAmount - hpsCurrent.fillAmount;
            hpsState.fillAmount -= diff * time;
        }
        else
        {
            hpsState.fillAmount = hpsCurrent.fillAmount;
        }
    }
    public void refreshPostion(Vector3 position)
    {
        this.transform.position = Camera.main.WorldToScreenPoint(position + new Vector3(0, 2, 0));
    }
}
