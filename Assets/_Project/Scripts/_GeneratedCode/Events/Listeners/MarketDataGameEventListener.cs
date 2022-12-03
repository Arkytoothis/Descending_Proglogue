using Descending.Features;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "MarketData")]
	public sealed class MarketDataGameEventListener : BaseGameEventListener<MarketData, MarketDataGameEvent, MarketDataUnityEvent>
	{
	}
}