namespace Server.Common.Constants
{
  public class AuthConstants
  {
    public const string EmailRegEx = "^[A-Za-z0-9]+[\\._A-Za-z0-9-]+@([A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)+(\\.[A-Za-z0-9]+[-\\.]?[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";

    public const int PasswordMinLength = 3;
    public const int PasswordMaxLength = 100;

    public const int TokenTimeSpanMinutes = 10;

    public const string AdministratorRoleName = "Administrator";
  }
}
