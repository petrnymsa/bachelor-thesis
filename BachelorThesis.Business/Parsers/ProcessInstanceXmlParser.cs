using System;
using System.Globalization;
using System.Xml.Linq;
using BachelorThesis.Business.DataModels;

namespace BachelorThesis.Business.Parsers
{
    public class ProcessInstanceXmlParser
    {
        //private const string IdAttribute = "Id";
        //private const string KindIdAttribute = "KindId";
        //private const string StartTimeAttribute = "StartTime";
        //private const string ExpectedEndTimeAttribute = "ExpectedEndTime";
        //private const string IdentificatorAttribute = "Identificator";
        //private const string CompletionTypeAttribute = "CompletionType";
        //private const string ProcessInstanceIdAttribute = "ProcessInstanceId";
        //private const string InitiatorIdAttribute = "InitiatorId";
        //private const string ExecutorIdAttribute = "ExecutorId";
        //private const string ParentIdAttribute = "ParentId";

        //private const string TransactionInstanceElement = "TransactionInstance";
        //private const string ProcessInstanceElement = "ProcessInstance";

        public ProcessInstance Parse(XElement root)
        {
            var processInstance = new ProcessInstance();

            var processInstanceElement = root.Element(XmlParsersConfig.ElementProcessInstance);

            if (processInstanceElement == null) throw new Exception("ProcessInstanceElement not found");

            var processId = int.Parse(processInstanceElement.Attribute(XmlParsersConfig.AttributeId).Value);
            var processKindId = int.Parse(processInstanceElement.Attribute(XmlParsersConfig.AttributeKindId).Value);
            var processStartTime = DateTime.ParseExact(processInstanceElement.Attribute(XmlParsersConfig.AttributeStartTime).Value, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture);
            var processExpectedEndTime = DateTime.ParseExact(processInstanceElement.Attribute(XmlParsersConfig.AttributeExpectedEndTime).Value, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture);

            processInstance.Id = processId;
            processInstance.ProcessKindId = processKindId;
            processInstance.StartTime = processStartTime;
            processInstance.ExpectedEndTime = processExpectedEndTime;

            var transactionElements = processInstanceElement.Elements(XmlParsersConfig.ElementTransactionInstance);

            foreach (var transactionElement in transactionElements)
            {
                var transactionInstance = ParseTransactionInstance(transactionElement);

                ParseTransactionChildren(transactionElement, transactionInstance);

                processInstance.AddTransaction(transactionInstance);
            }


            return processInstance;
        }

        private void ParseTransactionChildren(XElement element, TransactionInstance instance)
        {
            var childElements = element.Elements(XmlParsersConfig.ElementTransactionInstance);
            foreach (var childElement in childElements)
            {
                var childInstance = ParseTransactionInstance(childElement);
                instance.AddChild(childInstance);

                ParseTransactionChildren(childElement, childInstance);
            }
        }

        private TransactionInstance ParseTransactionInstance(XElement element)
        {
            var id = int.Parse(element.Attribute(XmlParsersConfig.AttributeId).Value);
            var kindId = int.Parse(element.Attribute(XmlParsersConfig.AttributeKindId).Value);
            var identificator = element.Attribute(XmlParsersConfig.AttributeIdentificator).Value;
            var completionType = (TransactionCompletion)int.Parse(element.Attribute(XmlParsersConfig.AttributeCompletionType).Value);
            var processInstanceId = int.Parse(element.Attribute(XmlParsersConfig.ElementProcessInstance).Value);

            var initiatorId = Int32.TryParse(element.Attribute(XmlParsersConfig.AttributeInitiatorId).Value, out var tmpInitiatorId) ? tmpInitiatorId : (int?)null;
            var executorId = Int32.TryParse(element.Attribute(XmlParsersConfig.AttributeExecutorId).Value, out var tmpExecutorId) ? tmpExecutorId : (int?)null;
            var parentId = Int32.TryParse(element.Attribute(XmlParsersConfig.AttributeParentid).Value, out var tmpParentId) ? tmpParentId : (int?)null;

            var instance = new TransactionInstance()
            {
                Completion = completionType,
                ExecutorId = executorId,
                Id = id,
                Identificator = identificator,
                InitiatorId = initiatorId,
                ParentId = parentId,
                ProcessInstanceId = processInstanceId,
                TransactionKindId = kindId
            };

            return instance;
        }

        public XElement Create(ProcessInstance process)
        {
            var root = new XElement(XmlParsersConfig.ElementProcessInstance,
                new XAttribute(XmlParsersConfig.AttributeId, process.Id),
                new XAttribute(XmlParsersConfig.AttributeKindId, process.ProcessKindId),
                new XAttribute(XmlParsersConfig.AttributeStartTime, process.StartTime.ToString(XmlParsersConfig.DateTimeFormat)),
                new XAttribute(XmlParsersConfig.AttributeExpectedEndTime, process.ExpectedEndTime?.ToString(XmlParsersConfig.DateTimeFormat)));

            foreach (var transaction in process.GetTransactions())
            {
                var element = CreateTransactionElement(transaction);
                TreeStructureHelper.Traverse(transaction,element, (t, e) => e.Add(CreateTransactionElement(t)));
                root.Add(element);
            }


            return root;
        }

        private XElement CreateTransactionElement(TransactionInstance transaction)
        {
            return new XElement(XmlParsersConfig.ElementTransactionInstance,
                new XAttribute(XmlParsersConfig.AttributeId, transaction.Id),
                new XAttribute(XmlParsersConfig.AttributeKindId, transaction.TransactionKindId),
                new XAttribute(XmlParsersConfig.AttributeIdentificator, transaction.Identificator),
                new XAttribute(XmlParsersConfig.AttributeCompletionType, (int)transaction.Completion),
                new XAttribute(XmlParsersConfig.AttributeProcessInstanceId, transaction.ProcessInstanceId),
                new XAttribute(XmlParsersConfig.AttributeInitiatorId, transaction.InitiatorId == null ? 0 : transaction.InitiatorId) ,
                new XAttribute(XmlParsersConfig.AttributeExecutorId, transaction.ExecutorId == null ? 0 : transaction.ExecutorId),
                new XAttribute(XmlParsersConfig.AttributeParentid, transaction.ParentId == null ? 0 : transaction.ParentId));
        }
    }
}