using Descending.Features;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class WorldFeatureEvent : UnityEvent<WorldFeature> { }

	[CreateAssetMenu(
	    fileName = "WorldFeatureVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "WorldFeature Event",
	    order = 120)]
	public class WorldFeatureVariable : BaseVariable<WorldFeature, WorldFeatureEvent>
	{
	}
}