using Descending.Tiles;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "MapPositionGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "MapPosition Event",
	    order = 120)]
	public sealed class MapPositionGameEvent : GameEventBase<MapPosition>
	{
	}
}