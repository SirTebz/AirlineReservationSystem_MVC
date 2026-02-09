namespace AirlineReservationSystem.Helpers
{
    public static class CurrencyHelper
    {
        public static string FormatPrice(decimal price)
        {
            return $"R {price:N2}";
        }
        
        public static string CurrencySymbol => "R";
        public static string CurrencyCode => "ZAR";
    }
}
