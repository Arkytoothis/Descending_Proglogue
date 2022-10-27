using Descending.Tiles;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class MapPositionEvent : UnityEvent<MapPosition> { }

	[CreateAssetMenu(
	    fileName = "MapPositionVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "MapPosition Event",
	    order = 120)]
	public class MapPositionVariable : BaseVariable<MapPosition, MapPositionEvent>
	{
	}
}