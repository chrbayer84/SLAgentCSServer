/*
 * Copyright (c) 2007-2008, Second Life Reverse Engineering Team
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

/* 
 * 
 * This tests are based upon the description at
 * 
 * http://wiki.secondlife.com/wiki/LLSD
 * 
 * and (partially) generated by the (supposed) reference implementation at
 * 
 * http://svn.secondlife.com/svn/linden/release/indra/lib/python/indra/base/llsd.py
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Framework;
using libsecondlife.StructuredData;

namespace libsecondlife.Tests
{

    [TestFixture()]
    public class NotationLLSDTests
    {
        [Test()]
        public void HelperFunctions()
        {
            StringReader reader = new StringReader("test1tast2test3");

            char[] charsOne = { 't', 'e', 's', 't' };
            int resultOne = LLSDParser.BufferCharactersEqual(reader, charsOne, 0);
            Assert.AreEqual(charsOne.Length, resultOne);

            char[] charsTwo = { '1', 't', 'e' };
            int resultTwo = LLSDParser.BufferCharactersEqual(reader, charsTwo, 0);
            Assert.AreEqual(2, resultTwo);

            char[] charsThree = { 'a', 's', 't', '2', 't', 'e', 's' };
            int resultThree = LLSDParser.BufferCharactersEqual(reader, charsThree, 1);
            Assert.AreEqual(1, resultThree);

            int resultFour = LLSDParser.BufferCharactersEqual(reader, charsThree, 0);
            Assert.AreEqual(charsThree.Length, resultFour);

            char[] charsFive = { 't', '3', 'a', 'a' };
            int resultFive = LLSDParser.BufferCharactersEqual(reader, charsFive, 0);
            Assert.AreEqual(2, resultFive);


        }

        [Test()]
        public void DeserializeUndef()
        {
            String s = "!";
            LLSD llsd = LLSDParser.DeserializeNotation(s);
            Assert.AreEqual(LLSDType.Unknown, llsd.Type);
        }

        [Test()]
        public void SerializeUndef()
        {
            LLSD llsd = new LLSD();
            string s = LLSDParser.SerializeNotation(llsd);

            LLSD llsdDS = LLSDParser.DeserializeNotation(s);
            Assert.AreEqual(LLSDType.Unknown, llsdDS.Type);
        }

        [Test()]
        public void DeserializeBoolean()
        {
            String t = "true";
            LLSD llsdT = LLSDParser.DeserializeNotation(t);
            Assert.AreEqual(LLSDType.Boolean, llsdT.Type);
            Assert.AreEqual(true, llsdT.AsBoolean());

            String tTwo = "t";
            LLSD llsdTTwo = LLSDParser.DeserializeNotation(tTwo);
            Assert.AreEqual(LLSDType.Boolean, llsdTTwo.Type);
            Assert.AreEqual(true, llsdTTwo.AsBoolean());

            String tThree = "TRUE";
            LLSD llsdTThree = LLSDParser.DeserializeNotation(tThree);
            Assert.AreEqual(LLSDType.Boolean, llsdTThree.Type);
            Assert.AreEqual(true, llsdTThree.AsBoolean());

            String tFour = "T";
            LLSD llsdTFour = LLSDParser.DeserializeNotation(tFour);
            Assert.AreEqual(LLSDType.Boolean, llsdTFour.Type);
            Assert.AreEqual(true, llsdTFour.AsBoolean());

            String tFive = "1";
            LLSD llsdTFive = LLSDParser.DeserializeNotation(tFive);
            Assert.AreEqual(LLSDType.Boolean, llsdTFive.Type);
            Assert.AreEqual(true, llsdTFive.AsBoolean());

            String f = "false";
            LLSD llsdF = LLSDParser.DeserializeNotation(f);
            Assert.AreEqual(LLSDType.Boolean, llsdF.Type);
            Assert.AreEqual(false, llsdF.AsBoolean());

            String fTwo = "f";
            LLSD llsdFTwo = LLSDParser.DeserializeNotation(fTwo);
            Assert.AreEqual(LLSDType.Boolean, llsdFTwo.Type);
            Assert.AreEqual(false, llsdFTwo.AsBoolean());

            String fThree = "FALSE";
            LLSD llsdFThree = LLSDParser.DeserializeNotation(fThree);
            Assert.AreEqual(LLSDType.Boolean, llsdFThree.Type);
            Assert.AreEqual(false, llsdFThree.AsBoolean());

            String fFour = "F";
            LLSD llsdFFour = LLSDParser.DeserializeNotation(fFour);
            Assert.AreEqual(LLSDType.Boolean, llsdFFour.Type);
            Assert.AreEqual(false, llsdFFour.AsBoolean());

            String fFive = "0";
            LLSD llsdFFive = LLSDParser.DeserializeNotation(fFive);
            Assert.AreEqual(LLSDType.Boolean, llsdFFive.Type);
            Assert.AreEqual(false, llsdFFive.AsBoolean());
        }

        [Test()]
        public void SerializeBoolean()
        {
            LLSD llsdTrue = LLSD.FromBoolean(true);
            string sTrue = LLSDParser.SerializeNotation(llsdTrue);
            LLSD llsdTrueDS = LLSDParser.DeserializeNotation(sTrue);
            Assert.AreEqual(LLSDType.Boolean, llsdTrueDS.Type);
            Assert.AreEqual(true, llsdTrueDS.AsBoolean());

            LLSD llsdFalse = LLSD.FromBoolean(false);
            string sFalse = LLSDParser.SerializeNotation(llsdFalse);
            LLSD llsdFalseDS = LLSDParser.DeserializeNotation(sFalse);
            Assert.AreEqual(LLSDType.Boolean, llsdFalseDS.Type);
            Assert.AreEqual(false, llsdFalseDS.AsBoolean());
        }

        [Test()]
        public void DeserializeInteger()
        {
            string integerOne = "i12319423";
            LLSD llsdOne = LLSDParser.DeserializeNotation(integerOne);
            Assert.AreEqual(LLSDType.Integer, llsdOne.Type);
            Assert.AreEqual(12319423, llsdOne.AsInteger());

            string integerTwo = "i-489234";
            LLSD llsdTwo = LLSDParser.DeserializeNotation(integerTwo);
            Assert.AreEqual(LLSDType.Integer, llsdTwo.Type);
            Assert.AreEqual(-489234, llsdTwo.AsInteger());
        }

        [Test()]
        public void SerializeInteger()
        {
            LLSD llsdOne = LLSD.FromInteger(12319423);
            string sOne = LLSDParser.SerializeNotation(llsdOne);
            LLSD llsdOneDS = LLSDParser.DeserializeNotation(sOne);
            Assert.AreEqual(LLSDType.Integer, llsdOneDS.Type);
            Assert.AreEqual(12319423, llsdOne.AsInteger());

            LLSD llsdTwo = LLSD.FromInteger(-71892034);
            string sTwo = LLSDParser.SerializeNotation(llsdTwo);
            LLSD llsdTwoDS = LLSDParser.DeserializeNotation(sTwo);
            Assert.AreEqual(LLSDType.Integer, llsdTwoDS.Type);
            Assert.AreEqual(-71892034, llsdTwoDS.AsInteger());
        }

        [Test()]
        public void DeserializeReal()
        {
            String realOne = "r1123412345.465711";
            LLSD llsdOne = LLSDParser.DeserializeNotation(realOne);
            Assert.AreEqual(LLSDType.Real, llsdOne.Type);
            Assert.AreEqual(1123412345.465711d, llsdOne.AsReal());

            String realTwo = "r-11234684.923411";
            LLSD llsdTwo = LLSDParser.DeserializeNotation(realTwo);
            Assert.AreEqual(LLSDType.Real, llsdTwo.Type);
            Assert.AreEqual(-11234684.923411d, llsdTwo.AsReal());

            String realThree = "r1";
            LLSD llsdThree = LLSDParser.DeserializeNotation(realThree);
            Assert.AreEqual(LLSDType.Real, llsdThree.Type);
            Assert.AreEqual(1d, llsdThree.AsReal());

            String realFour = "r2.0193899999999998204e-06";
            LLSD llsdFour = LLSDParser.DeserializeNotation(realFour);
            Assert.AreEqual(LLSDType.Real, llsdFour.Type);
            Assert.AreEqual(2.0193899999999998204e-06d, llsdFour.AsReal());

            String realFive = "r0";
            LLSD llsdFive = LLSDParser.DeserializeNotation(realFive);
            Assert.AreEqual(LLSDType.Real, llsdFive.Type);
            Assert.AreEqual(0d, llsdFive.AsReal());
        }

        [Test()]
        public void SerializeReal()
        {
            LLSD llsdOne = LLSD.FromReal(12987234.723847d);
            string sOne = LLSDParser.SerializeNotation(llsdOne);
            LLSD llsdOneDS = LLSDParser.DeserializeNotation(sOne);
            Assert.AreEqual(LLSDType.Real, llsdOneDS.Type);
            Assert.AreEqual(12987234.723847d, llsdOneDS.AsReal());

            LLSD llsdTwo = LLSD.FromReal(-32347892.234234d);
            string sTwo = LLSDParser.SerializeNotation(llsdTwo);
            LLSD llsdTwoDS = LLSDParser.DeserializeNotation(sTwo);
            Assert.AreEqual(LLSDType.Real, llsdTwoDS.Type);
            Assert.AreEqual(-32347892.234234d, llsdTwoDS.AsReal());

            /* The following two tests don't pass on mono 1.9, as
             * mono isnt able to parse its own Double.Max/MinValue.
             */
            //            LLSD llsdThree = LLSD.FromReal( Double.MaxValue );
            //            string sThree = LLSDParser.SerializeNotation( llsdThree );
            //            LLSD llsdThreeDS = LLSDParser.DeserializeNotation( sThree );
            //            Assert.AreEqual( LLSDType.Real, llsdThreeDS.Type );
            //            Assert.AreEqual( Double.MaxValue, llsdThreeDS.AsReal());
            //            
            //            LLSD llsdFour = LLSD.FromReal( Double.MinValue );
            //            string sFour = LLSDParser.SerializeNotation( llsdFour );
            //            LLSD llsdFourDS = LLSDParser.DeserializeNotation( sFour );
            //            Assert.AreEqual( LLSDType.Real, llsdFourDS.Type );
            //            Assert.AreEqual( Double.MinValue, llsdFourDS.AsReal());

            LLSD llsdFive = LLSD.FromReal(-1.1123123E+50d);
            string sFive = LLSDParser.SerializeNotation(llsdFive);
            LLSD llsdFiveDS = LLSDParser.DeserializeNotation(sFive);
            Assert.AreEqual(LLSDType.Real, llsdFiveDS.Type);
            Assert.AreEqual(-1.1123123E+50d, llsdFiveDS.AsReal());

            LLSD llsdSix = LLSD.FromReal(2.0193899999999998204e-06);
            string sSix = LLSDParser.SerializeNotation(llsdSix);
            LLSD llsdSixDS = LLSDParser.DeserializeNotation(sSix);
            Assert.AreEqual(LLSDType.Real, llsdSixDS.Type);
            Assert.AreEqual(2.0193899999999998204e-06, llsdSixDS.AsReal());


        }

        [Test()]
        public void DeserializeUUID()
        {
            String uuidOne = "u97f4aeca-88a1-42a1-b385-b97b18abb255";
            LLSD llsdOne = LLSDParser.DeserializeNotation(uuidOne);
            Assert.AreEqual(LLSDType.UUID, llsdOne.Type);
            Assert.AreEqual("97f4aeca-88a1-42a1-b385-b97b18abb255", llsdOne.AsString());

            String uuidTwo = "u00000000-0000-0000-0000-000000000000";
            LLSD llsdTwo = LLSDParser.DeserializeNotation(uuidTwo);
            Assert.AreEqual(LLSDType.UUID, llsdTwo.Type);
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", llsdTwo.AsString());
        }

        [Test()]
        public void SerializeUUID()
        {
            LLSD llsdOne = LLSD.FromUUID(new LLUUID("97f4aeca-88a1-42a1-b385-b97b18abb255"));
            string sOne = LLSDParser.SerializeNotation(llsdOne);
            LLSD llsdOneDS = LLSDParser.DeserializeNotation(sOne);
            Assert.AreEqual(LLSDType.UUID, llsdOneDS.Type);
            Assert.AreEqual("97f4aeca-88a1-42a1-b385-b97b18abb255", llsdOneDS.AsString());

            LLSD llsdTwo = LLSD.FromUUID(new LLUUID("00000000-0000-0000-0000-000000000000"));
            string sTwo = LLSDParser.SerializeNotation(llsdTwo);
            LLSD llsdTwoDS = LLSDParser.DeserializeNotation(sTwo);
            Assert.AreEqual(LLSDType.UUID, llsdTwoDS.Type);
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", llsdTwoDS.AsString());
        }

        public void DeserializeString()
        {
            string sOne = "''";
            LLSD llsdOne = LLSDParser.DeserializeNotation(sOne);
            Assert.AreEqual(LLSDType.String, llsdOne.Type);
            Assert.AreEqual("", llsdOne.AsString());

            // This is double escaping. Once for the encoding, and once for csharp.  
            string sTwo = "'test\\'\"test'";
            LLSD llsdTwo = LLSDParser.DeserializeNotation(sTwo);
            Assert.AreEqual(LLSDType.String, llsdTwo.Type);
            Assert.AreEqual("test'\"test", llsdTwo.AsString());

            // "test \\lest"
            char[] cThree = { (char)0x27, (char)0x74, (char)0x65, (char)0x73, (char)0x74, (char)0x20, (char)0x5c,
                                (char)0x5c, (char)0x6c, (char)0x65, (char)0x73, (char)0x74, (char)0x27 };
            string sThree = new string(cThree);

            LLSD llsdThree = LLSDParser.DeserializeNotation(sThree);
            Assert.AreEqual(LLSDType.String, llsdThree.Type);
            Assert.AreEqual("test \\lest", llsdThree.AsString());

            string sFour = "'aa\t la'";
            LLSD llsdFour = LLSDParser.DeserializeNotation(sFour);
            Assert.AreEqual(LLSDType.String, llsdFour.Type);
            Assert.AreEqual("aa\t la", llsdFour.AsString());

            char[] cFive = { (char)0x27, (char)0x5c, (char)0x5c, (char)0x27 };
            string sFive = new String(cFive);
            LLSD llsdFive = LLSDParser.DeserializeNotation(sFive);
            Assert.AreEqual(LLSDType.String, llsdFive.Type);
            Assert.AreEqual("\\", llsdFive.AsString());


            string sSix = "s(10)\"1234567890\"";
            LLSD llsdSix = LLSDParser.DeserializeNotation(sSix);
            Assert.AreEqual(LLSDType.String, llsdSix.Type);
            Assert.AreEqual("1234567890", llsdSix.AsString());

            string sSeven = "s(5)\"\\\\\\\\\\\"";
            LLSD llsdSeven = LLSDParser.DeserializeNotation(sSeven);
            Assert.AreEqual(LLSDType.String, llsdSeven.Type);
            Assert.AreEqual("\\\\\\\\\\", llsdSeven.AsString());

            string sEight = "\"aouAOUhsdjklfghskldjfghqeiurtzwieortzaslxfjkgh\"";
            LLSD llsdEight = LLSDParser.DeserializeNotation(sEight);
            Assert.AreEqual(LLSDType.String, llsdEight.Type);
            Assert.AreEqual("aouAOUhsdjklfghskldjfghqeiurtzwieortzaslxfjkgh", llsdEight.AsString());



        }

        public void DoSomeStringSerializingActionsAndAsserts(string s)
        {
            LLSD llsdOne = LLSD.FromString(s);
            string sOne = LLSDParser.SerializeNotation(llsdOne);
            LLSD llsdOneDS = LLSDParser.DeserializeNotation(sOne);
            Assert.AreEqual(LLSDType.String, llsdOne.Type);
            Assert.AreEqual(s, llsdOneDS.AsString());
        }


        [Test()]
        public void SerializeString()
        {
            DoSomeStringSerializingActionsAndAsserts("");

            DoSomeStringSerializingActionsAndAsserts("\\");

            DoSomeStringSerializingActionsAndAsserts("\"\"");

            DoSomeStringSerializingActionsAndAsserts("������-these-should-be-some-german-umlauts");

            DoSomeStringSerializingActionsAndAsserts("\t\n\r");

            DoSomeStringSerializingActionsAndAsserts("asdkjfhaksldjfhalskdjfhaklsjdfhaklsjdhjgzqeuiowrtzserghsldfg" +
                                                      "asdlkfhqeiortzsdkfjghslkdrjtzsoidklghuisoehiguhsierughaishdl" +
                                                      "asdfkjhueiorthsgsdkfughaslkdfjshldkfjghsldkjghsldkfghsdklghs" +
                                                      "wopeighisdjfghklasdfjghsdklfgjhsdklfgjshdlfkgjshdlfkgjshdlfk");

            DoSomeStringSerializingActionsAndAsserts("all is N\"\\'othing and n'oting is all");

            DoSomeStringSerializingActionsAndAsserts("very\"british is this.");

            // We test here also for 4byte characters
            string xml = "<x>&#x10137;</x>";
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            XmlTextReader xtr = new XmlTextReader(new MemoryStream(bytes, false));
            xtr.Read();
            xtr.Read();
            string content = xtr.ReadString();

            DoSomeStringSerializingActionsAndAsserts(content);

        }

        [Test()]
        public void DeserializeURI()
        {
            string sUriOne = "l\"http://test.com/test test>\\\"/&yes\"";
            LLSD llsdOne = LLSDParser.DeserializeNotation(sUriOne);
            Assert.AreEqual(LLSDType.URI, llsdOne.Type);
            Assert.AreEqual("http://test.com/test test>\"/&yes", llsdOne.AsString());

            string sUriTwo = "l\"test/test/test?test=1&toast=2\"";
            LLSD llsdTwo = LLSDParser.DeserializeNotation(sUriTwo);
            Assert.AreEqual(LLSDType.URI, llsdTwo.Type);
            Assert.AreEqual("test/test/test?test=1&toast=2", llsdTwo.AsString());
        }

        [Test()]
        public void SerializeURI()
        {
            Uri uriOne = new Uri("http://test.org/test test>\\\"/&yes\"", UriKind.RelativeOrAbsolute);
            LLSD llsdOne = LLSD.FromUri(uriOne);
            string sUriOne = LLSDParser.SerializeNotation(llsdOne);
            LLSD llsdOneDS = LLSDParser.DeserializeNotation(sUriOne);
            Assert.AreEqual(LLSDType.URI, llsdOneDS.Type);
            Assert.AreEqual(uriOne, llsdOneDS.AsUri());

            Uri uriTwo = new Uri("test/test/near/the/end?test=1", UriKind.RelativeOrAbsolute);
            LLSD llsdTwo = LLSD.FromUri(uriTwo);
            string sUriTwo = LLSDParser.SerializeNotation(llsdTwo);
            LLSD llsdTwoDS = LLSDParser.DeserializeNotation(sUriTwo);
            Assert.AreEqual(LLSDType.URI, llsdTwoDS.Type);
            Assert.AreEqual(uriTwo, llsdTwoDS.AsUri());
        }

        [Test()]
        public void DeserializeDate()
        {
            string sDateOne = "d\"2007-12-31T20:49:10Z\"";
            LLSD llsdOne = LLSDParser.DeserializeNotation(sDateOne);
            Assert.AreEqual(LLSDType.Date, llsdOne.Type);
            DateTime dt = new DateTime(2007, 12, 31, 20, 49, 10, 0, DateTimeKind.Utc);
            DateTime dtDS = llsdOne.AsDate();
            Assert.AreEqual(dt, dtDS.ToUniversalTime());
        }

        [Test()]
        public void SerializeDate()
        {
            DateTime dtOne = new DateTime(2005, 8, 10, 11, 23, 4, DateTimeKind.Utc);
            LLSD llsdOne = LLSD.FromDate(dtOne);
            string sDtOne = LLSDParser.SerializeNotation(llsdOne);
            LLSD llsdOneDS = LLSDParser.DeserializeNotation(sDtOne);
            Assert.AreEqual(LLSDType.Date, llsdOneDS.Type);
            DateTime dtOneDS = llsdOneDS.AsDate();
            Assert.AreEqual(dtOne, dtOneDS.ToUniversalTime());

            DateTime dtTwo = new DateTime(2010, 10, 11, 23, 00, 10, 100, DateTimeKind.Utc);
            LLSD llsdTwo = LLSD.FromDate(dtTwo);
            string sDtTwo = LLSDParser.SerializeNotation(llsdTwo);
            LLSD llsdTwoDS = LLSDParser.DeserializeNotation(sDtTwo);
            Assert.AreEqual(LLSDType.Date, llsdTwoDS.Type);
            DateTime dtTwoDS = llsdTwoDS.AsDate();
            Assert.AreEqual(dtTwo, dtTwoDS.ToUniversalTime());

            // check if a *local* time can be serialized and deserialized
            DateTime dtThree = new DateTime(2009, 12, 30, 8, 25, 10, DateTimeKind.Local);
            LLSD llsdDateThree = LLSD.FromDate(dtThree);
            string sDateThreeSerialized = LLSDParser.SerializeNotation(llsdDateThree);
            LLSD llsdDateThreeDS = LLSDParser.DeserializeNotation(sDateThreeSerialized);
            Assert.AreEqual(LLSDType.Date, llsdDateThreeDS.Type);
            Assert.AreEqual(dtThree, llsdDateThreeDS.AsDate());
        }

        [Test()]
        public void SerializeBinary()
        {
            byte[] binary = { 0x0, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0b,
                                0x0b, 0x0c, 0x0d, 0x0e, 0x0f };

            LLSD llsdBinary = LLSD.FromBinary(binary);
            string sBinarySerialized = LLSDParser.SerializeNotation(llsdBinary);
            LLSD llsdBinaryDS = LLSDParser.DeserializeNotation(sBinarySerialized);
            Assert.AreEqual(LLSDType.Binary, llsdBinaryDS.Type);
            Assert.AreEqual(binary, llsdBinaryDS.AsBinary());
        }

        [Test()]
        public void DeserializeArray()
        {
            string sArrayOne = "[]";
            LLSDArray llsdArrayOne = (LLSDArray)LLSDParser.DeserializeNotation(sArrayOne);
            Assert.AreEqual(LLSDType.Array, llsdArrayOne.Type);
            Assert.AreEqual(0, llsdArrayOne.Count);

            string sArrayTwo = "[ i0 ]";
            LLSDArray llsdArrayTwo = (LLSDArray)LLSDParser.DeserializeNotation(sArrayTwo);
            Assert.AreEqual(LLSDType.Array, llsdArrayTwo.Type);
            Assert.AreEqual(1, llsdArrayTwo.Count);
            LLSDInteger llsdIntOne = (LLSDInteger)llsdArrayTwo[0];
            Assert.AreEqual(LLSDType.Integer, llsdIntOne.Type);
            Assert.AreEqual(0, llsdIntOne.AsInteger());

            string sArrayThree = "[ i0, i1 ]";
            LLSDArray llsdArrayThree = (LLSDArray)LLSDParser.DeserializeNotation(sArrayThree);
            Assert.AreEqual(LLSDType.Array, llsdArrayThree.Type);
            Assert.AreEqual(2, llsdArrayThree.Count);
            LLSDInteger llsdIntTwo = (LLSDInteger)llsdArrayThree[0];
            Assert.AreEqual(LLSDType.Integer, llsdIntTwo.Type);
            Assert.AreEqual(0, llsdIntTwo.AsInteger());
            LLSDInteger llsdIntThree = (LLSDInteger)llsdArrayThree[1];
            Assert.AreEqual(LLSDType.Integer, llsdIntThree.Type);
            Assert.AreEqual(1, llsdIntThree.AsInteger());

            string sArrayFour = " [ \"testtest\", \"aha\",t,f,i1, r1.2, [ i1] ] ";
            LLSDArray llsdArrayFour = (LLSDArray)LLSDParser.DeserializeNotation(sArrayFour);
            Assert.AreEqual(LLSDType.Array, llsdArrayFour.Type);
            Assert.AreEqual(7, llsdArrayFour.Count);
            Assert.AreEqual("testtest", llsdArrayFour[0].AsString());
            Assert.AreEqual("aha", llsdArrayFour[1].AsString());
            Assert.AreEqual(true, llsdArrayFour[2].AsBoolean());
            Assert.AreEqual(false, llsdArrayFour[3].AsBoolean());
            Assert.AreEqual(1, llsdArrayFour[4].AsInteger());
            Assert.AreEqual(1.2d, llsdArrayFour[5].AsReal());
            Assert.AreEqual(LLSDType.Array, llsdArrayFour[6].Type);
            LLSDArray llsdArrayFive = (LLSDArray)llsdArrayFour[6];
            Assert.AreEqual(1, llsdArrayFive[0].AsInteger());

        }

        [Test()]
        public void SerializeArray()
        {
            LLSDArray llsdOne = new LLSDArray();
            string sOne = LLSDParser.SerializeNotation(llsdOne);
            LLSDArray llsdOneDS = (LLSDArray)LLSDParser.DeserializeNotation(sOne);
            Assert.AreEqual(LLSDType.Array, llsdOneDS.Type);
            Assert.AreEqual(0, llsdOneDS.Count);

            LLSD llsdTwo = LLSD.FromInteger(123234);
            LLSD llsdThree = LLSD.FromString("asedkfjhaqweiurohzasdf");
            LLSDArray llsdFour = new LLSDArray();
            llsdFour.Add(llsdTwo);
            llsdFour.Add(llsdThree);

            llsdOne.Add(llsdTwo);
            llsdOne.Add(llsdThree);
            llsdOne.Add(llsdFour);

            string sFive = LLSDParser.SerializeNotation(llsdOne);
            LLSDArray llsdFive = (LLSDArray)LLSDParser.DeserializeNotation(sFive);
            Assert.AreEqual(LLSDType.Array, llsdFive.Type);
            Assert.AreEqual(3, llsdFive.Count);
            Assert.AreEqual(LLSDType.Integer, llsdFive[0].Type);
            Assert.AreEqual(123234, llsdFive[0].AsInteger());
            Assert.AreEqual(LLSDType.String, llsdFive[1].Type);
            Assert.AreEqual("asedkfjhaqweiurohzasdf", llsdFive[1].AsString());

            LLSDArray llsdSix = (LLSDArray)llsdFive[2];
            Assert.AreEqual(LLSDType.Array, llsdSix.Type);
            Assert.AreEqual(2, llsdSix.Count);
            Assert.AreEqual(LLSDType.Integer, llsdSix[0].Type);
            Assert.AreEqual(123234, llsdSix[0].AsInteger());
            Assert.AreEqual(LLSDType.String, llsdSix[1].Type);
            Assert.AreEqual("asedkfjhaqweiurohzasdf", llsdSix[1].AsString());
        }

        [Test()]
        public void DeserializeMap()
        {
            string sMapOne = " { } ";
            LLSDMap llsdMapOne = (LLSDMap)LLSDParser.DeserializeNotation(sMapOne);
            Assert.AreEqual(LLSDType.Map, llsdMapOne.Type);
            Assert.AreEqual(0, llsdMapOne.Count);

            string sMapTwo = " { \"test\":i2 } ";
            LLSDMap llsdMapTwo = (LLSDMap)LLSDParser.DeserializeNotation(sMapTwo);
            Assert.AreEqual(LLSDType.Map, llsdMapTwo.Type);
            Assert.AreEqual(1, llsdMapTwo.Count);
            Assert.AreEqual(LLSDType.Integer, llsdMapTwo["test"].Type);
            Assert.AreEqual(2, llsdMapTwo["test"].AsInteger());

            string sMapThree = " { 'test':\"testtesttest\", 'aha':\"muahahaha\" , \"anywhere\":! } ";
            LLSDMap llsdMapThree = (LLSDMap)LLSDParser.DeserializeNotation(sMapThree);
            Assert.AreEqual(LLSDType.Map, llsdMapThree.Type);
            Assert.AreEqual(3, llsdMapThree.Count);
            Assert.AreEqual(LLSDType.String, llsdMapThree["test"].Type);
            Assert.AreEqual("testtesttest", llsdMapThree["test"].AsString());
            Assert.AreEqual(LLSDType.String, llsdMapThree["test"].Type);
            Assert.AreEqual("muahahaha", llsdMapThree["aha"].AsString());
            Assert.AreEqual(LLSDType.Unknown, llsdMapThree["self"].Type);

            string sMapFour = " { 'test' : { 'test' : i1, 't0st' : r2.5 }, 'tist' : \"hello world!\", 'tast' : \"last\" } ";
            LLSDMap llsdMapFour = (LLSDMap)LLSDParser.DeserializeNotation(sMapFour);
            Assert.AreEqual(LLSDType.Map, llsdMapFour.Type);
            Assert.AreEqual(3, llsdMapFour.Count);
            Assert.AreEqual("hello world!", llsdMapFour["tist"].AsString());
            Assert.AreEqual("last", llsdMapFour["tast"].AsString());
            LLSDMap llsdMapFive = (LLSDMap)llsdMapFour["test"];
            Assert.AreEqual(LLSDType.Map, llsdMapFive.Type);
            Assert.AreEqual(2, llsdMapFive.Count);
            Assert.AreEqual(LLSDType.Integer, llsdMapFive["test"].Type);
            Assert.AreEqual(1, llsdMapFive["test"].AsInteger());
            Assert.AreEqual(LLSDType.Real, llsdMapFive["t0st"].Type);
            Assert.AreEqual(2.5d, llsdMapFive["t0st"].AsReal());

        }

        [Test()]
        public void SerializeMap()
        {
            LLSDMap llsdOne = new LLSDMap();
            string sOne = LLSDParser.SerializeNotation(llsdOne);
            LLSDMap llsdOneDS = (LLSDMap)LLSDParser.DeserializeNotation(sOne);
            Assert.AreEqual(LLSDType.Map, llsdOneDS.Type);
            Assert.AreEqual(0, llsdOneDS.Count);

            LLSD llsdTwo = LLSD.FromInteger(123234);
            LLSD llsdThree = LLSD.FromString("asedkfjhaqweiurohzasdf");
            LLSDMap llsdFour = new LLSDMap();
            llsdFour["test0"] = llsdTwo;
            llsdFour["test1"] = llsdThree;

            llsdOne["test0"] = llsdTwo;
            llsdOne["test1"] = llsdThree;
            llsdOne["test2"] = llsdFour;

            string sFive = LLSDParser.SerializeNotation(llsdOne);
            LLSDMap llsdFive = (LLSDMap)LLSDParser.DeserializeNotation(sFive);
            Assert.AreEqual(LLSDType.Map, llsdFive.Type);
            Assert.AreEqual(3, llsdFive.Count);
            Assert.AreEqual(LLSDType.Integer, llsdFive["test0"].Type);
            Assert.AreEqual(123234, llsdFive["test0"].AsInteger());
            Assert.AreEqual(LLSDType.String, llsdFive["test1"].Type);
            Assert.AreEqual("asedkfjhaqweiurohzasdf", llsdFive["test1"].AsString());

            LLSDMap llsdSix = (LLSDMap)llsdFive["test2"];
            Assert.AreEqual(LLSDType.Map, llsdSix.Type);
            Assert.AreEqual(2, llsdSix.Count);
            Assert.AreEqual(LLSDType.Integer, llsdSix["test0"].Type);
            Assert.AreEqual(123234, llsdSix["test0"].AsInteger());
            Assert.AreEqual(LLSDType.String, llsdSix["test1"].Type);
            Assert.AreEqual("asedkfjhaqweiurohzasdf", llsdSix["test1"].AsString());

            // We test here also for 4byte characters as map keys
            string xml = "<x>&#x10137;</x>";
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            XmlTextReader xtr = new XmlTextReader(new MemoryStream(bytes, false));
            xtr.Read();
            xtr.Read();
            string content = xtr.ReadString();

            LLSDMap llsdSeven = new LLSDMap();
            llsdSeven[content] = LLSD.FromString(content);
            string sSeven = LLSDParser.SerializeNotation(llsdSeven);
            LLSDMap llsdSevenDS = (LLSDMap)LLSDParser.DeserializeNotation(sSeven);
            Assert.AreEqual(LLSDType.Map, llsdSevenDS.Type);
            Assert.AreEqual(1, llsdSevenDS.Count);
            Assert.AreEqual(content, llsdSevenDS[content].AsString());
        }

        [Test()]
        public void DeserializeRealWorldExamples()
        {
            string realWorldExample = @"
[
  {'destination':'http://secondlife.com'}, 
  {'version':i1}, 
  {
    'agent_id':u3c115e51-04f4-523c-9fa6-98aff1034730, 
    'session_id':u2c585cec-038c-40b0-b42e-a25ebab4d132, 
    'circuit_code':i1075, 
    'first_name':'Phoenix', 
    'last_name':'Linden',
    'position':[r70.9247,r254.378,r38.7304], 
    'look_at':[r-0.043753,r-0.999042,r0], 
    'granters':[ua2e76fcd-9360-4f6d-a924-000000000003],
    'attachment_data':
    [
      {
        'attachment_point':i2,
        'item_id':ud6852c11-a74e-309a-0462-50533f1ef9b3,
        'asset_id':uc69b29b1-8944-58ae-a7c5-2ca7b23e22fb
      },
      {
        'attachment_point':i10, 
        'item_id':uff852c22-a74e-309a-0462-50533f1ef900,
        'asset_id':u5868dd20-c25a-47bd-8b4c-dedc99ef9479
      }
    ]
  }
]";
            // We dont do full testing here. We are fine if a few values are right
            // and the parser doesnt throw an exception
            LLSDArray llsdArray = (LLSDArray)LLSDParser.DeserializeNotation(realWorldExample);
            Assert.AreEqual(LLSDType.Array, llsdArray.Type);
            Assert.AreEqual(3, llsdArray.Count);

            LLSDMap llsdMapOne = (LLSDMap)llsdArray[0];
            Assert.AreEqual(LLSDType.Map, llsdMapOne.Type);
            Assert.AreEqual("http://secondlife.com", llsdMapOne["destination"].AsString());

            LLSDMap llsdMapTwo = (LLSDMap)llsdArray[1];
            Assert.AreEqual(LLSDType.Map, llsdMapTwo.Type);
            Assert.AreEqual(LLSDType.Integer, llsdMapTwo["version"].Type);
            Assert.AreEqual(1, llsdMapTwo["version"].AsInteger());

            LLSDMap llsdMapThree = (LLSDMap)llsdArray[2];
            Assert.AreEqual(LLSDType.UUID, llsdMapThree["session_id"].Type);
            Assert.AreEqual("2c585cec-038c-40b0-b42e-a25ebab4d132", llsdMapThree["session_id"].AsString());
            Assert.AreEqual(LLSDType.UUID, llsdMapThree["agent_id"].Type);
            Assert.AreEqual("3c115e51-04f4-523c-9fa6-98aff1034730", llsdMapThree["agent_id"].AsString());

        }

        [Test()]
        public void SerializeFormattedTest()
        {
            // This is not a real test. Instead look at the console.out tab for how formatted notation looks like.
            LLSDArray llsdArray = new LLSDArray();
            LLSD llsdOne = LLSD.FromInteger(1);
            LLSD llsdTwo = LLSD.FromInteger(1);
            llsdArray.Add(llsdOne);
            llsdArray.Add(llsdTwo);

            string sOne = LLSDParser.SerializeNotationFormatted(llsdArray);
            Console.Write(sOne);

            LLSDMap llsdMap = new LLSDMap();
            LLSD llsdThree = LLSD.FromInteger(2);
            llsdMap["test1"] = llsdThree;
            LLSD llsdFour = LLSD.FromInteger(2);
            llsdMap["test2"] = llsdFour;

            llsdArray.Add(llsdMap);

            string sTwo = LLSDParser.SerializeNotationFormatted(llsdArray);
            Console.Write(sTwo);

            LLSDArray llsdArrayTwo = new LLSDArray();
            LLSD llsdFive = LLSD.FromString("asdflkhjasdhj");
            LLSD llsdSix = LLSD.FromString("asdkfhasjkldfghsd");
            llsdArrayTwo.Add(llsdFive);
            llsdArrayTwo.Add(llsdSix);

            llsdMap["test3"] = llsdArrayTwo;

            string sThree = LLSDParser.SerializeNotationFormatted(llsdArray);
            Console.Write(sThree);

            // we also try to parse this... and look a little at the results 
            LLSDArray llsdSeven = (LLSDArray)LLSDParser.DeserializeNotation(sThree);
            Assert.AreEqual(LLSDType.Array, llsdSeven.Type);
            Assert.AreEqual(3, llsdSeven.Count);
            Assert.AreEqual(LLSDType.Integer, llsdSeven[0].Type);
            Assert.AreEqual(1, llsdSeven[0].AsInteger());
            Assert.AreEqual(LLSDType.Integer, llsdSeven[1].Type);
            Assert.AreEqual(1, llsdSeven[1].AsInteger());

            Assert.AreEqual(LLSDType.Map, llsdSeven[2].Type);
            // thats enough for now.            
        }
    }
}