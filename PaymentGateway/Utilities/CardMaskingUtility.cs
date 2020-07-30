namespace PaymentGateway.Utilities
{
    public class CardMaskingUtility
    {
        public static string Mask(string cardNumber)
        {
            return cardNumber.Substring(0, 6) + "******" + cardNumber.Substring(12);
        }
    }
}