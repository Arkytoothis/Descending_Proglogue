using Descending.Units;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "PartyData")]
	public sealed class PartyDataGameEventListener : BaseGameEventListener<PartyData, PartyDataGameEvent, PartyDataUnityEvent>
	{
	}
}