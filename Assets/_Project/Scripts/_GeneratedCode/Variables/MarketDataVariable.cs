using Descending.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class MarketDataEvent : UnityEvent<MarketData> { }

	[CreateAssetMenu(
	    fileName = "MarketDataVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "MarketData Event",
	    order = 120)]
	public class MarketDataVariable : BaseVariable<MarketData, MarketDataEvent>
	{
	}
}