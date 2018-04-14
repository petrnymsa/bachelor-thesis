namespace BachelorThesis.Business.DataModels
{
    public enum TransactionCompletion
    {
        None = 0,
        Requested,
        Promised,
        Stated,
        Executed,
        Accepted,
        Declined,
        Quitted,
        Rejected,
        Stopped,
    }
}