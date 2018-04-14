using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BachelorThesis.Business.Parsers;
using NUnit.Framework;

namespace BachelorThesis.Tests.Business
{
    [TestFixture]
    public class ProcessKindXmlParserTest
    {
        [Test]
        [Ignore("Changed interface")]
        public void ParseDefinition_ValidXml_ShouldReturnCorrectResult()
        {
            //var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Business/XmlTestFiles/definition-valid.xml");
            //var xDoc = XDocument.Load(path);

            //var parser = new ProcessKindXmlParser();

            //var result = parser.ParseDefinition(XDocument.Parse());

            //Assert.NotNull(result.ProcessKind);
            //Assert.NotNull(result.ActorRoles);
            //Assert.That(result.ActorRoles.Count == 4);

        }
    }
}
