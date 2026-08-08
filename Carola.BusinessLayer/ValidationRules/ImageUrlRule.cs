using System;

namespace Carola.BusinessLayer.ValidationRules
{
    public static class ImageUrlRule
    {
        public static bool BeAValidImageUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return false;

            return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
        }
    }
}
