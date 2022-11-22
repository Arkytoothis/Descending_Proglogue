using Descending.Gui;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class TooltipTextEvent : UnityEvent<TooltipText> { }

	[CreateAssetMenu(
	    fileName = "TooltipTextVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "TooltipText Event",
	    order = 120)]
	public class TooltipTextVariable : BaseVariable<TooltipText, TooltipTextEvent>
	{
	}
}