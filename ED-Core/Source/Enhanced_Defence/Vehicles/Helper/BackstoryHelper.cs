using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace Enhanced_Defence.Vehicles.Helper
{
    class BackstoryHelper
    {
        /*
        public static void AddNewBackstorieToDatabase()
        {


            // Create minimal backstory
            backstory = new Backstory();
            backstory.baseDesc = txtBaseDesc;
            backstory.title = txtTitle;
            backstory.titleShort = txtTitleShort;
            backstory.bodyTypeGlobal = bodyType;
            backstory.workDisables = workDisables1;
            backstory.defName = baseBackstoryDefName + backstory.workDisables.ToString().Replace(", ", "_");
            backstories.Add(backstory);
        }
        */

        public static Backstory GetBackstory()
        {
            Backstory backstory;

            WorkTags workDisables = WorkTags.Artistic;

            workDisables = workDisables | WorkTags.Caring;
            workDisables = workDisables | WorkTags.Cleaning;
            workDisables = workDisables | WorkTags.Cooking;
            workDisables = workDisables | WorkTags.Crafting;
            workDisables = workDisables | WorkTags.Firefighting;
            workDisables = workDisables | WorkTags.Hauling;
            workDisables = workDisables | WorkTags.Intellectual;
            workDisables = workDisables | WorkTags.ManualDumb;
            workDisables = workDisables | WorkTags.ManualSkilled;
            workDisables = workDisables | WorkTags.Mining;
            workDisables = workDisables | WorkTags.None;
            workDisables = workDisables | WorkTags.PlantWork;
            workDisables = workDisables | WorkTags.Scary;
            workDisables = workDisables | WorkTags.Social;
            workDisables = workDisables | WorkTags.Violent;

            // Create minimal backstory
            backstory = new Backstory();
            backstory.baseDesc = "Vehicle";
            backstory.title = "Vehicle";
            backstory.titleShort = "Vehicle";
            backstory.bodyTypeGlobal = BodyType.Female;
            backstory.workDisables = workDisables;
            //backstory.defName = baseBackstoryDefName + backstory.workDisables.ToString().Replace(", ", "_");


            //WorkTags workDisables = WorkTags.Artistic | WorkTags.Social;

            //workDisables = workDisables | WorkTags.Violent;

            //string backstoryDefName = "AutonomicAI" + "_disabled_" + workDisables.ToString().Replace(", ", "_");
            //return BackstoryDatabase.GetWithKey(backstoryDefName);
            return backstory;
        }
    }
}
