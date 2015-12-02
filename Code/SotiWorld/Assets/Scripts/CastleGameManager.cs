using UnityEngine;
using System.Collections;

public class CastleGameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void Restartlevel()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void GoToMaze()
    {
        Application.LoadLevel("labyrinth");
    }
}
