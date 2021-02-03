namespace pdfsvc.Business
{
    public class SignInfo
    {
        public const string Key = "SignInfo";

        public string Stamper { get; set; }
        public string Cert { get; set; }
        public string Password { get; set; }
        public string Alias { get; set; }
    }
}