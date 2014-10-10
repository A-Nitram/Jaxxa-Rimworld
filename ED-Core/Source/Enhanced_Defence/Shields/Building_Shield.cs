using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;
using Enhanced_Defence.ShieldUtils;

namespace Enhanced_Defence.Shields
{
    public class Building_Shield : Building
    {
        #region Variables
        //UI elements
        private static Texture2D UI_DIRECT_ON;
        private static Texture2D UI_DIRECT_OFF;

        private static Texture2D UI_INDIRECT_ON;
        private static Texture2D UI_INDIRECT_OFF;

        private static Texture2D UI_FIRE_ON;
        private static Texture2D UI_FIRE_OFF;

        private static Texture2D UI_REPAIR_ON;
        private static Texture2D UI_REPAIR_OFF;


        private static Texture2D UI_SHOW_ON;
        private static Texture2D UI_SHOW_OFF;
        
        public bool flag_direct = true;
        public bool flag_indirect = true;
        public bool flag_fireSupression = true;
        public bool flag_shieldRepairMode = true;
        public bool flag_showVisually = true;

        //variables that are read in from XML
        public int shieldMaxShieldStrength;
        public int shieldInitialShieldStrength;
        public int shieldShieldRadius;

        public int shieldPowerRequiredLoading;
        public int shieldPowerRequiredCharging;
        public int shieldPowerRequiredSustaining;

        public bool shieldBlockIndirect;
        public bool shieldBlockDirect;
        public bool shieldFireSupression;

        public bool shieldStructuralIntegrityMode;

        public int shieldRechargeTickDelay;
        public int shieldRecoverWarmup;

        public float colourRed;
        public float colourGreen;
        public float colourBlue;


        ShieldField shieldField;
        CompPowerTrader power;
        //Prepared data
        private ShieldBlendingParticle[] sparksParticle = new ShieldBlendingParticle[3];

        #endregion

        //Dummy override
        public override void PostMake()
        {
            base.PostMake();
        }
        //On spawn, get the power component reference
        public override void SpawnSetup()
        {
            //Setup UI
            UI_DIRECT_OFF = ContentFinder<Texture2D>.Get("UI/DirectOff", true);
            UI_DIRECT_ON = ContentFinder<Texture2D>.Get("UI/DirectOn", true);
            UI_INDIRECT_OFF = ContentFinder<Texture2D>.Get("UI/IndirectOff", true);
            UI_INDIRECT_ON = ContentFinder<Texture2D>.Get("UI/IndirectOn", true);
            UI_FIRE_OFF = ContentFinder<Texture2D>.Get("UI/FireOff", true);
            UI_FIRE_ON = ContentFinder<Texture2D>.Get("UI/FireOn", true);
            UI_REPAIR_ON = ContentFinder<Texture2D>.Get("UI/RepairON", true);
            UI_REPAIR_OFF = ContentFinder<Texture2D>.Get("UI/RepairOFF", true);

            UI_SHOW_ON = ContentFinder<Texture2D>.Get("UI/ShieldShowOn", true);
            UI_SHOW_OFF = ContentFinder<Texture2D>.Get("UI/ShieldShowOff", true);


            base.SpawnSetup();
            this.power = base.GetComp<CompPowerTrader>();
            if (def is ShieldThingDef)
            {
                //Read in variables from the custom MyThingDef
                shieldMaxShieldStrength = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldMaxShieldStrength;
                shieldInitialShieldStrength = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldInitialShieldStrength;
                shieldShieldRadius = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldShieldRadius;

                shieldPowerRequiredLoading = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldPowerRequiredLoading;
                shieldPowerRequiredCharging = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldPowerRequiredCharging;
                shieldPowerRequiredSustaining = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldPowerRequiredSustaining;

                shieldRechargeTickDelay = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldRechargeTickDelay;
                shieldRecoverWarmup = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldRecoverWarmup;

                shieldBlockIndirect = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldBlockIndirect;
                shieldBlockDirect = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldBlockDirect;
                shieldFireSupression = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldFireSupression;

                shieldStructuralIntegrityMode = ((Enhanced_Defence.Shields.ShieldThingDef)def).shieldStructuralIntegrityMode;

                colourRed = ((Enhanced_Defence.Shields.ShieldThingDef)def).colourRed;
                colourGreen = ((Enhanced_Defence.Shields.ShieldThingDef)def).colourGreen;
                colourBlue = ((Enhanced_Defence.Shields.ShieldThingDef)def).colourBlue;
            }
            else
            {
                Log.Error("Shield definition not of type \"ShieldThingDef\"");
            }

            if (shieldField == null)
            {
                shieldField = new ShieldField(this, base.Position, shieldMaxShieldStrength, shieldInitialShieldStrength, shieldShieldRadius, shieldRechargeTickDelay, shieldRecoverWarmup, shieldBlockIndirect, shieldBlockDirect, shieldFireSupression, shieldStructuralIntegrityMode, colourRed, colourGreen, colourBlue);
            }

        }


