using RoR2;
using UnityEngine;
using R2API;
using System.Collections;
using AdamMod.Content;

namespace AdamMod.Components
{
    public class LurkingDread : MonoBehaviour
    {
        private CharacterBody body;

        private void Awake()
        {
            this.body = this.transform.parent.GetComponent<CharacterBody>();
        }

        private void OnTriggerEnter()
        {
            if (body)
                body.AddBuff(Buffs.Darkness);
        }

        private void OnTriggerExit()
        {
            if (body && body.HasBuff(Buffs.Darkness))
                body.RemoveBuff(Buffs.Darkness);
        }
    }
}
