using Descending.Gui;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class TooltipTextReference : BaseReference<TooltipText, TooltipTextVariable>
	{
	    public TooltipTextReference() : base() { }
	    public TooltipTextReference(TooltipText value) : base(value) { }
	}
}