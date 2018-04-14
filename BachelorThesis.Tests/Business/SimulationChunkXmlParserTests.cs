using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BachelorThesis.Business.Parsers;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BachelorThesis.Tests.Business
{
    [TestFixture()]
    public class SimulationChunkXmlParserTests
    {
        [Test]
        public void Parse_ValidXml_ShouldReturnParsedObject()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Business/XmlTestFiles/ChunksExample.xml");
            var xDoc = XDocument.Load(path);

            var parser = new SimulationChunksXmlParser();

            //throw new Exception();
            Action parseAction = () => parser.Parse(xDoc.Root);

            parseAction.Should().NotThrow<Exception>();

       //     Assert.AreEqual(0,10);
         
        }
    }
}
