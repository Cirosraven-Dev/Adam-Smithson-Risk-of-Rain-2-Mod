// RoR2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// EntityStates.Loader.BigPunch
using System.Collections.Generic;
using System.Linq;
using EntityStates.Loader;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using UnityEngine;

public class BigPunch : LoaderMeleeAttack
{
	public static int maxShockCount;

	public static float maxShockFOV;

	public static float maxShockDistance;

	public static float shockDamageCoefficient;

	public static float shockProcCoefficient;

	public static float knockbackForce;

	public static float shorthopVelocityOnEnter;

	private bool hasHit;

	private bool hasKnockbackedSelf;

	private static int BigPunchStateHash = Animator.StringToHash("BigPunch");

	private static int BigPunchParamHash = Animator.StringToHash("BigPunch.playbackRate");

	private Vector3 punchVector => base.characterDirection.forward.normalized;

	public override void OnEnter()
	{
		base.OnEnter();
		base.characterMotor.velocity.y = BigPunch.shorthopVelocityOnEnter;
	}

	protected override void PlayAnimation()
	{
		base.PlayAnimation();
		base.PlayAnimation("FullBody, Override", BigPunch.BigPunchStateHash, BigPunch.BigPunchParamHash, base.duration);
	}

	protected override void AuthorityFixedUpdate()
	{
		base.AuthorityFixedUpdate();
		if (this.hasHit && !this.hasKnockbackedSelf && !base.authorityInHitPause)
		{
			this.hasKnockbackedSelf = true;
			base.healthComponent.TakeDamageForce(this.punchVector * (0f - BigPunch.knockbackForce), alwaysApply: true);
		}
	}

	protected override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
	{
		base.AuthorityModifyOverlapAttack(overlapAttack);
		overlapAttack.maximumOverlapTargets = 1;
	}

	protected override void OnMeleeHitAuthority()
	{
		if (!this.hasHit)
		{
			base.OnMeleeHitAuthority();
			this.hasHit = true;
			if ((bool)base.FindModelChild(base.swingEffectMuzzleString))
			{
				FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
				fireProjectileInfo.position = base.GetAimRay().origin;
				fireProjectileInfo.rotation = Quaternion.LookRotation(this.punchVector);
				fireProjectileInfo.crit = base.RollCrit();
				fireProjectileInfo.damage = 1f * base.damageStat;
				fireProjectileInfo.owner = base.gameObject;
				fireProjectileInfo.projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LoaderZapCone");
				ProjectileManager.instance.FireProjectile(fireProjectileInfo);
			}
		}
	}

	private void FireSecondaryRaysServer()
	{
		Ray aimRay = base.GetAimRay();
		TeamIndex team = base.GetTeam();
		BullseyeSearch bullseyeSearch = new BullseyeSearch();
		bullseyeSearch.teamMaskFilter = TeamMask.GetEnemyTeams(team);
		bullseyeSearch.maxAngleFilter = BigPunch.maxShockFOV * 0.5f;
		bullseyeSearch.maxDistanceFilter = BigPunch.maxShockDistance;
		bullseyeSearch.searchOrigin = aimRay.origin;
		bullseyeSearch.searchDirection = this.punchVector;
		bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
		bullseyeSearch.filterByLoS = false;
		bullseyeSearch.RefreshCandidates();
		List<HurtBox> list = bullseyeSearch.GetResults().Where(Util.IsValid).ToList();
		Transform transform = base.FindModelChild(base.swingEffectMuzzleString);
		if (!transform)
		{
			return;
		}
		for (int i = 0; i < Mathf.Min(list.Count, BigPunch.maxShockCount); i++)
		{
			HurtBox hurtBox = list[i];
			if ((bool)hurtBox)
			{
				LightningOrb lightningOrb = new LightningOrb();
				lightningOrb.bouncedObjects = new List<HealthComponent>();
				lightningOrb.attacker = base.gameObject;
				lightningOrb.teamIndex = team;
				lightningOrb.damageValue = base.damageStat * BigPunch.shockDamageCoefficient;
				lightningOrb.isCrit = base.RollCrit();
				lightningOrb.origin = transform.position;
				lightningOrb.bouncesRemaining = 0;
				lightningOrb.lightningType = LightningOrb.LightningType.Loader;
				lightningOrb.procCoefficient = BigPunch.shockProcCoefficient;
				lightningOrb.target = hurtBox;
				OrbManager.instance.AddOrb(lightningOrb);
			}
		}
	}
}
