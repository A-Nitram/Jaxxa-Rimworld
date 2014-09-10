using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace Jaxxa_Shields
{
    public static class ShieldedPawnGenerator
    {
        public static ShieldedPawn GeneratePawn(string kindDefName, Faction faction)
        {
            return ShieldedPawnGenerator.GeneratePawn(PawnKindDef.Named(kindDefName), faction);
        }

        public static ShieldedPawn GeneratePawn(PawnKindDef kindDef, Faction faction)
        {
            Log.Error("Start GeneratePawn()");

            Log.Error("Race: " + kindDef.race.ToString());

            Log.Error("Ping0");
            //Pawn pawn = (Pawn)ThingMaker.MakeThing(kindDef.race);
            ShieldedPawn pawn = (ShieldedPawn)ThingMaker.MakeThing(kindDef.race);

            Log.Error("Ping1");


            /*
            Log.Error("Ping0.1");
            Pawn testing = (Pawn)Activator.CreateInstance(kindDef.race.thingClass);
            Log.Error("Ping0.2");
            testing.def = kindDef.race;
            Log.Error("Ping0.3");
            testing.PostMake();
            Log.Error("Ping0.4");
            ShieldedPawn pawn = (ShieldedPawn)testing;

            Log.Error("Ping0.5");
            pawn.kindDef = kindDef;
            pawn.SetFactionDirect(faction);
            Log.Error("Ping1");*/

            pawn.thinker = new Pawn_Thinker(pawn);
            Log.Error("Ping2");

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
                pawn.carryHands = new Pawn_CarryHands(pawn);
                pawn.psychology = new Pawn_PsychologyTracker(pawn);
                pawn.story = new Pawn_StoryTracker(pawn);
                pawn.workSettings = new Pawn_WorkSettings(pawn);
            }
            else
                pawn.caller = new Pawn_CallTracker(pawn);
            if (pawn.RaceProps.EatsFood)
                pawn.food = new Pawn_FoodTracker(pawn);
            if (pawn.RaceProps.needsRest)
                pawn.rest = new Pawn_RestTracker(pawn);
            pawn.gender = !pawn.RaceProps.hasGenders ? Gender.Sexless : ((double)Rand.Value >= 0.5 ? Gender.Female : Gender.Male);
            pawn.age = ShieldedPawnGenerator.RandomAgeFor(pawn);
            pawn.health = pawn.healthTracker.MaxHealth;

            Log.Error("Ping3");
            if ((double)Rand.Value < (double)Mathf.Lerp(0.05f, 0.8f, Mathf.Clamp(Mathf.InverseLerp(20f, 80f, (float)pawn.age), 0.0f, 1f)) && pawn.RaceProps.humanoid)
                PawnGenerator.ApplyOldWound(pawn);
            if (pawn.RaceProps.hasStory)
            {
                pawn.story.skinColor = PawnSkinColors.RandomSkinColor();
                pawn.story.crownType = (double)Rand.Value >= 0.5 ? CrownType.Narrow : CrownType.Average;
                pawn.story.headGraphicPath = GraphicDatabase_Head.GetHeadRandom(pawn.gender, pawn.story.skinColor, pawn.story.crownType).graphicPath;
                pawn.story.hairColor = PawnHairColors.RandomHairColor(pawn.story.skinColor);
                PawnBioGenerator.GiveAppropriateBioTo(pawn, faction.def);
                pawn.story.hairDef = PawnHairChooser.RandomHairDefFor(pawn, faction.def);
                pawn.story.MakeSkillsFromBackstory();
                ShieldedPawnGenerator.GiveRandomTraitsTo(pawn);
            }

            Log.Error("Ping4");
            PawnInventoryGenerator.GiveAppropriateKeysTo(pawn);
            PawnInventoryGenerator.GenerateWeaponFor(pawn);
            PawnInventoryGenerator.GenerateInventoryFor(pawn);
            PawnApparelGenerator.GenerateStartingApparelFor(pawn);
            pawn.AddAndRemoveComponentsAsAppropriate();

            Log.Error("About to Return");
            return pawn;
        }

        private static void GiveRandomTraitsTo(Pawn pawn)
        {
            if (pawn.story == null)
                return;
            int num = Rand.RangeInclusive(1, 2);
            while (pawn.story.traits.allTraits.Count < num)
            {
                TraitDef traitDef = GenLinq.RandomElementByWeight<TraitDef>((IEnumerable<TraitDef>)DefDatabase<TraitDef>.AllDefsListForReading, (Func<TraitDef, float>)(tr => tr.commonality));
                if (!pawn.story.traits.HasTrait(traitDef))
                {
                    Trait trait = new Trait(traitDef);
                    trait.degree = PawnGenerator.RandomTraitDegree(trait.def);
                    pawn.story.traits.GainTrait(trait);
                }
            }
        }

        private static int RandomAgeFor(Pawn pawn)
        {
            if (!pawn.RaceProps.humanoid)
                return Rand.Range(1, 10);
            switch (Rand.RangeInclusive(1, 12))
            {
                case 1:
                    return Rand.Range(15, 20);
                case 2:
                    return Rand.Range(18, 25);
                case 3:
                    return Rand.Range(21, 30);
                case 4:
                    return Rand.Range(21, 30);
                case 5:
                    return Rand.Range(21, 30);
                case 6:
                    return Rand.Range(31, 40);
                case 7:
                    return Rand.Range(31, 40);
                case 8:
                    return Rand.Range(31, 40);
                case 9:
                    return Rand.Range(41, 50);
                case 10:
                    return Rand.Range(41, 50);
                case 11:
                    return Rand.Range(51, 60);
                case 12:
                    return Rand.Range(61, 70);
                default:
                    Log.Warning("Didn't get age for " + (object)pawn);
                    return 19;
            }
        }
    }
}
