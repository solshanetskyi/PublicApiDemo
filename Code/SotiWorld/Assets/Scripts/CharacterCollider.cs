using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterCollider : MonoBehaviour
{
    public GameObject manageDevicePanel;
    public GameObject manageDevicePrompt;

    void OnTriggerEnter(Collider other)
    {
        manageDevicePrompt.GetComponent<Text>().text = String.Format("Press M to manage device '{0}'", other.name);
        manageDevicePanel.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        manageDevicePanel.SetActive(false);
    }
}
