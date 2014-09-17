/**
 * @(#) SLAgentCS.cs
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
 *
 *
 * This Software incorporates source code copyrighted (c) by Second Life Reverse Engineering Team, 2006-2008
 * and though must include this notice:
 * 
 * Copyright (c) 2006-2008, Second Life Reverse Engineering Team
 * All rights reserved.
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
using System.Diagnostics;
using System.Threading;
using Nwc.XmlRpc;
using libsecondlife;
using libsecondlife.Packets;
using libsecondlife.Imaging;


namespace CSSLAgentServer
{
    public class SLAgentCS : SecondLife
    {
        // XML-RPC stuff
        string XmlRpcAgencyConnectorUrl = "http://127.0.0.1:5051";
        XmlRpcRequest XmlRpcAgencyConnector = new XmlRpcRequest();
        string agentName;

        /// <summary>
        /// <c>LoggerDelegate</c> compliant method that does logging to Console.
        /// This method filters out the <c>LogLevel.Information</c> chatter.
        /// </summary>
        public void writeEntry(string msg, LogLevel level)
        {
            if (level > LogLevel.Information) // ignore debug msgs
                Console.WriteLine("{0}: {1}", level, msg);
        }

        // Avatar name to LUUID mapping async -> sync helpers
        string toAvatarName = string.Empty;
        ManualResetEvent nameSearchEvent = new ManualResetEvent(false);
        Dictionary<string, LLUUID> name2Key = new Dictionary<string, LLUUID>();

        // Avatar Appearance helpers
        Dictionary<LLUUID, AvatarAppearancePacket> appearances = new Dictionary<LLUUID, AvatarAppearancePacket>();
		uint SerialNum = 2;

        // Group IM init async -> sync helpers
        LLUUID toGroupID = LLUUID.Zero;
        ManualResetEvent waitForSessionStart = new ManualResetEvent(false);

        // Master handling        
        LLUUID masterKey = LLUUID.Zero;

		// Teleport Queue Helper Event
		ManualResetEvent waitOnEventQueueEvent = new ManualResetEvent(false);

        // Is Avatar Online Helper Event
        ManualResetEvent ReceivedAvatarPropertiesEvent = new ManualResetEvent(false);
        Avatar.AvatarProperties avatarProperties;

        void onChatMessageReceive(string message, ChatAudibleLevel audible, ChatType type, 
            ChatSourceType sourceType, string fromName, LLUUID id, LLUUID ownerid, LLVector3 position)
        {
			if ((message.Length != 0) && (message != null) && (message != String.Empty) && !(id == Self.AgentID)) {
				Self.AutoPilotCancel();
				string[] sender = fromName.Split(' ');			
	
				
	            XmlRpcAgencyConnector.MethodName = "XmlRpcAgencyConnector.onChatMessageReceive";
	            XmlRpcAgencyConnector.Params.Clear();
	            XmlRpcAgencyConnector.Params.Add(agentName);
	            XmlRpcAgencyConnector.Params.Add(message);
	            XmlRpcAgencyConnector.Params.Add((int)position.X);
	            XmlRpcAgencyConnector.Params.Add((int)position.Y);
	            XmlRpcAgencyConnector.Params.Add((int)position.Z);
	            XmlRpcAgencyConnector.Params.Add(sender[0]);
	            XmlRpcAgencyConnector.Params.Add(sender[1]);
	
	            try
	            {
	                #if DEBUG
	                Console.WriteLine("Request: " + XmlRpcAgencyConnector);
	                #endif
	                XmlRpcResponse response = XmlRpcAgencyConnector.Send(XmlRpcAgencyConnectorUrl);
	                #if DEBUG
	                Console.WriteLine("Response: " + response);
	                #endif
	
	                if (response.IsFault)
	                {
	                    #if DEBUG
	                    Console.WriteLine("Fault {0}: {1}", response.FaultCode, response.FaultString);
	                    #endif
	                }
	                else
	                {
	                    #if DEBUG
	                    Console.WriteLine("Returned: " + response.Value);
	                    #endif
	                }
	            }
	            catch (Exception e)
	            {
	                Console.WriteLine("Exception " + e);
	            }
	        }
		}


        void onIMReceive(InstantMessage im, Simulator simulator)
        {
            #if DEBUG
            Console.WriteLine("<{0} ({1})> {2}: {3} (@{4}:{5})",
                im.GroupIM ? "GroupIM" : "IM", im.Dialog, im.FromAgentName, im.Message,
                im.RegionID, im.Position);
            #endif
			string[] sender = im.FromAgentName.Split(' ');
            XmlRpcAgencyConnector.MethodName = "XmlRpcAgencyConnector.onIMReceive";
            XmlRpcAgencyConnector.Params.Clear();
            XmlRpcAgencyConnector.Params.Add(agentName);
            XmlRpcAgencyConnector.Params.Add(im.Message);
            XmlRpcAgencyConnector.Params.Add(sender[0]);
            XmlRpcAgencyConnector.Params.Add(sender[1]);

            try
            {
                #if DEBUG
                Console.WriteLine("Request: " + XmlRpcAgencyConnector);
                #endif
                XmlRpcResponse response = XmlRpcAgencyConnector.Send(XmlRpcAgencyConnectorUrl);
                #if DEBUG
                Console.WriteLine("Response: " + response);
                #endif

                if (response.IsFault)
                {
                    #if DEBUG
                    Console.WriteLine("Fault {0}: {1}", response.FaultCode, response.FaultString);
                    #endif
                }
                else
                {
                    #if DEBUG
                    Console.WriteLine("Returned: " + response.Value);
                    #endif
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e);
            }
        }

        // synchronous
        public bool teleportToPosition(string island, int x, int y, int z)
        {
			waitOnEventQueueEvent.WaitOne(6000, false);
			waitOnEventQueueEvent.Reset();
			
            island = island.Trim();
            if (Self.Teleport(island, new LLVector3((float)x, (float)y, (float)z)))
            {
                #if DEBUG
                Console.Out.WriteLine("Teleported to {0}", Network.CurrentSim);
                #endif

                return true;
            }
            else
            {
                #if DEBUG
                Console.Out.WriteLine("Teleport failed: {0}", Self.TeleportMessage);
                #endif

                return false;
            }
        }

        public bool clone(String firstName, String lastName)
        {
            string targetName = String.Empty;
            List<DirectoryManager.AgentSearchData> matches;

            targetName = firstName + " " + lastName;
            targetName = targetName.TrimEnd();

            if (targetName.Length == 0)
                return false;

            if (Directory.PeopleSearch(DirectoryManager.DirFindFlags.People, targetName, 0, 1000 * 10,
                out matches) && matches.Count > 0)
            {
                LLUUID target = matches[0].AgentID;
                targetName += String.Format(" ({0})", target);

                return clone(target);
            }
            else
            {
				#if DEBUG
                Console.Out.WriteLine("Couldn't find avatar {0}", targetName);
				#endif
                return false;
            }
        }

        bool clone(LLUUID target)
        {
            if (appearances.ContainsKey(target))
            {
                #region AvatarAppearance to AgentSetAppearance

                AvatarAppearancePacket appearance = appearances[target];

                AgentSetAppearancePacket set = new AgentSetAppearancePacket();
                set.AgentData.AgentID = Self.AgentID;
                set.AgentData.SessionID = Self.SessionID;
                set.AgentData.SerialNum = SerialNum++;
                set.AgentData.Size = new LLVector3(2f, 2f, 2f); // HACK

                set.WearableData = new AgentSetAppearancePacket.WearableDataBlock[0];
                set.VisualParam = new AgentSetAppearancePacket.VisualParamBlock[appearance.VisualParam.Length];

                for (int i = 0; i < appearance.VisualParam.Length; i++)
                {
                    set.VisualParam[i] = new AgentSetAppearancePacket.VisualParamBlock();
                    set.VisualParam[i].ParamValue = appearance.VisualParam[i].ParamValue;
                }

                set.ObjectData.TextureEntry = appearance.ObjectData.TextureEntry;

                #endregion AvatarAppearance to AgentSetAppearance

                // Detach everything we are currently wearing
                Appearance.AddAttachments(new List<InventoryBase>(), true);

                // Send the new appearance packet
                Network.SendPacket(set);

                #if DEBUG
                Console.Out.WriteLine("Cloned {0}", target.ToString());
                #endif

                return true;
            }
            else
            {
                #if DEBUG
                Console.Out.WriteLine("Don't know the appearance of avatar {0}", target.ToString());
                #endif
                return false;
            }
        
        }

        public bool isAvatarOnline(String firstName, String lastName)
        {			
            LLUUID avatarKey = LLUUID.Zero;
            lock (toAvatarName)
            {
                toAvatarName = firstName + " " + lastName;
                if (!name2Key.ContainsKey(toAvatarName.ToLower()))
                {
                    // Send the Query
                    Avatars.RequestAvatarNameSearch(toAvatarName, LLUUID.Random());

                    nameSearchEvent.WaitOne(6000, false);
                }

                if (name2Key.ContainsKey(toAvatarName.ToLower()))
                {
                    avatarKey = name2Key[toAvatarName.ToLower()];
                    Avatars.RequestAvatarProperties(avatarKey);

					
                    ReceivedAvatarPropertiesEvent.Reset();
                    ReceivedAvatarPropertiesEvent.WaitOne(5000, false);

                    return avatarProperties.Online;
                }
                else
                {
                    #if DEBUG
                    Console.Out.WriteLine("Name lookup for {0} failed", toAvatarName);
                    #endif

                    return false;
                }
            }
        }

        void onAvatarProperties(LLUUID avatarID, Avatar.AvatarProperties properties)
        {
            lock (ReceivedAvatarPropertiesEvent)
            {
                avatarProperties = properties;
                ReceivedAvatarPropertiesEvent.Set();
            }
        }
	
        // synchronous
        public bool setMaster(string masterFirstName, string masterLastName)
        {
            lock (toAvatarName)
            {
                toAvatarName = masterFirstName + " " + masterLastName;
                if (!name2Key.ContainsKey(toAvatarName.ToLower()))
                {
                    // Send the Query
                    Avatars.RequestAvatarNameSearch(toAvatarName, LLUUID.Random());

                    nameSearchEvent.WaitOne(6000, false);
                }

                if (name2Key.ContainsKey(toAvatarName.ToLower()))
                {
                    masterKey = name2Key[toAvatarName.ToLower()];

                    return clone(masterKey);
                }
                else
                {
                	#if DEBUG
                    Console.Out.WriteLine("Name lookup for {0} failed", toAvatarName);
                	#endif

                    return false;
                }
            }
        }

        // synchronous
        // Only works with non-LUUID-recipients if these recipients are logged in!!
        // if recipientLastName == "LLUUID", recipientFirstName is expected to contain a valid LLUUID
        // for instant messagin while avatar isn't online
        public bool sendIM(string message, string recipientFirstName, string recipientLastName)
        {
            LLUUID id = LLUUID.Zero;
			
			// passed LLUUID directly
            if (recipientLastName.Equals("LLUUID"))
            {
                if (!LLUUID.TryParse(recipientFirstName, out id))
                {

                    #if DEBUG
                    Console.Out.WriteLine("LLUUID parsing for {0} failed", recipientFirstName);
                    #endif

                    return false;
                }
            }
			
			// get LLUUID by friedship list lookup
            else if (Friends.FriendList.Count > 0)
            {
                Friends.FriendList.ForEach(delegate(FriendInfo friend)
                {
                    #if DEBUG
                    Console.Out.WriteLine(friend.Name);
                    #endif
					
					String tmpAvatarName = recipientFirstName + " " + recipientLastName;

                    if (friend.Name.ToLower() == tmpAvatarName.ToLower())
                    {
                        id = friend.UUID;
                    }
                });
            }
			
			// lookup LLUUID through system, only works if recipients are logged in!!
            if (id == LLUUID.Zero)
            {
                lock (toAvatarName)
                {
                    toAvatarName = recipientFirstName + " " + recipientLastName;

                    if (!name2Key.ContainsKey(toAvatarName.ToLower()))
                    {
                        // Send the Query
                        Avatars.RequestAvatarNameSearch(toAvatarName, LLUUID.Random());

                        nameSearchEvent.WaitOne(6000, false);
                    }

                    if (name2Key.ContainsKey(toAvatarName.ToLower()))
                    {
                        id = name2Key[toAvatarName.ToLower()];
                    }
                    else
                    {
                        #if DEBUG
                        Console.Out.WriteLine("Name lookup for {0} failed", toAvatarName);
                        #endif

                        return false;
                    }
                }
            }

			// Build the message
            message = message.TrimEnd();
            if (message.Length > 1023) message = message.Remove(1023);

            Self.InstantMessage(id, message);

            #if DEBUG
            Console.Out.WriteLine("Instant Messaged {0} with message: {1}", id.ToString(), message);
            #endif

            return true;
        }

        public bool sendGroupIM(string message, string groupUUID)
        {
            message = message.TrimEnd();
            if (message.Length > 1023) message = message.Remove(1023);

            Self.OnGroupChatJoin += new AgentManager.GroupChatJoined(onGroupChatJoin);
            if (!Self.GroupChatSessions.ContainsKey(toGroupID))
            {
                waitForSessionStart.Reset();
                Self.RequestJoinGroupChat(toGroupID);
            }
            else
            {
                waitForSessionStart.Set();
            }
            
            if (waitForSessionStart.WaitOne(10000, false))
            {
                Self.InstantMessageGroup(toGroupID, message);
            }
            else
            {
                #if DEBUG
                Console.Out.WriteLine("Timeout waiting for group session start");
                #endif

                Self.OnGroupChatJoin -= new AgentManager.GroupChatJoined(onGroupChatJoin);
                return false;
            }
            
            Self.OnGroupChatJoin -= new AgentManager.GroupChatJoined(onGroupChatJoin);
            #if DEBUG
            Console.Out.WriteLine("Instant Messaged group {0} with message: {1}", toGroupID.ToString(), message);
            #endif

            return true;
        }

        public bool sendChatMessage(string message)
        {  
            switch (message) {
                case "/kmb" :
                    Self.AnimationStart(Animations.KISS_MY_BUTT, true);
                    break;
                case "/yes!":
                    Self.AnimationStart(Animations.YES, true);
                    break;
                case "/no":
                    Self.AnimationStart(Animations.NO, true);
                    break;
                case "/pointme":
                    Self.AnimationStart(Animations.POINT_ME, true);
                    break;
                case "/pointyou":
                    Self.AnimationStart(Animations.POINT_YOU, true);
                    break;
                case "/bow":
                    Self.AnimationStart(Animations.BOW, true);
                    break;
                case "/stretch":
                    Self.AnimationStart(Animations.STRETCH, true);
                    break;
                case "/muscle":
                    Self.AnimationStart(Animations.MUSCLE_BEACH, true);
                    break;
                default:
                    Self.Chat(message, 0, ChatType.Normal);
                    break;
            }
            
            #if DEBUG
            Console.Out.WriteLine("Said {0}", message);
            #endif

            return true;
        }

		public bool moveToPosition(int x, int y, int z)
        {
			/*
            uint regionX, regionY;
            Helpers.LongToUInts(Network.CurrentSim.Handle, out regionX, out regionY);

            // Convert the local coordinates to global ones by adding the region handle parts to x and y
            x += (double)regionX;
            y += (double)regionY;
			
			LLVector3 pos = Self.SimPosition;
			

            Self.AutoPilotLocal((int)(pos.X + x), (int)(pos.Y + y), (float)z);
            */
			
            Self.AutoPilotLocal(x, y, z);

            #if DEBUG
            Console.Out.WriteLine("Attempting to move to <{0},{1},{2}>", x, y, z);
            #endif

            return true;
        }

        // still asynchronous, but no use in waiting for logout message...
        public bool logout()
        {
            Network.Logout();
            return true;
        }

        // synchronous
        public bool connectWithLogin(string agentName, string firstName, string lastName, string password)
        {
            #if DEBUG
            Console.WriteLine("{0}, {1}, {2}", firstName, lastName, password);
            LoginParams loginParams = Network.DefaultLoginParams(firstName, lastName,
                password, "CSSLAgentServer", "1.0.0");
            #endif

            this.agentName = agentName;
            return Network.Login(loginParams);
        }

        void onAvatarNameSearch(LLUUID queryID, Dictionary<LLUUID, string> avatars)
        {
            foreach (KeyValuePair<LLUUID, string> kvp in avatars)
            {
                if (kvp.Value.ToLower() == toAvatarName.ToLower())
                {
                    name2Key[toAvatarName.ToLower()] = kvp.Key;
                    nameSearchEvent.Set();
                    return;
                }
            }
        }

        void onGroupChatJoin(LLUUID groupChatSessionID, LLUUID tmpSessionID, bool success)
        {
            if (success)
            {
                #if DEBUG
                Console.WriteLine("Join Group Chat Success!");
                #endif
                waitForSessionStart.Set();
            }
            else
            {
                #if DEBUG
                Console.WriteLine("Join Group Chat failed :(");
                #endif
            }
        }

        void avatarAppearanceHandler(Packet packet, Simulator simulator)
        {
            AvatarAppearancePacket appearance = (AvatarAppearancePacket)packet;

            lock (appearances) appearances[appearance.Sender.ID] = appearance;
        }

        void alertMessageHandler(Packet packet, Simulator simulator)
        {
            AlertMessagePacket message = (AlertMessagePacket)packet;

            libsecondlife.Logger.Log("[AlertMessage] " + Helpers.FieldToUTF8String(message.AlertData.Message), Helpers.LogLevel.Info, this);
        }

        void onEventQueueRunning(Simulator simulator)
        {
            if (simulator == Network.CurrentSim)
            {
                Console.WriteLine("Event queue connected for the primary simulator, ready for teleport");
                waitOnEventQueueEvent.Set();
            }
        }

        void onLogout(NetworkManager.DisconnectType type, string message)
        {
            XmlRpcAgencyConnector.MethodName = "XmlRpcAgencyConnector.onLogout";
            XmlRpcAgencyConnector.Params.Clear();
            XmlRpcAgencyConnector.Params.Add(agentName);
            try
            {
                #if DEBUG
                Console.WriteLine("Request: " + XmlRpcAgencyConnector);
                #endif
                XmlRpcResponse response = XmlRpcAgencyConnector.Send(XmlRpcAgencyConnectorUrl);
                #if DEBUG
                Console.WriteLine("Response: " + response);
                #endif

                if (response.IsFault)
                {
                #if DEBUG
                    Console.WriteLine("Fault {0}: {1}", response.FaultCode, response.FaultString);
                #endif
                }
                else
                {
                #if DEBUG
                    Console.WriteLine("Returned: " + response.Value);
                #endif
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e);
            }
        }      

        public SLAgentCS()
        {   
            //Settings.MULTIPLE_SIMS = false;

            // Throttle unnecessary things down
            Throttle.Wind = 0;
            Throttle.Cloud = 0;
            Throttle.Land = 1000000;
            Throttle.Task = 1000000;


            Settings.LOG_RESENDS = false;
            Settings.STORE_LAND_PATCHES = true;
            Settings.ALWAYS_DECODE_OBJECTS = true;
            Settings.ALWAYS_REQUEST_OBJECTS = true;
            Settings.SEND_AGENT_UPDATES = true;
            Settings.USE_TEXTURE_CACHE = true;
            
            Self.OnInstantMessage += new AgentManager.InstantMessageCallback(onIMReceive);
            Self.OnChat += new AgentManager.ChatCallback(onChatMessageReceive);
            Avatars.OnAvatarNameSearch += new AvatarManager.AvatarNameSearchCallback(onAvatarNameSearch);
			Avatars.OnAvatarProperties += new AvatarManager.AvatarPropertiesCallback(onAvatarProperties);

            Network.RegisterCallback(PacketType.AvatarAppearance, new NetworkManager.PacketCallback(avatarAppearanceHandler));
            Network.RegisterCallback(PacketType.AlertMessage, new NetworkManager.PacketCallback(alertMessageHandler));

            Network.OnDisconnected += new NetworkManager.DisconnectedCallback(onLogout);
            Network.OnEventQueueRunning += new NetworkManager.EventQueueRunningCallback(onEventQueueRunning);
            //Groups.OnCurrentGroups += new GroupManager.CurrentGroupsCallback(OnCurrentGroups);
            //Network.OnLogin += new NetworkManager.LoginCallback(OnLogin);

        }

        ~SLAgentCS()
        {
            Self.OnInstantMessage -= new AgentManager.InstantMessageCallback(onIMReceive);
            Self.OnChat -= new AgentManager.ChatCallback(onChatMessageReceive);
            Avatars.OnAvatarNameSearch -= new AvatarManager.AvatarNameSearchCallback(onAvatarNameSearch);
            //Network.Logout();
        }
    }
}