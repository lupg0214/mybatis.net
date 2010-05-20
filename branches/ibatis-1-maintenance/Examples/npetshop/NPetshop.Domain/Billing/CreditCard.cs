using System;


namespace NPetshop.Domain.Billing
{

	/// <summary>
	/// Business entity used to model credit card information.
	/// </summary>
	[Serializable]
	public class CreditCard{

		#region Private Fields
		private string _cardType;
		private string _cardNumber;
		private string _cardExpiration;
		#endregion

		#region Properties 
		/// <summary>
		/// Default constructor
		/// </summary>
		public CreditCard()
		{
		}

		/// <summary>
		/// Constructor with specified initial values
		/// </summary>
		/// <param name="cardType">Card type, e.g. Visa, Master Card, American Express</param>
		/// <param name="cardNumber">Number on the card</param>
		/// <param name="cardExpiration">Expiry Date, form  MM/YY</param>
		public CreditCard(string cardType, string cardNumber, string cardExpiration){
			this._cardType = cardType;
			this._cardNumber = cardNumber;
			this._cardExpiration = cardExpiration;
		}

		// Properties
		public string CardType {
			get { return _cardType; }
			set { _cardType = value; }
		}

		public string CardNumber {
			get { return _cardNumber; }
			set { _cardNumber = value; }
		}

		public string CardExpiration {
			get { return _cardExpiration; }
			set { _cardExpiration = value; }
		}
		#endregion
	}
}
