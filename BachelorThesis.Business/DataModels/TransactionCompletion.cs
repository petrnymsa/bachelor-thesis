namespace BachelorThesis.Business.DataModels
{
    public enum TransactionCompletion
    {
        None = 0,
        Requested,
        Promised,
        Executed,
        Stated,
        Accepted,
        Declined,
        Quitted,
        Rejected,
        Stopped,
    }
}