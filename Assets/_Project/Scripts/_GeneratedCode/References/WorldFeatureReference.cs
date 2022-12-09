using Descending.Features;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class WorldFeatureReference : BaseReference<WorldFeature, WorldFeatureVariable>
	{
	    public WorldFeatureReference() : base() { }
	    public WorldFeatureReference(WorldFeature value) : base(value) { }
	}
}