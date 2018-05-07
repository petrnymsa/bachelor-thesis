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
        public const string Case03 = "BachelorThesis.Business.SimulationCases.case-03.xml";

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
    }
}
