namespace WriteDry.Utils {
	public static class Calculations {
		public static float CalculateDiscount(float Cost, float Discount) => Cost - (Cost * (Discount / 100));

		public static float GetDiscount(float Cost, float DiscountAmount) => (Cost * (DiscountAmount / 100));

    }
}
