using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace Descending.Scene_Overworld
{
    public class WorldCursor : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect = null;
        
        public void MoveTo(Vector3 position)
        {
            transform.position = position;
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.startLifetime = 1f;
            
            _effect.Emit(emitParams, 20);
        }
    }
}