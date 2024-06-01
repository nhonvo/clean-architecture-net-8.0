namespace CleanArchitecture.Domain.Constants;

public static class ErrorMessage
{
    public static string Internal = "something went wrong";
    public static string NotFoundMessage = "Could not find";
    public static string AppConfigurationMessage = "Can not get appsettings value";
    public static string TransactionNotCommit = "Transaction not commit";
    public static string TransactionNotExecute = "Transaction not execute";
}
