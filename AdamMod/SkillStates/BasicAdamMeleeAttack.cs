using EntityStates;
using R2API;
using RoR2;
using AdamMod.Content;
using UnityEngine;

namespace AdamMod.SkillStates
{
    public class BasicAdamMeleeAttack : BasicMeleeAttack
    {

        public override void OnEnter()
        {
            this.baseDuration = 1f;
            this.duration = 1f;

            base.OnEnter();

            RefreshState();

            /*
            Standard melee attack flow
            ** -> empty method/does nothing without override


            OnEnter
                CalcDuration
                GetHitboxGroupName **
                new OverlapAttack
                PlayAnimation **

            FixedUpdate
                BeginMeleeAttackEffect
                Authority - FixedUpdate
                    AuthFireAttack
                        AuthModifyOverlapAttack **
                        AuthTriggerHitPause
                        OnMeleeHitAuth **
                    AuthExitHitPause
                    AuthOnFinish **
                
            OnExit
                BeginMeleeAttackEffect
                AuthFireAttack
                AuthExitHitPause
                
             */
        }

        public void RefreshState()
        {
        }

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
        }

        public override void OnMeleeHitAuthority()
        {
            base.OnMeleeHitAuthority();
        }
    }
}
