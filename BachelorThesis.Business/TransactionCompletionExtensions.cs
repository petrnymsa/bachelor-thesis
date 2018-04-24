using System;
using BachelorThesis.Business.DataModels;

namespace BachelorThesis.Business
{
    public static class TransactionCompletionExtensions
    {
        public static float ToPercentValue(this TransactionCompletion completion)
        {
            switch (completion)
            {
                case TransactionCompletion.None: return 0f;
                case TransactionCompletion.Requested: return 0.001f;
                case TransactionCompletion.Promised: return 0.25f;
                case TransactionCompletion.Executed: return 0.50f;
                case TransactionCompletion.Stated: return 0.75f;
                case TransactionCompletion.Accepted: return 1f;
                case TransactionCompletion.Declined: return 0.10f;
                case TransactionCompletion.Quitted: return 0.10f;
                case TransactionCompletion.Rejected: return 0.50f;
                case TransactionCompletion.Stopped: return 0.50f;
         
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public static string AsAbbreviation(this TransactionCompletion completion)
        {
            switch (completion)
            {
                case TransactionCompletion.None: return "xx";
                case TransactionCompletion.Requested: return "Rq";
                case TransactionCompletion.Declined: return "Dc";
                case TransactionCompletion.Quitted: return "Qt";
                case TransactionCompletion.Promised: return "Pm";
                case TransactionCompletion.Executed: return "Ex";
                case TransactionCompletion.Stated: return "St";
                case TransactionCompletion.Rejected: return "Rj";
                case TransactionCompletion.Stopped: return "St";
                case TransactionCompletion.Accepted: return "Ac";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static float RemainingAsWidth(this TransactionCompletion completion, float widthRequest)
        {
            var total = 0f;
            for (var i = (int) completion + 1; i <= 5; i++)
                total += 0.25f * widthRequest;

            return total;
        }
    }
}