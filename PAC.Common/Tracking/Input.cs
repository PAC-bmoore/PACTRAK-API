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

            if (value != null && value.Length >= 1 && value.Length <= 10)
            {
                result = true;

                foreach (char c in value)
                {
                    if (!char.IsAsciiDigit(c))
                        result = false;
                }
            }

            return result;
        }
    }
}
