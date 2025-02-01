using BepInEx.Configuration;
using EntityStates;
using Grumpy.UnitTest;
using RoR2;
using AdamMod.Content;
using AdamMod.Modules;
using UnityEngine;
using UnityEngine.Networking;

namespace AdamMod.SkillStates
{
    public class MainState : GenericCharacterMain
    {
        private Animator animator;

        public LocalUser localUser;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.modelAnimator;
            this.FindLocalUser();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (this.animator)
            {
                bool inCombat = false;
                if (!base.characterBody.outOfDanger || !base.characterBody.outOfCombat)
                {
                    inCombat = true;
                }
                this.animator.SetBool("inCombat", inCombat);
                if (base.isGrounded)
                {
                    this.animator.SetFloat("airBlend", 0f);
                }
                else
                {
                    this.animator.SetFloat("airBlend", 1f);
                }

                this.animator.SetFloat("aimDir", base.inputBank.aimDirection.y);
            }
        }

        public override void Update()
        {
            base.Update();
            if (base.isAuthority && base.characterMotor.isGrounded)
            {
                //this.CheckEmote<Rest>(Config.restKey);
                //this.CheckEmote<Taunt>(Config.tauntKey);
                //this.CheckEmote<Dance>(Config.danceKey);
            }
        }

        private bool HasItem(ItemDef itemDef)
        {
            if ((bool)base.characterBody && (bool)base.characterBody.inventory)
            {
                return base.characterBody.inventory.GetItemCount(itemDef) > 0;
            }
            return false;
        }

        private void CheckEmote(KeyCode keybind, EntityState state)
        {
            if (Input.GetKeyDown(keybind) && !this.localUser.isUIFocused)
            {
                base.outer.SetInterruptState(state, InterruptPriority.Any);
            }
        }

        private void CheckEmote<T>(ConfigEntry<KeyboardShortcut> keybind) where T : EntityState, new()
        {
            if (Modules.Config.GetKeyPressed(keybind.Value))
            {
                this.FindLocalUser();
                if (this.localUser != null && !this.localUser.isUIFocused)
                {
                    base.outer.SetInterruptState(new T(), InterruptPriority.Any);
                }
            }
        }

        private void FindLocalUser()
        {
            if (this.localUser != null || !base.characterBody)
            {
                return;
            }
            foreach (LocalUser readOnlyLocalUsers in LocalUserManager.readOnlyLocalUsersList)
            {
                if (readOnlyLocalUsers.cachedBody == base.characterBody)
                {
                    this.localUser = readOnlyLocalUsers;
                    break;
                }
            }
        }

        public override void ProcessJump()
        {
            if (!base.hasCharacterMotor)
            {
                return;
            }
            bool flag = false;
            bool flag2 = false;
            if (!base.jumpInputReceived || !base.characterBody || base.characterMotor.jumpCount >= base.characterBody.maxJumpCount)
            {
                return;
            }
            Util.PlaySound("sfx_belmont_jump", base.gameObject);
            int itemCount = base.characterBody.inventory.GetItemCount(RoR2Content.Items.JumpBoost);
            float horizontalBonus = 1f;
            float verticalBonus = 1f;
            if (base.characterMotor.jumpCount >= base.characterBody.baseJumpCount)
            {
                flag = true;
                horizontalBonus = 1.5f;
                verticalBonus = 1.5f;
            }
            else if (itemCount > 0 && base.characterBody.isSprinting)
            {
                float num = base.characterBody.acceleration * base.characterMotor.airControl;
                if (base.characterBody.moveSpeed > 0f && num > 0f)
                {
                    flag2 = true;
                    float num2 = Mathf.Sqrt(10f * (float)itemCount / num);
                    float num3 = base.characterBody.moveSpeed / num;
                    horizontalBonus = (num2 + num3) / num3;
                }
            }
            GenericCharacterMain.ApplyJumpVelocity(base.characterMotor, base.characterBody, horizontalBonus, verticalBonus);
            if (base.hasModelAnimator)
            {
                int layerIndex = base.modelAnimator.GetLayerIndex("Body");
                if (layerIndex >= 0)
                {
                    if (base.characterBody.isSprinting && flag2)
                    {
                        base.modelAnimator.CrossFadeInFixedTime("SprintJump", base.smoothingParameters.intoJumpTransitionTime, layerIndex);
                    }
                    else
                    {
                        base.modelAnimator.CrossFadeInFixedTime("Jump", base.smoothingParameters.intoJumpTransitionTime, layerIndex);
                    }
                }
            }
            if (flag)
            {
                EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/FeatherEffect"), new EffectData
                {
                    origin = base.characterBody.footPosition
                }, transmit: true);
            }
            else if (base.characterMotor.jumpCount > 0)
            {
                EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CharacterLandImpact"), new EffectData
                {
                    origin = base.characterBody.footPosition,
                    scale = base.characterBody.radius
                }, transmit: true);
            }
            if (flag2)
            {
                EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BoostJumpEffect"), new EffectData
                {
                    origin = base.characterBody.footPosition,
                    rotation = Util.QuaternionSafeLookRotation(base.characterMotor.velocity)
                }, transmit: true);
            }
            base.characterMotor.jumpCount++;
            if ((bool)this.animator)
            {
                float num4 = ((BaseCharacterMain)this).animatorWalkParamCalculator.animatorWalkSpeed.y;
                float num5 = ((BaseCharacterMain)this).animatorWalkParamCalculator.animatorWalkSpeed.x;
                if ((Mathf.Abs(num4) <= 0.45f && Mathf.Abs(num5) <= 0.45f) || base.inputBank.moveVector == Vector3.zero)
                {
                    num4 = 0f;
                    num5 = 0f;
                }
                if (Mathf.Abs(num4) > Mathf.Abs(num5))
                {
                    num4 = ((!(num4 > 0f)) ? (-1f) : 1f);
                    num5 = 0f;
                }
                else if (Mathf.Abs(num4) < Mathf.Abs(num5))
                {
                    num5 = ((!(num5 > 0f)) ? (-1f) : 1f);
                    num4 = 0f;
                }
                this.animator.SetFloat("forwardSpeedCached", num5);
                this.animator.SetFloat("rightSpeedCached", num4);
            }
        }
    }
}
