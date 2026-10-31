using System.Globalization;

namespace PAC.Common.Tracking
{
    /// <summary>
    /// Route and query values that reach the data layer: customer numbers,
    /// sales order numbers and invoice numbers are letters, digits and
    /// dashes only. Shared by the public proxy and the inner service so a
    /// value is checked the same way at both doors.
    /// </summary>
    public static class Input
    {
        public static bool IsKey(string? value)
            => !string.IsNullOrWhiteSpace(value) && value.Length <= 32 && value.All(c => char.IsLetterOrDigit(c) || c == '-');

        public static bool IsSono(string? value)
        {
            bool result = false;
            long tmpSono = 0;

            if (value != null && value.Length <= 10 && value.IndexOf('\0') < 0 && long.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out tmpSono) && tmpSono > 0)
                result = true;

            return result;
        }

        public static bool IsSearch(string? value)
        {
            bool result = false;

            if (value != null && value.Trim().Length >= 2 && value.Length <= 30)
            {
                result = true;

                foreach (char c in value)
                {
                    if (!char.IsLetterOrDigit(c) && c != ' ' && c != '-')
                        result = false;
                }
            }

            return result;
        }
    }
}
