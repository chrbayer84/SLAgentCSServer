/**
 * @(#) CSSLAgentServer.cs
 *
 * Copyright (c) 2008-2009, Christian Bayer, christian_bay@gmx.de
 *
 * - Redistribution and use in source and binary forms, with or without
 *   modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * - Neither the name of the Second Life Reverse Engineering Team nor the names
 *   of its contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Nwc.XmlRpc;

namespace CSSLAgentServer
{
    [XmlRpcExposed]
    public class CSSLAgentServer : IXmlRpcSLConnector
    {
        const int port = 5050;
        static XmlRpcServer XmlRpcSLConnector;
        static Dictionary<string, SLAgentCS> agentAccountMap;

        /// <summary>
        /// <c>LoggerDelegate</c> compliant method that does logging to Console.
        /// This method filters out the <c>LogLevel.Information</c> chatter.
        /// </summary>
        public static void WriteEntry(string msg, LogLevel level)
        {
            if (level > LogLevel.Information) // ignore debug msgs
                Console.WriteLine("{0}: {1}", level, msg);
        }

        SLAgentCS mapAgentAccount(string agentName)
        {
            if (!(agentAccountMap.ContainsKey(agentName)))
            {
                //SLAgentList.Add(new SLAgentCS());
                agentAccountMap.Add(agentName, new SLAgentCS());
            }
            return agentAccountMap[agentName];
        }

        [XmlRpcExposed]
        public bool connectWithLogin(string agentName, string firstName, string lastName, string password)
        {
            Console.Out.WriteLine("Logging in {0}", agentName);

            return mapAgentAccount(agentName).connectWithLogin(agentName, firstName, lastName, password);
        }

        [XmlRpcExposed]
        public bool logout(string agentName)
        {
            Console.Out.WriteLine("Logging out {0}", agentName);
            bool tmp = mapAgentAccount(agentName).logout();
            agentAccountMap.Remove(agentName);
            return tmp;
        }

        [XmlRpcExposed]
        public bool moveToPosition(string agentName, int x, int y, int z)
        {
            return mapAgentAccount(agentName).moveToPosition(x, y, z);
        }

        [XmlRpcExposed]
        public bool sendChatMessage(string agentName, string message)
        {
            return mapAgentAccount(agentName).sendChatMessage(message);
        }

        [XmlRpcExposed]
        public bool sendGroupIM(string agentName, string message, string groupUUID)
        {
            return mapAgentAccount(agentName).sendGroupIM(message, groupUUID);
        }

        [XmlRpcExposed]
        public bool sendIM(string agentName, string message, string recipientFirstName, string recipientLastName)
        {
            return mapAgentAccount(agentName).sendIM(message, recipientFirstName, recipientLastName);
        }

        [XmlRpcExposed]
        public bool setMaster(string agentName, string masterFirstName, string masterLastName)
        {
            return mapAgentAccount(agentName).setMaster(masterFirstName, masterLastName);
        }

        [XmlRpcExposed]
        public bool teleportToPosition(string agentName, string island, int x, int y, int z)
        {
            return mapAgentAccount(agentName).teleportToPosition(island, x, y, z);
        }

        [XmlRpcExposed]
        public bool clone(string agentName, string firstName, string lastName)
        {
            return mapAgentAccount(agentName).clone(firstName, lastName);
        }

        [XmlRpcExposed]
        public bool isAvatarOnline(string agentName, string firstName, string lastName)
        {
            return mapAgentAccount(agentName).isAvatarOnline(firstName, lastName);
        }

        public static void Main(string[] args)
        {           
            // XML-RPC-Stuff
            // Use the console logger above.
            Nwc.XmlRpc.Logger.Delegate = new Nwc.XmlRpc.Logger.LoggerDelegate(WriteEntry);

            XmlRpcSLConnector = new XmlRpcServer(port);
            XmlRpcSLConnector.Add("XmlRpcSLConnector", new CSSLAgentServer());
            Console.WriteLine("Web Server Running on port {0} ... Press ^C to Stop...", port);
            XmlRpcSLConnector.Start();
        }

        public CSSLAgentServer()
        {
            agentAccountMap = new Dictionary<string, SLAgentCS>();
        }

        ~CSSLAgentServer()
        {
            
        }        
    }
}