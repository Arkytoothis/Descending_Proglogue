using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using UnityEngine;

namespace Descending.Attributes
{
    [System.Serializable]
    public class Attribute
    {
        [SerializeField] private string _key = "";
        [SerializeField] private int _current = 0;
        [SerializeField] private int _maximum = 0;
        [SerializeField] private int _modifier = 0;
        [SerializeField] private int _spent = 0;

        public string Key => _key;
        public int Current { get => _current; }
        public int Maximum { get => _maximum; }
        public int Modifier { get => _modifier; }
        public int Spent { get => _spent; }
        
        public Attribute(string key)
        {
            _key = key;
            _current = 0;
            _maximum = 0;
            _modifier = 0;
            _spent = 0;
        }
        
        public Attribute(string key, int maximum)
        {
            _key = key;
            _current = maximum;
            _maximum = maximum;
            _modifier = 0;
            _spent = 0;
        }
        
        public Attribute(StartingCharacteristic startingCharacteristic)
        {
            _key = startingCharacteristic.Attribute.Key;
            _current = startingCharacteristic.MaximumValue;
            _maximum = startingCharacteristic.MaximumValue;
            _modifier = 0;
            _spent = 0;
        }
        
        public void Setup(int maximum)
        {
            _current = maximum;
            _maximum = maximum;
            _modifier = 0;
            _spent = 0;
        }

        public void AddValue(int value)
        {
            _maximum += value;
            _current += value;
        }
        
        public void Damage(int amount)
        {
            _current -= amount;

            //if (_current < 0) _current = 0;
        }

        public void Restore(int amount)
        {
            _current += amount;

            if (_current > _maximum) _current = _maximum;
        }

        public void Refresh()
        {
            _current = _maximum;
        }

        public static AttributeDictionary ConvertToDictionary(List<Attribute> list)
        {
            AttributeDictionary dictionary = new AttributeDictionary();

            foreach (Attribute attribute in list)
            {
                dictionary.Add(attribute.Key, attribute);
            }

            return dictionary;
        }

        public static List<Attribute> ConvertToList(AttributeDictionary dictionary)
        {
            List<Attribute> list = new List<Attribute>();

            foreach (var kvp in dictionary)
            {
                list.Add(kvp.Value);
            }

            return list;
        }
    }
}