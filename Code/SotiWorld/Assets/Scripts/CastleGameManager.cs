using System;
using UnityEngine;
using System.Collections;
using System.Globalization;
using Assets.Scripts;
using Assets.Scripts.Integration;
using UnityEditor;
using UnityEngine.UI;

public class CastleGameManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
                DenyMazeAccess("Credentials must be in the format userName:password:clientId:clientSecret");
                return;
            }

            string userName = credentialTokens[0];
            string password = credentialTokens[1];
            string clientId = credentialTokens[2];
            string clientSecret = credentialTokens[3];

            publicApiGateway = new PublicApiGateway(instanceUrlText.text, clientId, clientSecret);
            try
            {
                publicApiGateway.Login(userName, password);
                Game.GenerateLabyrinth(publicApiGateway);
            }
            catch (Exception ex)
            {
                DenyMazeAccess("Failed to retrieve data from MobiControl public API. Make sure that the values you entered are correct.");
                return;
            }
        }

        Application.LoadLevel("labyrinth");
    }

    private void DenyMazeAccess(string reason)
    {
        EditorUtility.DisplayDialog("Access to the maze denied!", reason, "Ok");
        Restartlevel();
    }
}
