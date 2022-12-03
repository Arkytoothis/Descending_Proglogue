using Descending.Features;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "MarketDataGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "MarketData Event",
	    order = 120)]
	public sealed class MarketDataGameEvent : GameEventBase<MarketData>
	{
	}
}