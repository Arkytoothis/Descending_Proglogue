using Descending.Tiles;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "MapPosition")]
	public sealed class MapPositionGameEventListener : BaseGameEventListener<MapPosition, MapPositionGameEvent, MapPositionUnityEvent>
	{
	}
}