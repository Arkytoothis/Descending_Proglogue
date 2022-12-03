using Descending.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class MarketDataUnityEvent : UnityEvent<MarketData>
	{
	}
}