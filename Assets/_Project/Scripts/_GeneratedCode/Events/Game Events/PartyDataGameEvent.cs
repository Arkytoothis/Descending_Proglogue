using Descending.Units;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "PartyDataGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "PartyData",
	    order = 120)]
	public sealed class PartyDataGameEvent : GameEventBase<PartyData>
	{
	}
}