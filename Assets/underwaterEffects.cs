using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class underwaterEffects : MonoBehaviour
{

    [SerializeField] GameObject WaterFX;

    private void OnTriggerEnter(Collider other)
    {
        WaterFX.gameObject.SetActive(true);
        RenderSettings.fog = true;

    }

    private void OnTriggerExit(Collider other)
    {
        WaterFX.gameObject.SetActive(false);
        RenderSettings.fog = false;

    }

}
