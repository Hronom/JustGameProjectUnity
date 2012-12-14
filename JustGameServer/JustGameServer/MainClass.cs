using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using System.IO;

namespace JustGameServer
{
    public class MainClass : ApplicationBase
    {
        protected override void Setup()
        {
            FileInfo file = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if(file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }
        }

        protected override void TearDown()
        {
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new Client(initRequest.Protocol, initRequest.PhotonPeer);
        }
    }
}
