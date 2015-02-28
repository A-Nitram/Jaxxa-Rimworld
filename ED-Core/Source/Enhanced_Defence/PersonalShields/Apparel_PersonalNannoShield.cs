/*
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
    public class Apparel_PersonalNannoShield : Apparel
  {
    private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
    private static readonly SoundDef SoundAbsorbDamage = SoundDef.Named("PersonalShieldAbsorbDamage");
    private static readonly SoundDef SoundBreak = SoundDef.Named("PersonalShieldBroken");
    private static readonly SoundDef SoundReset = SoundDef.Named("PersonalShieldReset");
    private int ticksToReset = -1;
    private int lastAbsorbDamageTick = -9999;
    private int lastKeepDisplayTick = -9999;
    private int StartingTicksToReset = 1200;
    private float EnergyOnReset = 0.25f;
    private float EnergyLossPerDamage = 0.027f;
    private int KeepDisplayingTicks = 1000;
    private const float MinDrawSize = 1.2f;
    private const float MaxDrawSize = 1.55f;
    private const float MaxDamagedJitterDist = 0.05f;
    private const int JitterDurationTicks = 8;
    private float energy;
    private Vector3 impactAngleVect;

    private float EnergyMax
    {
      get
      {
        return StatExtension.GetStatValue((Thing) this, StatDefOf.PersonalShieldEnergyMax, true);
      }
    }

    private float EnergyGainPerTick
    {
      get
      {
        return StatExtension.GetStatValue((Thing) this, StatDefOf.PersonalShieldRechargeRate, true) / 60f;
      }
    }

    public float Energy
    {
      get
      {
        return this.energy;
      }
    }

    public ShieldState ShieldState
    {
      get
      {
        return this.ticksToReset > 0 ? ShieldState.Resetting : ShieldState.Active;
      }
    }

    private bool ShouldDisplay
    {
      get
      {
        return !this.wearer.Dead && !this.wearer.Downed && (this.wearer.playerController != null && this.wearer.playerController.Drafted || (FactionUtility.HostileTo(this.wearer.Faction, Faction.OfColony) || Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks));
      }
    }

    public override void ExposeData()
    {
      base.ExposeData();
      Scribe_Values.LookValue<float>(ref this.energy, "energy", 0.0f, false);
      Scribe_Values.LookValue<int>(ref this.ticksToReset, "ticksToReset", -1, false);
      Scribe_Values.LookValue<int>(ref this.lastKeepDisplayTick, "lastKeepDisplayTick", 0, false);
    }


    public override void Tick()
    {
      base.Tick();
      if (this.wearer == null)
        this.energy = 0.0f;
      else if (this.ShieldState == ShieldState.Resetting)
      {
        --this.ticksToReset;
        if (this.ticksToReset > 0)
          return;
        this.Reset();
      }
      else
      {
        if (this.ShieldState != ShieldState.Active)
          return;
        this.energy += this.EnergyGainPerTick;
        if ((double) this.energy <= (double) this.EnergyMax)
          return;
        this.energy = this.EnergyMax;
      }
    }

    public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
    {
      if (this.ShieldState != ShieldState.Active || (dinfo.Instigator == null || GenAdj.AdjacentTo8Way(dinfo.Instigator.Position, this.wearer.Position)) && !dinfo.Def.isExplosive)
        return false;
      this.energy -= (float) dinfo.Amount * this.EnergyLossPerDamage;
      if (dinfo.Def == DamageDefOf.EMP)
        this.energy = -1f;
      if ((double) this.energy < 0.0)
        this.Break();
      else
        this.AbsorbedDamage(dinfo);
      return true;
    }

    public void KeepDisplaying()
    {
      this.lastKeepDisplayTick = Find.TickManager.TicksGame;
    }

    private void AbsorbedDamage(DamageInfo dinfo)
    {
      SoundStarter.PlayOneShot(PersonalShield.SoundAbsorbDamage, (SoundInfo) this.wearer.Position);
      this.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
      Vector3 loc = Gen.TrueCenter((Thing) this.wearer) + Gen.RotatedBy(this.impactAngleVect, 180f) * 0.5f;
      float scale = Mathf.Min(10f, (float) (2.0 + (double) dinfo.Amount / 10.0));
      MoteThrower.ThrowStatic(loc, ThingDefOf.Mote_ExplosionFlash, scale);
      int num = (int) scale;
      for (int index = 0; index < num; ++index)
        MoteThrower.ThrowDustPuff(loc, Rand.Range(0.8f, 1.2f));
      this.lastAbsorbDamageTick = Find.TickManager.TicksGame;
      this.KeepDisplaying();
    }

    private void Break()
    {
      SoundStarter.PlayOneShot(PersonalShield.SoundBreak, (SoundInfo) this.wearer.Position);
      MoteThrower.ThrowStatic(Gen.TrueCenter((Thing) this.wearer), ThingDefOf.Mote_ExplosionFlash, 12f);
      for (int index = 0; index < 6; ++index)
        MoteThrower.ThrowDustPuff(Gen.TrueCenter((Thing) this.wearer) + Vector3Utility.HorizontalVectorFromAngle((float) Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), Rand.Range(0.8f, 1.2f));
      this.energy = 0.0f;
      this.ticksToReset = this.StartingTicksToReset;
    }

    private void Reset()
    {
      SoundStarter.PlayOneShot(PersonalShield.SoundReset, (SoundInfo) this.wearer.Position);
      MoteThrower.ThrowLightningGlow(Gen.TrueCenter((Thing) this.wearer), 3f);
      this.ticksToReset = -1;
      this.energy = this.EnergyOnReset;
    }

    public override void DrawWornExtras()
    {
      if (this.ShieldState != ShieldState.Active || !this.ShouldDisplay)
        return;
      float num1 = Mathf.Lerp(1.2f, 1.55f, this.energy);
      Vector3 tweenedPos = this.wearer.drawer.tweener.TweenedPos;
      tweenedPos.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
      int num2 = Find.TickManager.TicksGame - this.lastAbsorbDamageTick;
      if (num2 < 8)
      {
        float num3 = (float) ((double) (8 - num2) / 8.0 * 0.0500000007450581);
        tweenedPos += this.impactAngleVect * num3;
        num1 -= num3;
      }
      float angle = (float) Rand.Range(0, 360);
      Vector3 s = new Vector3(num1, 1f, num1);
      Matrix4x4 matrix = new Matrix4x4();
      matrix.SetTRS(tweenedPos, Quaternion.AngleAxis(angle, Vector3.up), s);
      Graphics.DrawMesh(MeshPool.plane10, matrix, PersonalShield.BubbleMat, 0);
    }

    public override bool AllowVerbCast(IntVec3 root, TargetInfo targ)
    {
      if (targ.HasThing && targ.Thing.def.size != IntVec2.one)
        return GenAdj.AdjacentTo8WayOrInside(root, targ.Thing);
      else
        return GenAdj.AdjacentTo8Way(root, targ.Cell);
    }
  }
}
*/