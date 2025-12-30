namespace EventSalesBackend.Exceptions.Companies;

public class CompanyNotFoundException : Exception
{
    public CompanyNotFoundException(string companyId) : base($"{companyId}  not found")
    {
        
    }
}