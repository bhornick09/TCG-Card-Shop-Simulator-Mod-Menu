using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using HarmonyLib.Tools;
using UnityEngine;
using System.ComponentModel;

namespace TCGCardPandL
{
    [BepInPlugin("com.wubzzey.pandlplugin", "PackPandL", "1.0.0")]
    [BepInProcess("Card Shop Simulator.exe")] // Only run plugin with Card Shop Sim
    public class Plugin : BaseUnityPlugin
    {
        // Config Dropdown Menus and Bools
        internal ConfigEntry<GodPackMode> godPackSelection;
        internal ConfigEntry<bool> freeShopItems;
        internal ConfigEntry<EDecoObject> spawnDecoSelection;

        // Config Hotkeys
        private ConfigEntry<KeyboardShortcut> SpawnDecoHK { get; set; }
        private ConfigEntry<KeyboardShortcut> SpawnMoneyHK { get; set; }
        private ConfigEntry<KeyboardShortcut> RemoveMoneyHK { get; set; }
        private ConfigEntry<KeyboardShortcut> ShopLevelUpHK { get; set; }

        internal static Plugin Instance;

        private void Awake()
        {
            Instance = this;

            // Bind config entries to menu
            godPackSelection = Config.Bind("Gameplay", "Pack Odds Selection",
             GodPackMode.Normal, "All packs will open with this type");

            spawnDecoSelection = Config.Bind("Gameplay", "Spawn this Decoration (Shift+O)",
             EDecoObject.None, "Use O to spawn");

            freeShopItems = Config.Bind("Gameplay", "Free MyDIY objects", false,
               new ConfigDescription("Toggle free MyDIY objects"));

            //Hotkeys
            SpawnDecoHK = Config.Bind("Hotkeys", "Spawn Decor Selection",
                new KeyboardShortcut(KeyCode.O));

            SpawnMoneyHK = Config.Bind("Hotkeys", "Add $5000 (Shift+P)",
                new KeyboardShortcut(KeyCode.P, KeyCode.LeftShift));

            RemoveMoneyHK = Config.Bind("Hotkeys", "Remove $5000 (Alt+P)",
                new KeyboardShortcut(KeyCode.P, KeyCode.LeftAlt));

            ShopLevelUpHK = Config.Bind("Hotkeys", "Level Up (Shift+L)",
                new KeyboardShortcut(KeyCode.L, KeyCode.LeftShift));

            // initialize harmony
            var harmony = new Harmony("com.wubzzey.pandlplugin");
            harmony.PatchAll();
            HarmonyFileLog.Enabled = true;
        }
        private void Update()
        {
            {
                // Check for hotkey presses
                if (SpawnDecoHK.Value.IsDown())
                {
                    EDecoObject spawnDecoConfirm = Instance.spawnDecoSelection.Value;
                    ShelfManager.SpawnDecoObjectOnHand(spawnDecoConfirm);
                }
                if (SpawnMoneyHK.Value.IsDown())
                {
                    var coinEvent = new CEventPlayer_AddCoin(5000, true);
                    CEventManager.QueueEvent(coinEvent);
                }
                if (RemoveMoneyHK.Value.IsDown())
                {
                    var coinEvent = new CEventPlayer_ReduceCoin(5000, true);
                    CEventManager.QueueEvent(coinEvent);
                }
                if (ShopLevelUpHK.Value.IsDown())
                {

                }

            }
        }
    }
    //used for godpack dropdown menu
    public enum GodPackMode
    {
        Normal = 0,
        FirstEdition = 1,
        Silver = 2,
        Gold = 3,
        EX = 4,
        FullArt = 5,
        FoilFirstEdition = 6,
        FoilSilver = 7,
        FoilGold = 8,
        FoilEX = 9,
        FoilFullArt = 10,
        AllFoil = 12,
        Ghost = 20 // outside of normal roll index (overrides different value)
    }
}
