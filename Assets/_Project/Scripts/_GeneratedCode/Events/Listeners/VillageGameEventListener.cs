using Descending.Features;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Village")]
	public sealed class VillageGameEventListener : BaseGameEventListener<Village, VillageGameEvent, VillageUnityEvent>
	{
	}
}