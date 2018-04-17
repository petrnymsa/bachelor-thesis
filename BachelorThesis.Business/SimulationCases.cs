using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BachelorThesis.Business
{
    public class SimulationCases
    {
        public const string Case01 = "BachelorThesis.Business.SimulationCases.case-01.xml";
        public const string Case02 = "BachelorThesis.Business.SimulationCases.case-02.xml";

        public const string ModelDefinition = "BachelorThesis.Business.SimulationCases.definition.xml";

        public static async Task<string> LoadXmlAsync(string name)
        {
            var assembly = typeof(SimulationCases).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(name);

            string xml = "";
            using (var reader = new StreamReader(stream))
                xml = await reader.ReadToEndAsync();

            return xml;
        }

        public string LiveCase1()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
<Simulation Name=""Case01"">
  <Actors>
    <Actor Id=""1"" ActorRoleId=""1"" FirstName=""George"" LastName=""Lucas"" />
    <Actor Id=""2"" ActorRoleId=""2"" FirstName=""George"" LastName=""Lucas"" />
    <Actor Id=""3"" ActorRoleId=""3"" FirstName=""Bob"" LastName=""Freeman"" />
    <Actor Id=""4"" ActorRoleId=""4"" FirstName=""Alice"" LastName=""Freeman"" />
  </Actors>
  <ProcessInstance Id=""1"" KindId=""1"" StartTime=""01-02-2018 15:34:23"" ExpectedEndTime=""01-02-2018 15:34:23"">
    <TransactionInstance Id=""1"" KindId=""1"" Identificator=""T1"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""0"">
      <TransactionInstance Id=""2"" KindId=""2"" Identificator=""T2"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""1"" />
    </TransactionInstance>
    <TransactionInstance Id=""3"" KindId=""3"" Identificator=""T3"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""0"">
      <TransactionInstance Id=""4"" KindId=""4"" Identificator=""T4"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""3"" />
      <TransactionInstance Id=""5"" KindId=""5"" Identificator=""T5"" CompletionType=""0"" ProcessInstanceId=""1"" InitiatorId=""0"" ExecutorId=""0"" ParentId=""3"" />
    </TransactionInstance>
  </ProcessInstance>
  <Chunks>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""1"" Created=""01-02-2018 09:00:00"">
        <CompletionChanged Completion=""Requested"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""4"" Created=""01-02-2018 09:01:00"">
        <CompletionChanged Completion=""Requested"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""1"" Created=""01-02-2018 09:01:10"">
        <CompletionChanged Completion=""Promised"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""4"" Created=""01-02-2018 09:01:30"">
        <CompletionChanged Completion=""Promised"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""1"" Created=""01-02-2018 09:01:40"">
        <CompletionChanged Completion=""Executed"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""1"" Created=""01-02-2018 09:02:00"">
        <CompletionChanged Completion=""Stated"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""2"" TransactionKindId=""2""  RaisedById=""4"" Created=""01-02-2018 09:02:15"">
        <CompletionChanged Completion=""Accepted"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""4"" Created=""01-02-2018 09:02:30"">
        <CompletionChanged Completion=""Executed"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""4"" Created=""01-02-2018 09:20:00"">
        <CompletionChanged Completion=""Stated"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""1"" TransactionKindId=""1""  RaisedById=""1"" Created=""01-02-2018 09:30:05"">
        <CompletionChanged Completion=""Accepted"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""2"" Created=""03-02-2018 08:30:00"">
        <CompletionChanged Completion=""Requested"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""3"" Created=""03-02-2018 08:31:00"">
        <CompletionChanged Completion=""Promised"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4""  RaisedById=""3"" Created=""03-02-2018 08:31:20"">
        <CompletionChanged Completion=""Requested"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4""  RaisedById=""2"" Created=""03-02-2018 08:31:55"">
        <CompletionChanged Completion=""Promised"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""3"" Created=""03-02-2018 08:32:00"">
        <CompletionChanged Completion=""Executed"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""3"" Created=""03-02-2018 08:36:00"">
        <CompletionChanged Completion=""Stated"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""3"" TransactionKindId=""3""  RaisedById=""2"" Created=""03-02-2018 08:40:50"">
        <CompletionChanged Completion=""Accepted"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4""  RaisedById=""2"" Created=""01-04-2018 16:30:00"">
        <CompletionChanged Completion=""Executed"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4""  RaisedById=""2"" Created=""01-04-2018 16:31:00"">
        <CompletionChanged Completion=""Stated"" />
      </Event>
    </Chunk>
    <Chunk>
      <Event Type=""0"" TransactionId=""4"" TransactionKindId=""4"" RaisedById=""3"" Created=""01-04-2018 16:45:00"">
        <CompletionChanged Completion=""Accepted"" />
      </Event>
    </Chunk>
  </Chunks>
</Simulation>";
        }
    }
}
