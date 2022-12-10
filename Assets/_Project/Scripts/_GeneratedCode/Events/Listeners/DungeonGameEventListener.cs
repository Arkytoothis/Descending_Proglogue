using Descending.Features;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Dungeon")]
	public sealed class DungeonGameEventListener : BaseGameEventListener<Dungeon, DungeonGameEvent, DungeonUnityEvent>
	{
	}
}