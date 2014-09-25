using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;
using Jaxxa_Shields.Pawns.Nano;

namespace Jaxxa_Shields
{
    public class Building_Pawn_Upgrader : Building
    {
        #region Variables

        public float MAX_DISTANCE = 2.0f;
        public bool flag_charge = false;
        CompPowerTrader power;
        //NanoConnector nanoConnector;

        private static Texture2D UI_UPGRADE;
        private static Texture2D UI_CHARGE_OFF;
        private static Texture2D UI_CHARGE_ON;

        #endregion

        //Dummy override
        public override void PostMake()
        {

            UI_UPGRADE = ContentFinder<Texture2D>.Get("UI/Upgrade", true);
            UI_CHARGE_OFF = ContentFinder<Texture2D>.Get("UI/ChargeOFF", true);
            UI_CHARGE_ON = ContentFinder<Texture2D>.Get("UI/ChargeON", true);

            base.PostMake();
        }
        //On spawn, get the power component reference
        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.power = base.GetComp<CompPowerTrader>();
            //this.nanoConnector = new Jaxxa_Shields.Pawns.Nano.NanoConnector();
        }

        public override void Tick()
        {
            base.Tick();

            if (this.power.PowerOn == true)
            {
                NanoManager.tick();
                this.rechargePawns();
            }
        }


        public override void Draw()
        {
            base.Draw();
        }

        public override void DrawExtraSelectionOverlays()
        {
            //GenDraw.DrawRadiusRing(base.Position, shieldField.shieldShieldRadius);
        }
        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(base.GetInspectString());
            ///stringBuilder.Append(shieldField.GetInspectString());

            /*
            for (int i = 0, l = sparksParticle.Length; i < l; i++)
            {
                stringBuilder.AppendLine("   " + (i + 1) + ". " + sparksParticle[i].currentDir + " -> " + sparksParticle[i].currentStep);
            }*/

            string text;

            text = "Nano Charge: " + NanoManager.getCurrentCharge() + " / " + NanoManager.getMaxCharge();
            stringBuilder.AppendLine(text);

            if (power != null)
            {
                text = power.CompInspectStringExtra();
                if (!text.NullOrEmpty())
                {
                    stringBuilder.AppendLine(text);
                }
            }


            return stringBuilder.ToString();
        }
        //Saving game
        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.LookValue(ref flag_charge, "flag_charge");
            Scribe_Values.LookValue(ref NanoManager.currentCharge, "currentCharge");
            /*
            Scribe_Deep.LookDeep(ref shieldField, "shieldField");

            shieldField.position = base.Position;

            Scribe_Values.LookValue(ref flag_direct, "flag_direct");
            Scribe_Values.LookValue(ref flag_indirect, "flag_indirect");
            Scribe_Values.LookValue(ref flag_fireSupression, "flag_fireSupression");
            Scribe_Values.LookValue(ref flag_shieldRepairMode, "flag_shieldRepairMode");*/
        }

        public override IEnumerable<Command> GetCommands()
        {
            IList<Command> CommandList = new List<Command>();
            IEnumerable<Command> baseCommands = base.GetCommands();

            if (baseCommands != null)
            {
                CommandList = baseCommands.ToList();
            }

            if (true)
            {
                //Upgrading
                Command_Action command_Action_InstallShield = new Command_Action();

                command_Action_InstallShield.defaultLabel = "Upgrade To NanoShield";

                command_Action_InstallShield.icon = UI_UPGRADE;
                command_Action_InstallShield.defaultDesc = "Upgrade To NanoShield";

                command_Action_InstallShield.activateSound = SoundDef.Named("Click");
                command_Action_InstallShield.action = new Action(this.tryReplacePawn);

                CommandList.Add(command_Action_InstallShield);
            }

            //Charging
            if (true)
            {
                //switchDirect
                Command_Action command_Action_switchCharge = new Command_Action();

                command_Action_switchCharge.defaultLabel = "Charge Shields";
                if (flag_charge)
                {
                    command_Action_switchCharge.icon = UI_CHARGE_ON;
                    command_Action_switchCharge.defaultDesc = "On";

                }
                else
                {
                    command_Action_switchCharge.icon = UI_CHARGE_OFF;
                    command_Action_switchCharge.defaultDesc = "Off";
                }

                command_Action_switchCharge.activateSound = SoundDef.Named("Click");
                command_Action_switchCharge.action = new Action(this.SwitchCharge);

                CommandList.Add(command_Action_switchCharge);
            }


            return CommandList.AsEnumerable<Command>();
        }

        private void SwitchCharge()
        {
            flag_charge = !flag_charge;
        }

        private void tryReplacePawn()
        {
            if (this.power.PowerOn == true)
            {
                this.replacePawns();
            }
        }

        private bool replacePawns()
        {
            IEnumerable<Pawn> pawns = Find.ListerPawns.ColonistsAndPrisoners;

            if (pawns != null)
            {
                IEnumerable<Pawn> closePawns = pawns.Where<Pawn>(t => t.Position.WithinHorizontalDistanceOf(this.Position, this.MAX_DISTANCE));

                if (closePawns != null)
                {
                    //List<Thing> fireTo
                    foreach (Pawn currentPawn in closePawns.ToList())
                    {
                        if (currentPawn.GetType() == typeof(Pawn))
                        {
                            if (NanoManager.requestCharge(100))
                            {
                                IntVec3 pawnPosition = currentPawn.Position;

                                ShieldedPawn newPawn = Jaxxa_Shields.ShieldedPawnGenerator.GeneratePawn("PawnKindDef_ShieldedPawn", Faction.OfColony, currentPawn);

                                Log.Message("Despawn");
                                currentPawn.Destroy();

                                GenSpawn.Spawn(newPawn, pawnPosition);
                                return true;
                            }
                        }
                        else if (currentPawn.GetType() == typeof(ShieldedPawn))
                        {
                            ShieldedPawn currentShieldedPawn = (ShieldedPawn)currentPawn;

                            if (currentShieldedPawn.currentShields < currentShieldedPawn.max_shields)
                            {
                                if (NanoManager.requestCharge(1))
                                {
                                    currentShieldedPawn.currentShields += 1;
                                }
                            }
                        }
                        else
                        {
                            Log.Error("Unknown Pawn Type");
                        }
                    }
                }
            }

            return false;
        }

        public void rechargePawns()
        {
            int currentTick = Find.TickManager.tickCount;
            //Only every 10 ticks
            if (currentTick % 10 == 0)
            {
                IEnumerable<Pawn> pawns = Find.ListerPawns.ColonistsAndPrisoners;

                if (pawns != null)
                {
                    IEnumerable<Pawn> closePawns = pawns.Where<Pawn>(t => t.Position.WithinHorizontalDistanceOf(this.Position, this.MAX_DISTANCE));

                    if (closePawns != null)
                    {
                        //List<Thing> fireTo
                        foreach (Pawn currentPawn in closePawns.ToList())
                        {
                            if (currentPawn.GetType() == typeof(ShieldedPawn))
                            {
                                ShieldedPawn currentShieldedPawn = (ShieldedPawn)currentPawn;

                                if (currentShieldedPawn.getRequiresRecharge())
                                {
                                    int chargeAmmount = 1;

                                    if (NanoManager.requestCharge(chargeAmmount))
                                    {
                                        currentShieldedPawn.recharge(chargeAmmount);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}