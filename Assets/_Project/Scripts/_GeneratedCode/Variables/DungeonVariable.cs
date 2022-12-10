using Descending.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class DungeonEvent : UnityEvent<Dungeon> { }

	[CreateAssetMenu(
	    fileName = "DungeonVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Dungeon Event",
	    order = 120)]
	public class DungeonVariable : BaseVariable<Dungeon, DungeonEvent>
	{
	}
}