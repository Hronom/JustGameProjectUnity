using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PhotonServer : IPhotonPeerListener
{
    public string Status { get; set; }

    PhotonPeer m_peer;
    Login m_login;

    public PhotonServer()
    {
        Status = "Disconnected";
    }

    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnEvent(EventData eventData)
    {
        switch (eventData.Code)
        {
            case 1:
                if (eventData.Parameters.ContainsKey(1))
                {
                    m_login.DebugMessage("Event:" + eventData.Parameters[1]);
                }
                break;
            default:
                m_login.DebugMessage("Unknown Event:" + eventData.Code);
                break;
        }
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.OperationCode)
        {
            case 1:
                if (operationResponse.Parameters.ContainsKey(1))
                {
                    m_login.DebugMessage("OperationResponse:" + operationResponse.Parameters[1]);
                }
                break;
            default:
                m_login.DebugMessage("Unknown OperationResponse:" + operationResponse.OperationCode);
                break;
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                Status = "Connected";
                break;
            case StatusCode.Disconnect:
            case StatusCode.DisconnectByServer:
            case StatusCode.DisconnectByServerLogic:
            case StatusCode.DisconnectByServerUserLimit:
            case StatusCode.TimeoutDisconnect:
                Status = "Disconnected";
                break;
            default:
                Status = "Unknown";
                break;
        }
    }

    public void Init(PhotonPeer peer, string serverAddress, string applicationName, Login login)
    {
        m_peer = peer;
        m_peer.Connect(serverAddress, applicationName);
        m_login = login;
    }

    public void Disconnect()
    {
        m_peer.Disconnect();
    }

    public void Update()
    {
        m_peer.Service();
    }

    //=============================== Operations ===============================	
    public void Login(string name, string password)
    {
        Dictionary<byte, object> dictionary;
        dictionary = new Dictionary<byte, object>();
        dictionary.Add(1, name);
        dictionary.Add(2, password);

        m_peer.OpCustom(1, dictionary, false);
    }
}