using HarmonyLib;

namespace DarknessNotIncluded.LightBonuses
{
  public static class DecorBonusThreshold
  {
    private static int decorBonusThresholdLux;

    private static Config.Observer configObserver = new Config.Observer((config) =>
    {
      decorBonusThresholdLux = config.decorBonusThresholdLux;
    });

    [HarmonyPatch(typeof(DecorProvider)), HarmonyPatch(nameof(DecorProvider.GetLightDecorBonus))]
    static class Patched_DecorProvider_GetLightDecorBonus
    {
      static void Postfix(int cell, ref int __result)
      {
        __result = Grid.LightIntensity[cell] >= decorBonusThresholdLux ? TUNING.DECOR.LIT_BONUS : 0;
      }
    }
  }

}
