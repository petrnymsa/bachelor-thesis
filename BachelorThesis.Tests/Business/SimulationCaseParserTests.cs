using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BachelorThesis.Business;
using BachelorThesis.Business.DataModels;
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


        [Test]
        public void Parse_SmallCase_ShouldReturnCorrectData()
        {

            var expectedFirst = new TransactionEvent(1,1,1, new DateTime(2018,2,1,9,0,0),TransactionCompletion.Requested);

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Business/XmlTestFiles/ChunksExample.xml");
            var xml = File.ReadAllText(path);

            var parser = new SimulationCaseParser();

            var result = parser.Parse(xml);

            result.ProcessInstance.Should().NotBeNull();

            var transactions = result.ProcessInstance.GetTransactions();
            var chunks = result.Chunks;

            transactions.Should().NotBeEmpty();
            chunks.Should().NotBeEmpty().And.HaveCount(2);

            var firstChunk = chunks[0];
            var secondChunk = chunks[1];

            var firstEvents = firstChunk.GetEvents();
            var secondEvents = secondChunk.GetEvents();

            firstEvents.Should().NotBeEmpty().And.HaveCount(1);
            secondEvents.Should().NotBeEmpty().And.HaveCount(1);

            var firstEvent = firstEvents.FirstOrDefault();
            firstEvent.Should().NotBeNull().And.BeEquivalentTo(
                new TransactionEvent(1, 1, 1, new DateTime(2018, 2, 1, 9, 0, 0), TransactionCompletion.Requested)
                , options => options.Excluding(p => p.Id));
            //            firstEvent.Completion.Should().HaveFlag(TransactionCompletion.Requested);


            var secondEvent = secondEvents.FirstOrDefault();
            secondEvent.Should().NotBeNull().And.BeEquivalentTo(
                new TransactionEvent(2,2, 4, new DateTime(2018, 2, 1, 9, 1, 0), TransactionCompletion.Stated),
                options => options.Excluding(p => p.Id));

        }
    }
}
