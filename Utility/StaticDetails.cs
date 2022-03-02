namespace Spice.Utility
{
	public static class StaticDetails
	{
		// This array is used for the menu items
		public static readonly string[] SpicynessLevels = { "Not Applicable", "Not Spicy", "Spicy", "Very Spicy" };

		public const string DefaultFoodImage = "default_food.png";

		// Roles
		public const string Manager = "Manager";
		public const string Kitchen = "Kitchen";
		public const string FrontDesk = "FrontDesk";

		// Order Status
		public const string StatusSubmitted = "Submitted";
		public const string StatusInProgress = "Being Prepared";
		public const string StatusReady = "Ready For Pickup";
		public const string StatusCompleted = "Completed";
		public const string StatusCancelled = "Cancelled";

		// Payment Status
		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusRejected = "Rejected";

		// Session String
		public const string CartCount = "ssCartCount";
		public const string CouponCode = "ssCouponCode";
	}
}