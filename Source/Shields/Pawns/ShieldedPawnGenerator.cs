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
            //Does this work?
            return (ShieldedPawn)PawnGenerator.GeneratePawn(kindDef, faction);
        }

        public static ShieldedPawn GeneratePawn(string kindDefName, Faction faction, Pawn currentPawn)
        {
            return ShieldedPawnGenerator.GeneratePawn(PawnKindDef.Named(kindDefName), faction, currentPawn);
        }

        public static ShieldedPawn GeneratePawn(PawnKindDef kindDef, Faction faction, Pawn currentPawn)
        {
            ShieldedPawn newPawn = ShieldedPawnGenerator.GeneratePawn(kindDef, faction);

            /*
            newPawn.age = currentPawn.age;
            newPawn.apparel = currentPawn.apparel;
            newPawn.attachments = currentPawn.attachments;
            newPawn.caller = currentPawn.caller;
            newPawn.carryHands = currentPawn.carryHands;
            newPawn.def = currentPawn.def;
            newPawn.drawer = currentPawn.drawer;
            newPawn.equipment = currentPawn.equipment;
            newPawn.filth = currentPawn.filth;
            newPawn.food = currentPawn.food;
            newPawn.gender = currentPawn.gender;
            newPawn.health = currentPawn.health;
            newPawn.healthTracker = currentPawn.healthTracker;
            newPawn.holder = currentPawn.holder;
            newPawn.inventory = currentPawn.inventory;
            newPawn.jailerFactionInt = currentPawn.jailerFactionInt;
            newPawn.jobs = currentPawn.jobs;
            newPawn.kindDef = currentPawn.kindDef;
            newPawn.natives = currentPawn.natives;
            newPawn.ownership = currentPawn.ownership;
            newPawn.pather = currentPawn.pather;
            newPawn.playerController = currentPawn.playerController;
            newPawn.prisoner = currentPawn.prisoner;
            newPawn.psychology = currentPawn.psychology;
            newPawn.rest = currentPawn.rest;
            newPawn.rotation = currentPawn.rotation;
            newPawn.skills = currentPawn.skills;
            newPawn.stackCount = currentPawn.stackCount;
            newPawn.stances = currentPawn.stances;
            newPawn.story = currentPawn.story;
            newPawn.talker = currentPawn.talker;
            newPawn.thingIDNumber = currentPawn.thingIDNumber;
            newPawn.thinker = currentPawn.thinker;
            newPawn.workSettings = currentPawn.workSettings;
            */

            newPawn.age = currentPawn.age;
            newPawn.gender = currentPawn.gender;

            //newPawn.story = new Pawn_StoryTracker();
            newPawn.story.name = currentPawn.story.name;


            return newPawn;
        }
    }
}
