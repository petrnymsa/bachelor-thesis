﻿using System;
using System.Globalization;
using System.Xml.Linq;
using BachelorThesis.Bussiness.DataModels;

namespace BachelorThesis.ConsoleTest
{
    public class ProcessInstanceXmlParser
    {
        private const string IdAttribute = "Id";
        private const string KindIdAttribute = "KindId";
        private const string StartTimeAttribute = "StartTime";
        private const string ExpectedEndTimeAttribute = "ExpectedEndTime";
        private const string IdentificatorAttribute = "Identificator";
        private const string CompletionTypeAttribute = "CompletionType";
        private const string ProcessInstanceIdAttribute = "ProcessInstanceId";
        private const string InitiatorIdAttribute = "InitiatorId";
        private const string ExecutorIdAttribute = "ExecutorId";
        private const string ParentIdAttribute = "ParentId";

        private const string TransactionInstanceElement = "TransactionInstance";
        private const string ProcessInstanceElement = "ProcessInstance";

        public ProcessInstance Parse(XElement root)
        {
            var processInstance = new ProcessInstance();

            var processInstanceElement = root.Element(ProcessInstanceElement);

            if (processInstanceElement == null) throw new Exception("ProcessInstanceElement not found");

            var processId = int.Parse(processInstanceElement.Attribute(IdAttribute).Value);
            var processKindId = int.Parse(processInstanceElement.Attribute(KindIdAttribute).Value);
            var processStartTime = DateTime.ParseExact(processInstanceElement.Attribute(StartTimeAttribute).Value, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture);
            var processExpectedEndTime = DateTime.ParseExact(processInstanceElement.Attribute(ExpectedEndTimeAttribute).Value, XmlParsersConfig.DateTimeFormat, CultureInfo.InvariantCulture);

            processInstance.Id = processId;
            processInstance.ProcessKindId = processKindId;
            processInstance.StartTime = processStartTime;
            processInstance.ExpectedEndTime = processExpectedEndTime;

            var transactionElements = processInstanceElement.Elements(TransactionInstanceElement);

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
            var childElements = element.Elements(TransactionInstanceElement);
            foreach (var childElement in childElements)
            {
                var childInstance = ParseTransactionInstance(childElement);
                instance.AddChild(childInstance);

                ParseTransactionChildren(childElement, childInstance);
            }
        }

        private TransactionInstance ParseTransactionInstance(XElement element)
        {
            var id = int.Parse(element.Attribute(IdAttribute).Value);
            var kindId = int.Parse(element.Attribute(KindIdAttribute).Value);
            var identificator = element.Attribute(IdentificatorAttribute).Value;
            var completionType = (TransactionCompletion)int.Parse(element.Attribute(CompletionTypeAttribute).Value);
            var processInstanceId = int.Parse(element.Attribute(ProcessInstanceIdAttribute).Value);

            var initiatorId = Int32.TryParse(element.Attribute(InitiatorIdAttribute).Value, out var tmpInitiatorId) ? tmpInitiatorId : (int?)null;
            var executorId = Int32.TryParse(element.Attribute(ExecutorIdAttribute).Value, out var tmpExecutorId) ? tmpExecutorId : (int?)null;
            var parentId = Int32.TryParse(element.Attribute(ParentIdAttribute).Value, out var tmpParentId) ? tmpParentId : (int?)null;

            var instance = new TransactionInstance()
            {
                CompletionType = completionType,
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
    }
}