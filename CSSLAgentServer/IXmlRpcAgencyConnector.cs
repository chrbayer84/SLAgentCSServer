using System;
/**
 * @(#) IXmlRpcAgencyConnector.cs
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
 * Enth�lt Funktionen, die das SLBrigdePlugin per XMLRPC exportiert und f�r den C#-Part zur Verf�gung stellt. Diese Klasse ist daher ein XMLRPC-Server oder ein Methoden-Anbieter.
 * Die Methoden bilden Events nach, welche nicht objektorientiert �ber XMLRPC exportiert werden k�nnen. So ruft SLAgentCS beispielsweise die Methode SLBridgePlugin.onIMRecieve() auf, wenn eine Instant-Message f�r den assoziierten Avatar ankommt. Logischerweise kommt diese Nachricht von SecondLife und muss daher an den Java-Part �bergeben werden.
 * Dieses Interface besteht in Wirklichkeit aus zwei verschiedenen Dateien, IXmlRpcAgencyConnector.cs und IXmlRpcAgencyConnector.java.
 */
namespace CSSLAgentServer
{
    public interface IXmlRpcAgencyConnector
    {
        bool onIMReceive(string agentName, string message, string senderFirstName, string senderLastName);

        bool onChatMessageReceive(string agentName, string message, int x, int y, int z, string senderFirstName, string senderLastName);

        bool onLogout(string agentName);

    }
}