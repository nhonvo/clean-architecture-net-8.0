namespace CleanArchitecture.Domain.Constants;

public static class ErrorMessage
{
    public static string Internal = "something went wrong";
    public static string NotFoundMessage = "Could not find";
    public static string AppConfigurationMessage = "Can not get appsetting variables";
    public static string TransactionNotCommit = "Transaction can not commit";
    public static string TransactionNotExecute = "Transaction can not execute";
}
