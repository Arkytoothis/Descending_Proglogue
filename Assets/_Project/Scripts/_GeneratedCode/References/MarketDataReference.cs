using Descending.Features;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class MarketDataReference : BaseReference<MarketData, MarketDataVariable>
	{
	    public MarketDataReference() : base() { }
	    public MarketDataReference(MarketData value) : base(value) { }
	}
}