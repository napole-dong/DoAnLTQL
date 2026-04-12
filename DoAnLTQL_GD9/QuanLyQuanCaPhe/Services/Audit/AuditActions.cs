namespace QuanLyQuanCaPhe.Services.Audit;

public static class AuditActions
{
    public const string LoginSuccess = "LOGIN_SUCCESS";
    public const string LoginFailed = "LOGIN_FAILED";
    public const string Logout = "LOGOUT";

    public const string CreateUser = "CREATE_USER";
    public const string UpdateUser = "UPDATE_USER";
    public const string DeleteUser = "DELETE_USER";

    public const string CreateProduct = "CREATE_PRODUCT";
    public const string UpdateProduct = "UPDATE_PRODUCT";
    public const string DeleteProduct = "DELETE_PRODUCT";

    public const string CreateInvoice = "CREATE_INVOICE";
    public const string UpdateInvoice = "UPDATE_INVOICE";
    public const string DeleteInvoice = "DELETE_INVOICE";
    public const string AddItem = "ADD_ITEM";
    public const string RemoveItem = "REMOVE_ITEM";
    public const string ReplaceItem = "REPLACE_ITEM";
    public const string PayInvoice = "PAY_INVOICE";
}
