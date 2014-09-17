using System;

/**
 * @(#) IXmlRpcSLConnector.cs
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

/**
 * Interface, welche alleenth�lt alle Methoden, die von libsecondlife �ber XMLRPC zur Verf�gung gestellt werden und die ein Agent aufrufen kann. Diese Klasse ist daher ein XMLRPC-Server oder ein Methoden-Anbieter.
 * Alle Methoden entsprechen 1:1 den Methoden der Klassen SLAgentCS und SLBridgePlugin. Ein Agent ruft also Methoden von SLBridgePlugin auf, die direkt an die libsecondlife durchgereicht werden.
 * Die Methodenaufrufe sind synchron, ein R�ckgabewert wird also direkt �bergeben.
 * Dieses Interface besteht in Wirklichkeit aus zwei verschiedenen Dateien, IXmlRpcSLConnector.cs und IXmlRpcSLConnector.java.
 */
namespace CSSLAgentServer
{
    public interface IXmlRpcSLConnector
    {
        bool connectWithLogin(string agentName, string firstName, string lastName, string password);

        bool logout(string agentName);

        bool setMaster(string agentName, string masterFirstName, string masterLastName);

        bool sendIM(string agentName, string message, string recipientFirstName, string recipientLastName);

        bool sendChatMessage(string agentName, string message);

        bool moveToPosition(string agentName, int x, int y, int z);

        bool sendGroupIM(string agentName, string message, string groupUUID);

        bool teleportToPosition(string agentName, string island, int x, int y, int z);
		
		bool clone(string agentName, string firstName, string lastName);
		
		bool isAvatarOnline(string agentName, string firstName, string lastName);

    }
}