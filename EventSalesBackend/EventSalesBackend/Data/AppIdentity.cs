namespace EventSalesBackend.Data;

public static class AppIdentity
{
    public static readonly string InstanceId = Guid.NewGuid().ToString("N");
}
