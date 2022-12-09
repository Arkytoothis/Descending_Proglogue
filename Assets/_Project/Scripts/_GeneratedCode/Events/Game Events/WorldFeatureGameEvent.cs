using Descending.Features;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "WorldFeatureGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "WorldFeature Event",
	    order = 120)]
	public sealed class WorldFeatureGameEvent : GameEventBase<WorldFeature>
	{
	}
}