using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace Jaxxa_Shields
{
    public class Building_Pawn_Upgrader : Building
    {
        #region Variables

        public float MAX_DISTANCE = 5.0f;

        CompPowerTrader power;

        #endregion

        //Dummy override
        public override void PostMake()
        {
            base.PostMake();
        }
        //On spawn, get the power component reference
        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.power = base.GetComp<CompPowerTrader>();
        }

        public override void Tick()
        {
            base.Tick();

            if (this.power.PowerOn == true)
            {
                if (this.replacePAwns())
                {
                    this.power.DesirePowerOn = false;
                    this.power.PowerOn = false;
                }
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

            if (power != null)
            {
                string text = power.CompInspectString();
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

            return CommandList.AsEnumerable<Command>();

        }
         
        private bool replacePAwns()
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

                        //PawnKindDef temp = currentPawn.kindDef;
                        //ThingDef tempDef = currentPawn.def;
                        //Pawn_StoryTracker tempStory = currentPawn.story;


                        //Jaxxa_Shields.Pawns.ShieldedPawn newPawn = Jaxxa_Shields.Pawns.ShieldedPawnGenerator.GeneratePawn(temp, Faction.OfColony);
                        ShieldedPawn newPawn = Jaxxa_Shields.ShieldedPawnGenerator.GeneratePawn("PawnKindDef_ShieldedPawn", Faction.OfColony, currentPawn);

                        Log.Message("Despawn");
                        currentPawn.Destroy();

                        //newPawn.def = tempDef;
                        //newPawn.story = tempStory;
                        
                        GenSpawn.Spawn(newPawn, this.Position);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}