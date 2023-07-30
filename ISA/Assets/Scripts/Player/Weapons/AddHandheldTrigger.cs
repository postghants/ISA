using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHandheldTrigger : MonoBehaviour
{
    public HandheldScriptableObject handheld;
    private void OnTriggerEnter(Collider other)
    {
        carrierSystem carrier = other.GetComponentInChildren<carrierSystem>();
        if(carrier != null)
        {
            carrier.AddHandheld(handheld);
            gameObject.SetActive(false);
        }
    }
}
