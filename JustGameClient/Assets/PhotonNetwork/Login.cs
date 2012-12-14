using UnityEngine;
using System.Collections;
using System;
using ExitGames.Client.Photon;

public class Login : MonoBehaviour
{
    private PhotonServer m_photonServer;
    private bool m_isConnected;

    public string m_login;
    public string m_password;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        m_photonServer = new PhotonServer();
        PhotonPeer peer = new PhotonPeer(m_photonServer, ConnectionProtocol.Udp);
        m_photonServer.Init(peer, "localhost:5055", "JustGameServer", this);
        m_isConnected = true;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (m_isConnected)
            {
                m_photonServer.Update();
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    void OnApplicationQuit()
    {
        try
        {
            m_photonServer.Disconnect();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, Screen.height - 25, 300, 20), m_photonServer.Status);

        // Make a group on the center of the screen
        GUI.BeginGroup(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 130));
        // All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

        // We'll make a box so you can see where the group is on-screen.
        GUI.Box(new Rect(0, 0, 300, 130), "Login");

        m_login = GUI.TextField(new Rect(5, 35, 290, 25), m_login, 1000);
        m_password = GUI.PasswordField(new Rect(5, 65, 290, 25), m_password, '*', 1000);

        if (GUI.Button(new Rect(5, 95, 290, 30), "Login"))
            m_photonServer.Login(m_login, m_password);

        // End the group we started above. This is very important to remember!
        GUI.EndGroup();
    }

    public void DebugMessage(string message)
    {
        Debug.Log(message);
    }
}