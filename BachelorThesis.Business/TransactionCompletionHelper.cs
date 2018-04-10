using System;
using BachelorThesis.Business.DataModels;

namespace BachelorThesis.Business
{
    public static class TransactionCompletionHelper
    {
        public static float GetNumberValue(TransactionCompletion completion)
        {
            switch (completion)
            {
                case TransactionCompletion.None: return 0f;
                case TransactionCompletion.Requested: return 0.20f;
                case TransactionCompletion.Declined: return 0.25f;
                case TransactionCompletion.Quitted: return 0.25f;
                case TransactionCompletion.Promised: return 0.40f;
                case TransactionCompletion.Executed: return 0.60f;
                case TransactionCompletion.Stated: return 0.80f;
                case TransactionCompletion.Rejected: return 0.5f;
                case TransactionCompletion.Stopped: return 0.5f;
                case TransactionCompletion.Accepted: return 1f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}