using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Enhanced_Defence.Vehicles
{
    public class VehiclePawn : Pawn
    {

        List<Pawn> listOfCrewPawns = new List<Pawn>();
        private static Texture2D UI_ADD_COLONIST;

        public VehiclePawn()
        {
            this.verbTracker = new VerbTracker((VerbOwner)this);
            this.drawer = new Pawn_DrawTracker(this);
            this.stances = new Pawn_StanceTracker(this);
            this.natives = new Pawn_NativeVerbs(this);
            this.meleeVerbs = new Pawn_MeleeVerbs(this);
            this.thinker = new Pawn_Thinker(this);

            //float statValue = StatExtension.GetStatValue((Thing)this, StatDefOf.MoveSpeed, true);
            //this.stances.FullBodyBusy = true;
        }

        public override void SpawnSetup()
        {

            base.SpawnSetup();
            UI_ADD_COLONIST = ContentFinder<Texture2D>.Get("UI/ADD_COLONIST", true);
        }

        public override void Tick()
        {
            {
                if (DebugSettings.noAnimals && this.RaceProps.Animal)
                {
                    this.Destroy(DestroyMode.Vanish);
                }
                else
                {
                    if (!this.stances.FullBodyBusy)
                    {
                        this.pather.PatherTick();
                    }
                    this.drawer.DrawTrackerTick();
                    this.ageTracker.AgeTick();
                    this.health.HealthTick();
                    this.stances.StanceTrackerTick();
                    this.mindState.MindTick();
                    if (this.equipment != null)
                        this.equipment.EquipmentTrackerTick();
                    if (this.apparel != null)
                        this.apparel.ApparelTrackerTick();
                    if (this.jobs != null)
                        this.jobs.JobTrackerTick();
                    if (this.carryHands != null)
                        this.carryHands.CarryHandsTick();
                    if (this.talker != null)
                        this.talker.TalkTrackerTick();
                    this.needs.NeedsTrackerTick();
                    if (this.caller != null)
                        this.caller.CallTrackerTick();
                    if (this.skills != null)
                        this.skills.SkillsTick();
                    if (this.playerController == null)
                        return;
                    this.playerController.PlayerControllerTick();
                }
            }

            //base.Tick();
            if (this.listOfCrewPawns.Count == 0)
            {
                DamageInfo temp = new DamageInfo(DamageDefOf.Stun, 1, this);
                this.stances.stunner.Notify_DamageApplied(temp, true);
            }
        }



        public override IEnumerable<Gizmo> GetGizmos()
        {
            //Add the stock Gizmoes
            foreach (var g in base.GetGizmos())
            {
                yield return g;
            }

            if (true)
            {
                Command_Action act = new Command_Action();
                //act.action = () => Designator_Deconstruct.DesignateDeconstruct(this);
                act.action = () => this.AddColonist();
                act.icon = UI_ADD_COLONIST;
                act.defaultLabel = "Add Colonist";
                act.defaultDesc = "Add Colonist";
                act.activateSound = SoundDef.Named("Click");
                //act.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
                //act.groupKey = 689736;
                yield return act;
            }

            if (true)
            {
                Command_Action act = new Command_Action();
                //act.action = () => Designator_Deconstruct.DesignateDeconstruct(this);
                act.action = () => this.ExitVehicle();
                act.icon = UI_ADD_COLONIST;
                act.defaultLabel = "Disembark";
                act.defaultDesc = "Disembark";
                act.activateSound = SoundDef.Named("Click");
                //act.hotKey = KeyBindingDefOf.DesignatorDeconstruct;
                //act.groupKey = 689736;
                yield return act;
            }
        }


        public void AddColonist()
        {
            //Log.Message("CLick AddColonist");
            IEnumerable<Pawn> closePawns = Enhanced_Defence.Utilities.Utilities.findPawns(this.Position, 3);

            if (closePawns != null)
            {
                foreach (Pawn currentPawn in closePawns.ToList())
                {
                    if (currentPawn.SpawnedInWorld)
                    {
                        List<Thing> thingList = new List<Thing>();
                        listOfCrewPawns.Add(currentPawn);
                        currentPawn.DeSpawn();
                        //int tempHealth = currentPawn.HitPoints;
                        //currentPawn.Destroy(DestroyMode.Vanish);
                        //currentPawn.HitPoints = tempHealth;
                    }
                }
            }

            // Tell the MapDrawer that here is something thats changed
            Find.MapDrawer.MapChanged(Position, MapChangeType.Things, true, false);

        }

        
        public void ExitVehicle()
        {

            foreach (Pawn currentPawn in listOfCrewPawns)
            {
                //Log.Message("Placing Thing");

                //Setup the New ID for the Thing
                //currentThing.thingIDNumber = -1;
                //Verse.ThingIDMaker.GiveIDTo(currentThing);

                //currentThing.SetFactionDirect(RimWorld.Faction.OfColony);

                GenPlace.TryPlaceThing(currentPawn, this.Position + new IntVec3(0, 0, -2), ThingPlaceMode.Near);
            }
            listOfCrewPawns.Clear();

        }
        


    }

}
