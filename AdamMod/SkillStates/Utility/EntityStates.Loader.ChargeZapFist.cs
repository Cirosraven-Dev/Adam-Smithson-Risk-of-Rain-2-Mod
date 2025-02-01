// RoR2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// EntityStates.Loader.ChargeZapFist
using EntityStates;
using EntityStates.Loader;

public class ChargeZapFist : BaseChargeFist
{
	protected override bool ShouldKeepChargingAuthority()
	{
		return base.fixedAge < base.chargeDuration;
	}

	protected override EntityState GetNextStateAuthority()
	{
		return new SwingZapFist
		{
			charge = base.charge
		};
	}
}
