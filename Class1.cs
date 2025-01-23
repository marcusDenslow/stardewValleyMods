using System;
using System.Threading;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;
using System.Collections.Generic;
using StardewValley.GameData.Shops;


print("hello world")
console.log("hello world")


namespace stardewvalleyMod
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {

        private int GoldAddedManually = 0;
        /*********
        ** Public methods
        *********/

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            //helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            //helper.Events.Input.ButtonPressed += this.gratulererNatalie;
            helper.Events.GameLoop.TimeChanged += this.SetTimeNight;
            helper.Events.Input.ButtonPressed += GiveGold;
            helper.Events.Input.ButtonPressed += RemoveGold;
            helper.Events.Player.Warped += GreetMessageOnWarpFarm;
            helper.Events.Player.Warped += givestaminaandhealthonwarptown;
            helper.Events.GameLoop.DayStarted += TimeOfDay;
            helper.Events.GameLoop.TimeChanged += HeiPiaHudMessage;
            helper.Events.GameLoop.DayStarted += GrowCropsInta;
            //helper.Events.GameLoop.DayStarted += WeatherIsAlwaysRainy;
            helper.Events.Content.AssetRequested += OnAssetRequested;
            helper.Events.GameLoop.TimeChanged += LogStamina;
            helper.Events.GameLoop.DayStarted += BirthdayReminders;
            helper.Events.GameLoop.DayStarted += HelperWateredCrops;
            Helper.Events.Player.Warped += HelloMsgInMap;
        }



        //private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        //{
        //    // ignore if player hasn't loaded a save yet
        //    if (!Context.IsWorldReady)
        //        return;

        //    // print button presses to the console window
        //    this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        //}

        //private void PiaErCute(object? sender, ButtonPressedEventArgs e)
        //{
        //    if (!Context.IsWorldReady)
        //        return;
        //    this.Monitor.Log("du er søt pia", LogLevel.Debug);
        //}

        //private void gratulererNatalie(object? sender, ButtonPressedEventArgs e)
        //{
        //    if (!Context.IsWorldReady)
        //        return;
        //    this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}. -NOTE FRA DEV gratulerer med dagen natalie");
        //}

        private void SetTimeNight(object? sender, TimeChangedEventArgs e)
        {
            this.Monitor.Log($"{Game1.timeOfDay}", LogLevel.Debug);
        }


        private void LogStamina(object? sender, TimeChangedEventArgs e)
        {
            this.Monitor.Log($"{Game1.player.stamina}", LogLevel.Debug);
            this.Monitor.Log($"{Game1.player.health}", LogLevel.Debug);
        }

        private void HeiPiaHudMessage(object? sender, TimeChangedEventArgs e)
        {
            if (Game1.timeOfDay == 600)
            {
                Game1.showGlobalMessage("hei pia");
            }
        }



       private void RemindPlayerOfSeasonalCrops(object? sender, DayStartedEventArgs e)
       {
         if (Game1.currentSeason == "fall")
         {
           Game1.showGlobalMessage("It's fall. Don't forget to buy the seasonal Items")
         }
         else if (Game1.currentSeason == "summer")
         {
           Game1.showGlobalMessage("It's summer. Don't forget to buy the seasonal Items")
         }
         else if (Game1.currentSeason == "winter")
         {
           Game1.showGlobalMessage("It's winter. Don't forget to buy the seasonal Items")
         }
          else if (Game1.currentSeason == "spring")
         {
           Game1.showGlobalMessage("It's spring. Don't forget to buy the seasonal Items")
         }
       
       }
        

        private void getHealthOverTimeInSauna(object? sender, WarpedEventArgs e)
        {
          if (e.NewLocation.Name == "Sauna" && Game1.player.health <= Game1.player.maxHealth - 50)
          {
            int milliseconds = 2000
            while (Game1.player.health < Game1.player.maxHealth -20)
            {
              Game1.player.health += 20;
              Thread.SLeep(millseconds)
            }
          }
        }




        private void GiveGold(object? sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.G)
            {
                Game1.player.Money += 1000;
                GoldAddedManually += 1000;

                if (GoldAddedManually == 5000)
                {
                    Game1.addHUDMessage(new HUDMessage("cheater! stopp adding gold, and go play the game", HUDMessage.error_type));
                }

                if (GoldAddedManually == 10000)
                {
                    Game1.addHUDMessage(new HUDMessage($"still cheating? you have now manually added {GoldAddedManually} gold", HUDMessage.error_type));
                }

            }


        }

        private void HelperWateredCrops(object? sender, EventArgs e)
        {
          foreach (var terrainFeature in Game1.getFarm().terrainFeatures.Pairs)
          {
            if (terrainFeature.Value is HoeDirt dirt && dirt.crop != null && dirt.state.Value == HoeDirt.dry)
              dirt.state.Value = HoeDirt.watered;
          }
            Game1.addHUDMessage(new HUDMessage("Helper Watered Crops!", HUDMessage.newQuest_type));
            DelayedAction.playSoundAfterDelay("wateringCan", 1500);
        }

        private void RemoveGold(object? sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.H)
                Game1.player.Money -= 1000;
        }

        private void GreetMessageOnWarpFarm(object? sender, WarpedEventArgs e)
        {
            if (e.NewLocation.Name == "Farm" && Game1.timeOfDay >= 2000)
            {
              if (Game1.isRaining)
              {
                Game1.showGlobalMessage($"Welcome home {Game1.player.name}. What a rainy day")
              }
              else
              {                
                Game1.showGlobalMessage($"Welcome home {Game1.player.name}. What a rainy day")
              }
            }
        }



        private void givestaminaandhealthonwarptown(object? sender, WarpedEventArgs e)
        {
            if (e.NewLocation.Name == "Map")
                if (Game1.player.stamina <= Game1.player.maxStamina.Value - 50)
                {
                    Game1.player.stamina += 50;
                    Game1.addHUDMessage(new HUDMessage("restored 50 stamina", HUDMessage.stamina_type));
                    Game1.player.Items.Add(new StardewValley.Object("24", 3));
                    Game1.addHUDMessage(new HUDMessage("Recieved parsnip!", HUDMessage.stamina_type));
                }
                else
                    Game1.addHUDMessage(new HUDMessage("You have not used enough stamina yet", HUDMessage.error_type));

        }




        private void TimeOfDay(object? sender, EventArgs e)
        {
            Game1.timeOfDay = 1000;
            Game1.currentSeason = "fall";
        }


        private void GrowCropsInta(object? sender, EventArgs e)
        {

            if (Game1.isRaining || Game1.isDebrisWeather)
            {
                foreach (var terrainFeature in Game1.getFarm().terrainFeatures.Pairs)
                {
                    if (terrainFeature.Value is HoeDirt dirt && dirt.crop != null)
                    {
                        dirt.crop.growCompletely();
                    }
                }
                Game1.showGlobalMessage("Rain finished crop growth!");
            }
        }

        private void warpOnFarm(object? sender, WarpedEventArgs e)
        {
          if (e.NewLocation.Name == "Farm")
          {
            Game1.showGlobalMessage($"Welcome back to the farm {Game1.player.Name}")
          }
        }

     

        //private void WeatherIsAlwaysRainy(object? sender, StardewModdingAPI.Events.DayStartedEventArgs e)
        //{
        //    Game1.weatherForTomorrow = Game1.weather_rain;
        //}

        public void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Shops"))
            {
                e.Edit(asset =>
                {
                    var shopData = asset.AsDictionary<string, ShopData>().Data;
                    // This would throw if SeedShop doesn't exist, which should never happens, hopefully
                    var seedShop = shopData["SeedShop"];
                    var wheatShopEntry = seedShop.Items.Find(entry => entry.Id == "WheatSeeds_Summer");
                    var sunFlowerShopEntry = seedShop.Items.Find(entry => entry.Id == "SunflowerSeeds_Summer");

                    if (wheatShopEntry is not null && sunFlowerShopEntry is not null)
                    {
                        // Remove the condition that limits this entry to summer
                        wheatShopEntry.Condition = null;
                        sunFlowerShopEntry.Condition = null;
                    }
                });
            }
        }

        public void BirthdayReminders(object? sender, EventArgs e)
        {
            var day = Game1.dayOfMonth;
            if (Game1.currentSeason == "fall")
            {
                switch (day)
                {
                    case 2:
                        Game1.addHUDMessage(new HUDMessage("It's Penny's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 5:
                        Game1.addHUDMessage(new HUDMessage("It's Elliot's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 11:
                        Game1.addHUDMessage(new HUDMessage("It's Jodi's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 13:
                        Game1.addHUDMessage(new HUDMessage("It's Abigail's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 15:
                        Game1.addHUDMessage(new HUDMessage("It's Sandy's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 18:
                        Game1.addHUDMessage(new HUDMessage("It's Marnie's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 21:
                        Game1.addHUDMessage(new HUDMessage("It's Robin's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 24:
                        Game1.addHUDMessage(new HUDMessage("It's George's birthday today!", HUDMessage.newQuest_type));
                        break;
                }
            }
            if (Game1.currentSeason == "summer")
            {
                switch (day)
                {
                    case 4:
                        Game1.addHUDMessage(new HUDMessage("It's Jas's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 8:
                        Game1.addHUDMessage(new HUDMessage("It's Gus's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 10:
                        Game1.addHUDMessage(new HUDMessage("It's Maru's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 13:
                        Game1.addHUDMessage(new HUDMessage("It's Alex's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 17:
                        Game1.addHUDMessage(new HUDMessage("It's Sam's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 19:
                        Game1.addHUDMessage(new HUDMessage("It's Demetrius's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 22:
                        Game1.addHUDMessage(new HUDMessage("It's Dwarf's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 24:
                        Game1.addHUDMessage(new HUDMessage("It's Willy's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 26:
                        Game1.addHUDMessage(new HUDMessage("It's Leo's birthday today!", HUDMessage.newQuest_type));
                        break;
                }
            }

            if (Game1.currentSeason == "spring")
            {

                switch (day)
                {
                    case 4:
                        Game1.addHUDMessage(new HUDMessage("It's kent's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 7:
                        Game1.addHUDMessage(new HUDMessage("It's Lewis's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 10:
                        Game1.addHUDMessage(new HUDMessage("It's Vincent's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 14:
                        Game1.addHUDMessage(new HUDMessage("It's Haley's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 18:
                        Game1.addHUDMessage(new HUDMessage("It's Pam's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 20:
                        Game1.addHUDMessage(new HUDMessage("It's Shane's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 26:
                        Game1.addHUDMessage(new HUDMessage("It's Pierre's birthday today!", HUDMessage.newQuest_type));
                        break;
                    case 27:
                        Game1.addHUDMessage(new HUDMessage("It's Emily's birthday today!", HUDMessage.newQuest_type));
                        break;
                }
                if (Game1.currentSeason == "winter")
                    switch (day)
                    {
                        case 1:
                            Game1.addHUDMessage(new HUDMessage("It's Krobus's birthday today!", HUDMessage.newQuest_type));
                            break;
                        case 3:
                            Game1.addHUDMessage(new HUDMessage("It's Linus's birthday today!", HUDMessage.newQuest_type));
                            break;
                        case 7:
                            Game1.addHUDMessage(new HUDMessage("It's Caroline's birthday today!", HUDMessage.newQuest_type));
                            break;
                        case 10:
                            Game1.addHUDMessage(new HUDMessage("It's Sebastian's birthday today!", HUDMessage.newQuest_type));
                            break;
                        case 14:
                            Game1.addHUDMessage(new HUDMessage("It's Harvey's birthday today!", HUDMessage.newQuest_type));
                            break;
                        case 17:
                            Game1.addHUDMessage(new HUDMessage("It's Wizard's birthday today!", HUDMessage.newQuest_type));
                            break;
                        case 20:
                            Game1.addHUDMessage(new HUDMessage("It's Evilyn's birthday today!", HUDMessage.newQuest_type));
                            break;
                        case 23:
                            Game1.addHUDMessage(new HUDMessage("It's Leah's birthday today!", HUDMessage.newQuest_type));
                            break;
                        case 26:
                            Game1.addHUDMessage(new HUDMessage("It's Clint's birthday today!", HUDMessage.newQuest_type));
                            break;
                    }
            }
        }
    }
}

