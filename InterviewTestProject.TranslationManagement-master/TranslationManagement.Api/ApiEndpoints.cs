namespace TranslationManagement.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Jobs
    {
        private const string Base = $"{ApiBase}/jobs";

        public const string Create = Base;
        public const string CreateWithFile = $"{Base}/createWithFile";
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetAll = Base;
        public const string UpdateStatus = $"{Base}/updateStatus";
    }

    public static class Translators
    {
        private const string Base = $"{ApiBase}/translators";

        public const string Create = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string GetTranslatorsByName = $"{Base}/{{name}}";
        public const string GetAll = Base;
        public const string UpdateStatus = $"{Base}/updateStatus";
    }
}
