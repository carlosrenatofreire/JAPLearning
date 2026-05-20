namespace JAPLearning.Helper.StaticVariables
{
    public static class GeneralVariableHelper
    {
        // ─── Contract Status ───────────────────────────────────────────────
        public const string ContractStatusActive = "Active";
        public const string ContractStatusClosed = "Closed";
        public const string ContractStatusCanceled = "Canceled";

        // ─── Vehicle ───────────────────────────────────────────────────────
        public const int VehicleMinManufactureYear = 1900;

        // ─── HTTP Response Status ──────────────────────────────────────────
        public const int HttpStatusCodeMin = 100;
        public const int HttpStatusCodeMax = 599;

        // ─── HTTP Methods ──────────────────────────────────────────────────
        public const string HttpMethodGet = "GET";
        public const string HttpMethodPost = "POST";
        public const string HttpMethodPut = "PUT";
        public const string HttpMethodPatch = "PATCH";
        public const string HttpMethodDelete = "DELETE";

        // ─── Field Lengths ─────────────────────────────────────────────────
        public const int NameMinLength = 2;
        public const int NameShortMaxLength = 50;
        public const int NameDefaultMaxLength = 100;
        public const int NameLongMaxLength = 200;
        public const int DescriptionMaxLength = 200;
        public const int TagMaxLength = 50;
        public const int EmailMaxLength = 150;
        public const int PhoneMaxLength = 20;
        public const int PlateMaxLength = 20;
        public const int LicenseNumberMaxLength = 50;
        public const int StatusMaxLength = 50;
        public const int CreatedByMaxLength = 100;
        public const int ChangedByMaxLength = 100;
        public const int UserNameMinLength = 3;
        public const int UserNameMaxLength = 100;
        public const int PasswordMinLength = 6;

        // ─── AES Crypto ────────────────────────────────────────────────────
        public const int AesKeyRequiredBytes = 32;
        public const int HmacKeyMinBytes = 32;
        public const int AesIvBytes = 16;
        public const int HmacTagBytes = 32;

        // ─── Application ──────────────────────────────────────────────────
        public const string AppName = "DMC — Aluguer de Viaturas";
        public const string AppVersion = "1.0.0";
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public const string DateFormat = "dd/MM/yyyy";

        // ─── Application Source (ApiLog) ──────────────────────────────────
        public const string AppSourceApi = "API";
        public const string AppSourceMvc = "MVC";

        // ─── Seeder Defaults ───────────────────────────────────────────────
        public const string PasswordDefault = "Admin@123456";
    }
}
