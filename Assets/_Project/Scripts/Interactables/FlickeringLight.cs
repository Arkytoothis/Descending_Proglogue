using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Descending.Interactables
{
    public class FlickeringLight : MonoBehaviour
    {
        [SerializeField] private Light _lightSource;
        [SerializeField] private float MaxReduction = 0.2f;
        [SerializeField] private float MaxIncrease = 0.2f;
        [SerializeField] private float RateDamping = 0.1f;
        [SerializeField] private float Strength = 300;
        [SerializeField] private bool StopFlickering;
        
        private float _baseIntensity;
        private bool _flickering;
 
        public void Reset()
        {
            MaxReduction = 0.2f;
            MaxIncrease = 0.2f;
            RateDamping = 0.1f;
            Strength = 300;
        }
 
        public void Start()
        {
            _baseIntensity = _lightSource.intensity;
            StartCoroutine(DoFlicker());
        }
 
        void Update()
        {
            if (!StopFlickering && !_flickering)
            {
                StartCoroutine(DoFlicker());
            }
        }
 
        private IEnumerator DoFlicker()
        {
            _flickering = true;
            while (!StopFlickering)
            {
                _lightSource.intensity = Mathf.Lerp(_lightSource.intensity, Random.Range(_baseIntensity - MaxReduction, _baseIntensity + MaxIncrease), Strength * Time.deltaTime);
                yield return new WaitForSeconds(RateDamping);
            }
            _flickering = false;
        }
    }
}