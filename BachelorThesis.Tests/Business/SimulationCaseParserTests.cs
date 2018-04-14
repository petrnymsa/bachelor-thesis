using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business;
using BachelorThesis.Business.Parsers;
using FluentAssertions;
using NUnit.Framework;

namespace BachelorThesis.Tests.Business
{
    [TestFixture]
    public class SimulationCaseParserTests
    {
        [Test]
        [TestCase(SimulationCases.Case01)]
        [TestCase(SimulationCases.Case02)]
        public async Task Parse_Cases_ShouldReturnWithoutException(string caseName)
        {
            var assembly = typeof(SimulationCases).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(caseName);
            string xml = "";
            using (var reader = new StreamReader(stream))
                xml = await reader.ReadToEndAsync();


            var parser = new SimulationCaseParser();

            Action parse = () => parser.Parse(xml);
                
            parse.Should().NotThrow<Exception>();
        }
    }
}
