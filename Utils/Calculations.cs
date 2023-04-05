namespace WriteDry.Utils
{
    public static class ProductCalculations
    {
        public static float CalculateCostWithDiscount(float Cost, float Discount) => Cost - GetDiscount(Cost, Discount);

        public static float GetDiscount(float Cost, float DiscountAmount) => (Cost * (DiscountAmount / 100));

    }
}