        public override void Tick()
        {
            base.Tick();
            //Carefully check to prevent NullPointerExceptions
            if (shieldField != null)
            {
                //Disable shield when power goes off
                if (this.power != null)
                {
                    if (!this.power.PowerOn)
                    {
                        shieldField.enabled = false;
                    }
                    else
                    {
                        shieldField.enabled = true;
                    }
                }
                //Do tick for the shield field
                shieldField.ShieldTick(this.flag_direct, this.flag_indirect, this.flag_fireSupression, this.flag_shieldRepairMode);
                //Change power requirements depending on shield status
                switch (shieldField.status)
                {
                    //Disabled shield also requires power (to avoid flickering when thing increases power requirements because it gained power...)
                    case ShieldStatus.Disabled:
                    case ShieldStatus.Loading:
                        {
                            //this.power.powerOutput = -1000;
                            this.power.powerOutput = shieldPowerRequiredLoading;
                            break;
                        }
                    case ShieldStatus.Charging:
                        {
                            //this.power.powerOutput = -2750;
                            this.power.powerOutput = shieldPowerRequiredCharging;
                            break;
                        }
                    case ShieldStatus.Sustaining:
                        {
                            //this.power.powerOutput = -1820;
                            this.power.powerOutput = shieldPowerRequiredSustaining;
                            break;
                        }
                }
            }
            else
            {
                //Log.Message("shieldField is null in tick event, creating new one.");
                shieldField = new ShieldField(this, base.Position, shieldMaxShieldStrength, shieldInitialShieldStrength, shieldShieldRadius, shieldRechargeTickDelay, shieldRecoverWarmup, shieldBlockIndirect, shieldBlockDirect, shieldFireSupression, shieldStructuralIntegrityMode, colourRed, colourGreen, colourBlue);
            }

        }

