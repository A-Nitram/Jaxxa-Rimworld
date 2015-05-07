using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;

namespace Enhanced_Defence.Vehicles
{
    class VehiclePawnGenerator
    {
        public static VehiclePawn GeneratePawn(string kindDefName, Faction faction)
        {
            return VehiclePawnGenerator.GeneratePawn(PawnKindDef.Named(kindDefName), faction);
        }

        public static VehiclePawn GeneratePawn(PawnKindDef kindDef, Faction faction)
        {
            VehiclePawn pawn = (VehiclePawn)ThingMaker.MakeThing(kindDef.race, (ThingDef)null);

            if (kindDef.race.race.Humanlike && faction == null)
            {
                faction = FactionUtility.DefaultFactionFrom(kindDef.defaultFactionType);
                object[] objArray = new object[4];
                int index1 = 0;
                string str1 = "Tried to generate pawn of Humanlike race ";
                objArray[index1] = (object)str1;
                int index2 = 1;
                PawnKindDef pawnKindDef = kindDef;
                objArray[index2] = (object)pawnKindDef;
                int index3 = 2;
                string str2 = " with null faction. Setting to ";
                objArray[index3] = (object)str2;
                int index4 = 3;
                Faction faction1 = faction;
                objArray[index4] = (object)faction1;
                Log.Error(string.Concat(objArray));
            }
            //Pawn pawn = (Pawn)ThingMaker.MakeThing(kindDef.race, (ThingDef)null);
            pawn.kindDef = kindDef;
            pawn.SetFactionDirect(faction);
            pawn.pather = new Pawn_PathFollower(pawn);
            pawn.ageTracker = new Pawn_AgeTracker(pawn);
            pawn.health = new Pawn_HealthTracker(pawn);
            pawn.jobs = new Pawn_JobTracker(pawn);
            pawn.mindState = new Pawn_MindState(pawn);
            pawn.filth = new Pawn_FilthTracker(pawn);
            pawn.needs = new Pawn_NeedsTracker(pawn);

            //if (pawn.RaceProps.ToolUser)
            //{
            Log.Message("ToolUser");
            pawn.equipment = new Pawn_EquipmentTracker(pawn);
            pawn.carryHands = new Pawn_CarryHands(pawn);
            pawn.apparel = new Pawn_ApparelTracker(pawn);
            pawn.inventory = new Pawn_InventoryTracker(pawn);
            //}

            //if (pawn.RaceProps.Humanlike)
            //{
            Log.Message("Humanlike");
            pawn.ownership = new Pawn_Ownership(pawn);
            pawn.skills = new Pawn_SkillTracker(pawn);
            pawn.talker = new Pawn_TalkTracker(pawn);
            pawn.story = new Pawn_StoryTracker(pawn);
            pawn.workSettings = new Pawn_WorkSettings(pawn);
            //}

            if (pawn.RaceProps.intelligence <= Intelligence.ToolUser)
            {
                pawn.caller = new Pawn_CallTracker(pawn);
            }
            PawnUtility.AddAndRemoveComponentsAsAppropriate(pawn);
            pawn.gender = !pawn.RaceProps.hasGenders ? Gender.None : ((double)Rand.Value >= 0.5 ? Gender.Female : Gender.Male);
            //PawnGenerator.GenerateRandomAge(pawn);

            //PawnGenerator.GenerateInitialHediffs(pawn);
            pawn.health.hediffSet.Clear();

            if (pawn.RaceProps.Humanlike)
            {
                pawn.story.skinColor = PawnSkinColors.RandomSkinColor();
                pawn.story.crownType = (double)Rand.Value >= 0.5 ? CrownType.Narrow : CrownType.Average;
                pawn.story.headGraphicPath = GraphicDatabaseHeadRecords.GetHeadRandom(pawn.gender, pawn.story.skinColor, pawn.story.crownType).GraphicPath;
                pawn.story.hairColor = PawnHairColors.RandomHairColor(pawn.story.skinColor, pawn.ageTracker.AgeBiologicalYears);
                PawnBioGenerator.GiveAppropriateBioTo(pawn, faction.def);
                pawn.story.hairDef = PawnHairChooser.RandomHairDefFor(pawn, faction.def);
                //PawnGenerator.GiveRandomTraitsTo(pawn);
                pawn.story.GenerateSkillsFromBackstory();
            }
            PawnApparelGenerator.GenerateStartingApparelFor(pawn);
            PawnWeaponGenerator.TryGenerateWeaponFor(pawn);
            PawnInventoryGenerator.GenerateInventoryFor(pawn);
            PawnUtility.AddAndRemoveComponentsAsAppropriate(pawn);

            return (VehiclePawn)pawn;
        }
    }
}
