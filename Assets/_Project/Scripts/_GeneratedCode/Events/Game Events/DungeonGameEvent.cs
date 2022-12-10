using Descending.Features;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "DungeonGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "Dungeon Event",
	    order = 120)]
	public sealed class DungeonGameEvent : GameEventBase<Dungeon>
	{
	}
}