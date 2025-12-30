namespace EventSalesBackend.Exceptions.MongoDB
{
    public class MongoFailedToUpdateException : Exception
    {
        public MongoFailedToUpdateException(string updateTargetType) : base($"Update to {updateTargetType} Failed.") { }

    }
   
}
