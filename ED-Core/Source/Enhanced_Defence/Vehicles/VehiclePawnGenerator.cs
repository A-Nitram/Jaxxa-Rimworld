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

            //Log.Message(pawn.Label);
            //Log.Message(pawn.def.label);
            //Log.Message(pawn.def.defName);

            //Log.Message("Type: " + pawn.GetType());

            pawn.kindDef = kindDef;
            pawn.SetFactionDirect(faction);
            pawn.pather = new Pawn_PathFollower(pawn);
            pawn.ageTracker = new Pawn_AgeTracker(pawn);
            pawn.healthTracker = new Pawn_HealthTracker(pawn);
            pawn.jobs = new Pawn_JobTracker(pawn);
            pawn.inventory = new Pawn_InventoryTracker(pawn);
            pawn.filth = new Pawn_FilthTracker(pawn);
            pawn.mindState = new Pawn_MindState(pawn);
            pawn.needs = new Pawn_NeedsTracker(pawn);
            if (pawn.RaceProps.humanoid || pawn.RaceProps.mechanoid)
            {
                pawn.equipment = new Pawn_EquipmentTracker(pawn);
                pawn.carryHands = new Pawn_CarryHands(pawn);
            }
            if (pawn.RaceProps.humanoid)
            {
                pawn.apparel = new Pawn_ApparelTracker(pawn);
                pawn.ownership = new Pawn_Ownership(pawn);
                pawn.skills = new Pawn_SkillTracker(pawn);
                pawn.talker = new Pawn_TalkTracker(pawn);
                pawn.story = new Pawn_StoryTracker(pawn);
                pawn.workSettings = new Pawn_WorkSettings(pawn);
                pawn.guest = new Pawn_GuestTracker(pawn);
                pawn.needs.mood = new Need_Mood(pawn);
                pawn.needs.beauty = new Need_Beauty(pawn);
                pawn.needs.space = new Need_Space(pawn);
            }
            else
                pawn.caller = new Pawn_CallTracker(pawn);
            if (pawn.RaceProps.EatsFood)
                pawn.needs.food = new Need_Food(pawn);
            if (pawn.RaceProps.needsRest)
                pawn.needs.rest = new Need_Rest(pawn);
            pawn.gender = !pawn.RaceProps.hasGenders ? Gender.None : ((double)Rand.Value >= 0.5 ? Gender.Female : Gender.Male);
            //PawnGenerator.GenerateRandomAge(pawn);

            //pawn.ageTracker.SetBirthDate(birthYear, birthDayOfYear);
            pawn.ageTracker.SetBirthDate(GenDate.CurrentYear, GenDate.CurrentYear);

            //if (pawn.RaceProps.humanoid)
            //    AgeInjuryUtility.GenerateInitialOldInjuries(pawn);
            pawn.Health = pawn.MaxHealth;

            if (pawn.RaceProps.hasStory)
            {
                pawn.story.skinColor = PawnSkinColors.RandomSkinColor();
                pawn.story.crownType = (double)Rand.Value >= 0.5 ? CrownType.Narrow : CrownType.Average;
                pawn.story.headGraphicPath = GraphicDatabaseHeadRecords.GetHeadRandom(pawn.gender, pawn.story.skinColor, pawn.story.crownType).GraphicPath;
                pawn.story.hairColor = PawnHairColors.RandomHairColor(pawn.story.skinColor, pawn.ageTracker.AgeBiologicalYears);
                PawnBioGenerator.GiveAppropriateBioTo(pawn, faction.def);
                pawn.story.hairDef = PawnHairChooser.RandomHairDefFor(pawn, faction.def);
                //PawnGenerator.GiveRandomTraitsTo(pawn);
                //pawn.story.GenerateSkillsFromBackstory();

                pawn.story.childhood = Enhanced_Defence.Vehicles.Helper.BackstoryHelper.GetBackstory();
                pawn.story.adulthood = Enhanced_Defence.Vehicles.Helper.BackstoryHelper.GetBackstory();

                MakeSkillsFromBackstory(pawn);
            }

            //PawnApparelGenerator.GenerateStartingApparelFor(pawn);
            //PawnInventoryGenerator.GiveAppropriateKeysTo(pawn);
            //PawnWeaponGenerator.TryGenerateWeaponFor(pawn);
            //PawnArtificialBodyPartsGenerator.GeneratePartsFor(pawn);
            //PawnInventoryGenerator.GenerateInventoryFor(pawn);
            pawn.AddAndRemoveComponentsAsAppropriate();

            //pawn.workSettings.Disable(WorkTypeDefOf.Cleaning);
            //pawn.workSettings.Disable(WorkTypeDefOf.Construction);
            //pawn.workSettings.Disable(WorkTypeDefOf.Cooking);
            //pawn.workSettings.Disable(WorkTypeDefOf.Doctor);
            //pawn.workSettings.Disable(WorkTypeDefOf.Firefighter);
            //pawn.workSettings.Disable(WorkTypeDefOf.Growing);
            //pawn.workSettings.Disable(WorkTypeDefOf.Hauling);
            //pawn.workSettings.Disable(WorkTypeDefOf.Hunting);
            //pawn.workSettings.Disable(WorkTypeDefOf.Mining);
            //pawn.workSettings.Disable(WorkTypeDefOf.PlantCutting);
            //pawn.workSettings.Disable(WorkTypeDefOf.Repair);
            //pawn.workSettings.Disable(WorkTypeDefOf.Research);
            //pawn.workSettings.Disable(WorkTypeDefOf.Warden);

            return (VehiclePawn)pawn;
        }

        public static void MakeSkillsFromBackstory(VehiclePawn pawn)
        {
            IEnumerator<SkillDef> enumerator = DefDatabase<SkillDef>.AllDefs.GetEnumerator();
            try
            {

                while (enumerator.MoveNext())
                {
                    SkillDef current = enumerator.Current;
                    //int num = FinalLevelOfSkill(current);
                    int num = 5;
                    SkillRecord skill = pawn.skills.GetSkill(current);
                    skill.level = num;
                    if (skill.TotallyDisabled)
                    {
                        continue;
                    }

                    skill.xpSinceLastLevel = 0;

                    switch (0)
                    {
                        case 1:
                            skill.passion = Passion.Minor;
                            break;
                        case 2:
                            skill.passion = Passion.Major;
                            break;
                        default:
                            skill.passion = Passion.None;
                            break;
                    }

                }
            }
            finally
            {
                enumerator.Dispose();
            }
        }


    }
}
