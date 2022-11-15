using DarkTonic.MasterAudio;
using System.Collections.Generic;
using System.Text;
using Descending.Attributes;
using Descending.Core;
using Descending.Units;
using Descending.Gui;
using UnityEngine;

namespace Descending.Abilities
{
    [System.Serializable]
    public class RestoreEffect : AbilityEffect
    {
        [SerializeField] private AttributeDefinition _attribute = null;
        [SerializeField] private int _minimumValue = 0;
        [SerializeField] private int _maximumValue = 0;

        //[SerializeField] private GameObject _casterEffectPrefab = null;
        //[SerializeField] private GameObject _targetEffectPrefab = null;
        
        public AttributeDefinition Attribute => _attribute;
        public int MinimumValue => _minimumValue;
        public int MaximumValue => _maximumValue;

        public override string GetTooltipText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Restores ").Append(_minimumValue).Append(" - ").Append(_maximumValue).Append(" ").Append(_attribute.Name).Append("\n");

            return sb.ToString();
        }

        public override void Process(Unit user, List<Unit> targets)
        {
            //Debug.Log("Processing RestoreEffect");
            if (_affects == AbilityEffectAffects.User)
            {
                int amount = Random.Range(_minimumValue, _maximumValue + 1);
                user.RestoreVital(_attribute.Key, amount);
                //MessageHandler.Instance.DisplayMessage(new GameMessage(user.GetName() + " gains " + amount + " " + _attribute.Name));
            }
            else if (_affects == AbilityEffectAffects.Target)
            {
                foreach (Unit entity in targets)
                {
                    int amount = Random.Range(_minimumValue, _maximumValue + 1);
                    entity.RestoreVital(_attribute.Key, amount);
                    //MessageHandler.Instance.DisplayMessage(new GameMessage(user.GetName() + " gains " + amount + " " + _attribute.Name));
                }
            }
            
           //user.SyncData();
            
            for (int i = 0; i < targets.Count; i++)
            {
                //targets[i].SyncData();
            }
        }
    }
}