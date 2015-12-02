using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class CastleEntranceCollider : MonoBehaviour {
    public GameObject menu;
    public GameObject fps;

    // Update is called once per frame
    void OnTriggerEnter (Collider collider)
	{
	    if (collider.tag == "Player")
	    {
            menu.SetActive(true);

            fps.GetComponent<FirstPersonController>().enabled = false;
        }
	}
}
