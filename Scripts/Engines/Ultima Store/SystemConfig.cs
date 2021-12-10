using System;

namespace Server.Engines.UOStore
{
    public enum CurrencyType
    {
        None,
        Sovereigns,
        Gold,
        Custom
    }

    public delegate int CustomCurrencyHandler(Mobile m, int consume);

    public static class Configuration
    {
		public static bool Enabled = true;

		public static string Website = "https://uo.com/ultima-store/";

		public static CurrencyType CurrencyImpl = CurrencyType.Sovereigns;

		public static string CurrencyName = "Sovereigns";

		public static bool CurrencyDisplay = true;

		public static double CostMultiplier = 1.0;

		public static int CartCapacity = 10;

		/// <summary>
		///     A hook to allow handling of custom currencies.
		///     This implementation should be treated as such;
		///     If 'consume' is less than zero, return the currency total.
		///     Else deduct from the currency total, return the amount consumed.
		/// </summary>
		public static CustomCurrencyHandler ResolveCurrency { get; set; }

        public static int GetCustomCurrency(Mobile m)
        {
            if (ResolveCurrency != null)
            {
                return ResolveCurrency(m, -1);
            }

            m.SendMessage(1174, "Currency is not set up for this system. Contact a shard administrator.");

			Utility.PushColor(ConsoleColor.Red);
			Console.WriteLine("[Ultima Store]: No custom currency method has been implemented.");
			Utility.PopColor();

			return 0;
        }

        public static int DeductCustomCurrecy(Mobile m, int amount)
        {
            if (ResolveCurrency != null)
            {
                return ResolveCurrency(m, amount);
            }

            m.SendMessage(1174, "Currency is not set up for this system. Contact a shard administrator.");

			Utility.PushColor(ConsoleColor.Red);
			Console.WriteLine("[Ultima Store]: No custom currency deduction method has been implemented.");
			Utility.PopColor();

            return 0;
        }
    }
}
