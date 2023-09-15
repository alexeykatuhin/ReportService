using System;

namespace ReportService.Domain
{
    public static class DateHepler
    {
        public static bool IsValidDate(int year, int month)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                return false;

            if (month < 1 || month > 12)
                return false;

            return true;
        }
    }
}