using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;

public class CharacterCollider : MonoBehaviour
{
    public GameObject manageDevicePanel;
    public GameObject manageDevicePrompt;

    void OnTriggerEnter(Collider other)
    {
        manageDevicePrompt.GetComponent<Text>().text = String.Format("Press M to manage device '{0}'", Game.GetDeviceById(other.name).Name);
        manageDevicePanel.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        manageDevicePanel.SetActive(false);
    }
}
