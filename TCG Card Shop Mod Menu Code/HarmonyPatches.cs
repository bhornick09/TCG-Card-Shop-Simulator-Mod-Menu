using HarmonyLib;
using System.Reflection;

namespace TCGCardPandL
{
    // God Pack (change random number to god pack key)
    [HarmonyPatch(typeof(CardOpeningSequence), "GetPackContent")]
    public static class GodPackPatch
    {
        // Use access tools to access the card pack type 
        static FieldInfo collectionPackTypeField = AccessTools.Field(typeof(CardOpeningSequence), "m_CollectionPackType");

        static void Prefix(CardOpeningSequence __instance, ref int godPackRollIndex, ref ECollectionPackType overrideCollectionPackType)
        {
            // if Normal is not selected, override pack odds with selection
            if (Plugin.Instance.godPackSelection.Value != GodPackMode.Normal)
            {
                // check for type ghost, which overrides godPackRollIndex if true
                if (Plugin.Instance.godPackSelection.Value == GodPackMode.Ghost)
                {
                    collectionPackTypeField.SetValue(__instance, ECollectionPackType.GhostPack);
                }
                // if not ghost push other pack type
                else
                {
                    GodPackMode packTypeSelection = Plugin.Instance.godPackSelection.Value;
                    godPackRollIndex = (int)packTypeSelection; //convert to int for index use
                }
            }
        }
    }

    // Override checkout total
    [HarmonyPatch(typeof(FurnitureShopUIScreen), "EvaluateCartCheckout")]
    public static class EvaluateCartCheckoutPatch
    {

        static void Prefix(ref float totalCost)
        {
            if (Plugin.Instance.freeShopItems.Value)
            {
                // Override checkout total to 0
                totalCost = 0.0f;
            }
        }
    }
}