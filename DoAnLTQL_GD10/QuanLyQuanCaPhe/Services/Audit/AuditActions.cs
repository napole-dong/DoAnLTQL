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
    public const string VoidInvoice = "VOID_INVOICE";
    public const string DeleteInvoice = "DELETE_INVOICE";
    public const string AddItem = "ADD_ITEM";
    public const string RemoveItem = "REMOVE_ITEM";
    public const string ReplaceItem = "REPLACE_ITEM";
    public const string PayInvoice = "PAY_INVOICE";

    public const string CreateCustomer = "CREATE_CUSTOMER";
    public const string UpdateCustomer = "UPDATE_CUSTOMER";
    public const string DeleteCustomer = "DELETE_CUSTOMER";
    public const string RestoreCustomer = "RESTORE_CUSTOMER";
    public const string HardDeleteCustomer = "HARD_DELETE_CUSTOMER";

    public const string CreateRecipe = "CREATE_RECIPE";
    public const string UpdateRecipe = "UPDATE_RECIPE";
    public const string DeleteRecipe = "DELETE_RECIPE";

    public const string CreateCategory = "CREATE_CATEGORY";
    public const string UpdateCategory = "UPDATE_CATEGORY";
    public const string DeleteCategory = "DELETE_CATEGORY";
    public const string TransferCategoryItems = "TRANSFER_CATEGORY_ITEMS";

    public const string CreateIngredient = "CREATE_INGREDIENT";
    public const string UpdateIngredient = "UPDATE_INGREDIENT";
    public const string DeleteIngredient = "DELETE_INGREDIENT";

    public const string PrintOriginal = "PRINT_ORIGINAL";
    public const string PrintCopy = "PRINT_COPY";
    public const string PrintDraft = "PRINT_DRAFT";
    public const string PrintBlocked = "PRINT_BLOCKED";
}
