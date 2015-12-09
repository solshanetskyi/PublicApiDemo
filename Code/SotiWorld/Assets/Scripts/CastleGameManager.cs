using System;
using UnityEngine;
using System.Collections;
using System.Globalization;
using Assets.Scripts;
using Assets.Scripts.Integration;
using UnityEngine.UI;

public class CastleGameManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject mazeMenu;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !mazeMenu.activeSelf)
        {
            mainMenu.SetActive(!mainMenu.activeSelf);
            Time.timeScale = mainMenu.activeSelf ? 0 : 1;
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    public void GoToMaze()
    {
        var instanceUrlText = GameObject.Find("InstanceUrlText").GetComponent<Text>();
        var credentialsTextBox = GameObject.Find("CredentialsText").GetComponent<Text>();

        IPublicApiGateway publicApiGateway;
        if (instanceUrlText.text.Equals("mock", StringComparison.OrdinalIgnoreCase))
            publicApiGateway = new PublicApiGatewayMock();
        else
        {
            string[] credentialTokens = credentialsTextBox.text.Split(new[] { ":" }, StringSplitOptions.None);
            if (credentialTokens.Length != 4)
            {
                ShowMessage("Credentials must be in the format userName:password:clientId:clientSecret");
                return;
            }

            string userName = credentialTokens[0];
            string password = credentialTokens[1];
            string clientId = credentialTokens[2];
            string clientSecret = credentialTokens[3];

            publicApiGateway = new PublicApiGateway(instanceUrlText.text, clientId, clientSecret);
            try
            {
                ShowMessage("Please wait...");
                publicApiGateway.Login(userName, password);
                Game.GenerateLabyrinth(publicApiGateway);
            }
            catch (Exception ex)
            {
                ShowMessage("Failed to retrieve data from MobiControl public API. Make sure that the values you entered are correct.");
                return;
            }
        }

        Application.LoadLevel("labyrinth");
    }

    private void ShowMessage(string reason)
    {
        var errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        errorText.text = reason;
    }
}
