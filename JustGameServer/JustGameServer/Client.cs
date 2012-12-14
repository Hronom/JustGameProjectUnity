using System.Collections.Generic;
using Photon.SocketServer;
using ExitGames.Logging;
using PhotonHostRuntimeInterfaces;

namespace JustGameServer
{
    public class Client : PeerBase
    {
        private readonly ILogger m_log = LogManager.GetCurrentClassLogger();

        public Client(IRpcProtocol protocol, IPhotonPeer peer)
            : base(protocol, peer)
        {
            m_log.Debug("New client: " + peer.GetRemoteIP());
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            m_log.Debug("Client disconnected: " + this.RemoteIP + ":" + this.RemotePort);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            switch (operationRequest.OperationCode)
            {
                case 1:
                    if (operationRequest.Parameters.ContainsKey(1))
                    {
                        m_log.Debug("Login:" + operationRequest.Parameters[1] + " password:" + operationRequest.Parameters[2]);
                        if (operationRequest.Parameters[2].Equals("1"))
                        {
                            OperationResponse response;
                            response = new OperationResponse(operationRequest.OperationCode);

                            Dictionary<byte, object> dictionary;
                            dictionary = new Dictionary<byte, object>();
                            dictionary.Add(1, "Accepted");
                            response.SetParameters(dictionary);

                            SendOperationResponse(response, sendParameters);
                        }
                        else
                        {
                            OperationResponse response;
                            response = new OperationResponse(operationRequest.OperationCode);

                            Dictionary<byte, object> dictionary;
                            dictionary = new Dictionary<byte, object>();
                            dictionary.Add(1, "Bad password");
                            response.SetParameters(dictionary);

                            SendOperationResponse(response, sendParameters);
                        }
                    }
                    break;
                /*case 2:
                    if (operationRequest.Parameters.ContainsKey(1))
                    {
                        Log.Debug("recv:" + operationRequest.Parameters[1]);
                        EventData eventdata = new EventData(1);
                        eventdata.Parameters = new Dictionary<byte, object> { { 1, "response for event" } };
                        SendEvent(eventdata, sendParameters);
                    }
                    break;*/
                default:
                    m_log.Debug("Unknown OperationRequest received!:" + operationRequest.OperationCode);
                    break;
            }
        }
    }
}