        /// <summary>
        /// Draw the shield Field
        /// </summary>
        public void DrawShieldField()
        {
            if (shieldField.isOnline() || shieldField.shieldRecoverWarmup - shieldField.warmupPower < 60)
            {
                //Draw field
                //shield.DrawField(Vectors.IntVecToVec(base.Position) + (new Vector3(0.5f, 0f, 0.5f)));
                shieldField.DrawField(Vectors.IntVecToVec(base.Position));
                //Initialize the spark particle array
                if (sparksParticle[0] == null)
                {
                    for (int i = 0; i < sparksParticle.Length; i++)
                    {
                        sparksParticle[i] = new ShieldBlendingParticle(this.DrawPos, (int)Math.Round(((float)i / (float)(sparksParticle.Length - 1)) * (float)ShieldBlendingParticle.transitionMax));
                    }
                }
                //Animate spark particles
                for (int i = 0, l = sparksParticle.Length; i < l; i++)
                {
                    sparksParticle[i].DrawMe();
                }

                /*Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(this.DrawPos + Altitudes.AltIncVect, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 30f), 0f), UnityEngine.Vector3.one);
                Graphics.DrawMesh(MeshPool.plane20, matrix, Building_Shield.ShieldSparksMat, 0);*/


                //float percentCharged = (float)this.warmup / (float)Building_Turret_Sentinel.warmup_max; ;
                //Fading charge top
                //top.shader = Shader.Find("Diffuse");
                //Graphics.DrawMesh(MeshPool.plane20, matrix, FadedMaterialPool.FadedVersionOf(Building_Turret_Sentinel.TurretTopCharging, percentCharged), 0);
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (flag_showVisually)
            {
                DrawShieldField();
            }
        }
        public override void DrawExtraSelectionOverlays()
        {
            GenDraw.DrawRadiusRing(base.Position, shieldField.shieldShieldRadius);
        }
        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(base.GetInspectString());
            stringBuilder.Append(shieldField.GetInspectString());

            /*
            for (int i = 0, l = sparksParticle.Length; i < l; i++)
            {
                stringBuilder.AppendLine("   " + (i + 1) + ". " + sparksParticle[i].currentDir + " -> " + sparksParticle[i].currentStep);
            }*/

            if (power != null)
            {
                string text = power.CompInspectStringExtra();
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

            Scribe_Deep.LookDeep(ref shieldField, "shieldField");

            shieldField.position = base.Position;

            Scribe_Values.LookValue(ref flag_direct, "flag_direct");
            Scribe_Values.LookValue(ref flag_indirect, "flag_indirect");
            Scribe_Values.LookValue(ref flag_fireSupression, "flag_fireSupression");
            Scribe_Values.LookValue(ref flag_shieldRepairMode, "flag_shieldRepairMode");
        }

        public override IEnumerable<Command> GetCommands()
        {
            IList<Command> CommandList = new List<Command>();
            IEnumerable<Command> baseCommands = base.GetCommands();

            if (baseCommands != null)
            {
                //CommandList = CommandList.Concat<Command>(baseCommands).ToList();
                CommandList = baseCommands.ToList();
                //CommandList.AsEnumerable<Command>().Concat(baseCommands);
            }

            if (shieldBlockDirect)
            {
                //switchDirect
                Command_Action command_Action_switchDirect = new Command_Action();

                command_Action_switchDirect.defaultLabel = "Block Direct";
                if (flag_direct)
                {
                    command_Action_switchDirect.icon = UI_DIRECT_ON;
                    command_Action_switchDirect.defaultDesc = "On";

                }
                else
                {
                    command_Action_switchDirect.icon = UI_DIRECT_OFF;
                    command_Action_switchDirect.defaultDesc = "Off";
                }

                command_Action_switchDirect.activateSound = SoundDef.Named("Click");
                command_Action_switchDirect.action = new Action(this.SwitchDirect);

                CommandList.Add(command_Action_switchDirect);
            }

            if (shieldBlockIndirect)
            {
                //switchIndirect
                Command_Action command_Action_switchIndirect = new Command_Action();
                command_Action_switchIndirect.defaultLabel = "Block Indirect";
                if (flag_indirect)
                {
                    command_Action_switchIndirect.icon = UI_INDIRECT_ON;
                    command_Action_switchIndirect.defaultDesc = "On";

                }
                else
                {
                    command_Action_switchIndirect.icon = UI_INDIRECT_OFF;
                    command_Action_switchIndirect.defaultDesc = "Off";
                }

                command_Action_switchIndirect.activateSound = SoundDef.Named("Click");
                command_Action_switchIndirect.action = new Action(this.SwitchIndirect);

                CommandList.Add(command_Action_switchIndirect);
            }

            if (shieldFireSupression)
            {
                //Switch Fire
                Command_Action command_Action_switchFire = new Command_Action();
                command_Action_switchFire.defaultLabel = "Fire Suppression";
                if (flag_fireSupression)
                {
                    command_Action_switchFire.icon = UI_FIRE_ON;
                    command_Action_switchFire.defaultDesc = "On";

                }
                else
                {
                    command_Action_switchFire.icon = UI_FIRE_OFF;
                    command_Action_switchFire.defaultDesc = "Off";
                }

                command_Action_switchFire.activateSound = SoundDef.Named("Click");
                command_Action_switchFire.action = new Action(this.SwitchFire);

                CommandList.Add(command_Action_switchFire);
            }

            if (shieldStructuralIntegrityMode)
            {
                //Switch Repair
                Command_Action command_Action_switchRepair = new Command_Action();
                command_Action_switchRepair.defaultLabel = "Repair Mode";
                if (flag_shieldRepairMode)
                {
                    command_Action_switchRepair.icon = UI_REPAIR_ON;
                    command_Action_switchRepair.defaultDesc = "On";

                }
                else
                {
                    command_Action_switchRepair.icon = UI_REPAIR_OFF;
                    command_Action_switchRepair.defaultDesc = "Off";
                }

                command_Action_switchRepair.activateSound = SoundDef.Named("Click");
                command_Action_switchRepair.action = new Action(this.SwitchShieldRepairMode);

                CommandList.Add(command_Action_switchRepair);
            }

            if (true)
            {
                //Switch Repair
                Command_Action command_Action_switchShow = new Command_Action();
                command_Action_switchShow.defaultLabel = "Show Visually";
                if (flag_showVisually)
                {
                    command_Action_switchShow.icon = UI_SHOW_ON;
                    command_Action_switchShow.defaultDesc = "Show";

                }
                else
                {
                    command_Action_switchShow.icon = UI_SHOW_OFF;
                    command_Action_switchShow.defaultDesc = "Hide";
                }

                command_Action_switchShow.activateSound = SoundDef.Named("Click");
                command_Action_switchShow.action = new Action(this.SwitchVisual);

                CommandList.Add(command_Action_switchShow);
            }


            return CommandList.AsEnumerable<Command>();
        }

        private void SwitchDirect()
        {
            flag_direct = !flag_direct;
        }

        private void SwitchIndirect()
        {
            flag_indirect = !flag_indirect;
        }

        private void SwitchFire()
        {
            flag_fireSupression = !flag_fireSupression;
        }

        private void SwitchVisual()
        {
            flag_showVisually = !flag_showVisually;
        }

        private void SwitchShieldRepairMode()
        {
            flag_shieldRepairMode = !flag_shieldRepairMode;
        }

    }
}