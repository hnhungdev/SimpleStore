namespace Catalog.Domain.Helpers;

public static class SlugHelper
{
    public static string Generate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        value = value.Trim().ToLowerInvariant();

        value = value
            .Replace("à", "a")
            .Replace("á", "a")
            .Replace("ạ", "a")
            .Replace("ả", "a")
            .Replace("ã", "a")
            .Replace("ă", "a")
            .Replace("ằ", "a")
            .Replace("ắ", "a")
            .Replace("ặ", "a")
            .Replace("ẳ", "a")
            .Replace("ẵ", "a")
            .Replace("â", "a")
            .Replace("ầ", "a")
            .Replace("ấ", "a")
            .Replace("ậ", "a")
            .Replace("ẩ", "a")
            .Replace("ẫ", "a")
            .Replace("đ", "d")
            .Replace("è", "e")
            .Replace("é", "e")
            .Replace("ẹ", "e")
            .Replace("ẻ", "e")
            .Replace("ẽ", "e")
            .Replace("ê", "e")
            .Replace("ề", "e")
            .Replace("ế", "e")
            .Replace("ệ", "e")
            .Replace("ể", "e")
            .Replace("ễ", "e")
            .Replace("ì", "i")
            .Replace("í", "i")
            .Replace("ị", "i")
            .Replace("ỉ", "i")
            .Replace("ĩ", "i")
            .Replace("ò", "o")
            .Replace("ó", "o")
            .Replace("ọ", "o")
            .Replace("ỏ", "o")
            .Replace("õ", "o")
            .Replace("ô", "o")
            .Replace("ồ", "o")
            .Replace("ố", "o")
            .Replace("ộ", "o")
            .Replace("ổ", "o")
            .Replace("ỗ", "o")
            .Replace("ơ", "o")
            .Replace("ờ", "o")
            .Replace("ớ", "o")
            .Replace("ợ", "o")
            .Replace("ở", "o")
            .Replace("ỡ", "o")
            .Replace("ù", "u")
            .Replace("ú", "u")
            .Replace("ụ", "u")
            .Replace("ủ", "u")
            .Replace("ũ", "u")
            .Replace("ư", "u")
            .Replace("ừ", "u")
            .Replace("ứ", "u")
            .Replace("ự", "u")
            .Replace("ử", "u")
            .Replace("ữ", "u")
            .Replace("ỳ", "y")
            .Replace("ý", "y")
            .Replace("ỵ", "y")
            .Replace("ỷ", "y")
            .Replace("ỹ", "y");

        value = System.Text.RegularExpressions.Regex
            .Replace(value, @"[^a-z0-9\s-]", "");

        value = System.Text.RegularExpressions.Regex
            .Replace(value, @"\s+", "-");

        value = System.Text.RegularExpressions.Regex
            .Replace(value, @"-+", "-");

        return value.Trim('-');
    }
}