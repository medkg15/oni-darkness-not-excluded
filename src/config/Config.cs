using HarmonyLib;
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using UnityEngine;

namespace DarknessNotIncluded
{
  [JsonObject(MemberSerialization.OptOut)]
  [ModInfo("https://github.com/nevir/oni-darkness-not-excluded")]
  [ConfigFile(SharedConfigLocation: true)]
  class Config : SingletonOptions<Config>
  {
    // Darkness

    [Option("Darkness grace period (cycles)", "How many cycles should it take to go from no darkness to maximum darkness?", "Darkness")]
    public float gracePeriodCycles { get; set; }

    [Option("Initial darkness level", "How dark should 0 lux tiles be at the start of the game?\n0 = pitch black, 255 = fully visible", "Darkness")]
    [Limit(0, 255)]
    public int initialFogLevel { get; set; }

    [Option("Maximum darkness level", "How dark should 0 lux tiles be after the initial grace period?\n0 = pitch black, 255 = fully visible", "Darkness")]
    [Limit(0, 255)]
    public int minimumFogLevel { get; set; }

    [Option("Lux threshold", "At what lux should a tile be fully visible?", "Darkness")]
    public int fullyVisibleLuxThreshold { get; set; }

    [Option("Tooltip blocked by darkness", "Whether the tooltip should not show information if pointing at a dark tile.", "Darkness")]
    public bool selectToolBlockedByDarkness { get; set; }

    // Dupe Behavior

    [Option("Disable lights in lit areas", "Whether dupes should turn their lights off when entering an area with at least the same level of brightness as their light.", "Dupe Behavior")]
    public bool disableDupeLightsInLitAreas { get; set; }

    [Option("Disable lights around sleeping dupes", "Whether dupes should turn their lights off when entering a bedroom.", "Dupe Behavior")]
    public bool disableDupeLightsInBedrooms { get; set; }

    [Option("Maximum lux while sleeping", "The maximum lux that a dupe can handle before being rudely woken up.", "Dupe Behavior")]
    public int maxSleepingLux { get; set; }

    // Dupe (Hat) Lights

    [DynamicOption(typeof(MinionLightingConfigEntry), "Dupe Lights")]
    [Option("Configuration for dupe lights", "Configuration for dupe lights", "Dupe Lights")]
    public MinionLightingConfig minionLightingConfig { get; set; }

    public Config()
    {
      // Darkness
      initialFogLevel = 150;
      minimumFogLevel = 10;
      gracePeriodCycles = 3.0f;
      fullyVisibleLuxThreshold = TUNING.DUPLICANTSTATS.LIGHT.MEDIUM_LIGHT;
      selectToolBlockedByDarkness = true;

      // Dupe Behavior
      disableDupeLightsInLitAreas = true;
      disableDupeLightsInBedrooms = true;
      maxSleepingLux = 400;

      minionLightingConfig = new MinionLightingConfig {
        { MinionLightType.Intrinsic, new MinionLightingConfig.LightConfig(true,  200,  2, Color.white) },
        { MinionLightType.Mining1,   new MinionLightingConfig.LightConfig(true,  600,  3, TUNING.LIGHT2D.LIGHT_YELLOW)},
        { MinionLightType.Mining2,   new MinionLightingConfig.LightConfig(true,  800,  4, TUNING.LIGHT2D.LIGHT_YELLOW)},
        { MinionLightType.Mining3,   new MinionLightingConfig.LightConfig(true,  1200, 5, Color.white)},
        { MinionLightType.Mining4,   new MinionLightingConfig.LightConfig(true,  1600, 6, Color.white)},
        { MinionLightType.Science,   new MinionLightingConfig.LightConfig(true,  400,  3, Color.white)},
        { MinionLightType.Rocketry,  new MinionLightingConfig.LightConfig(true,  600,  4, Color.white)},
      };
    }

    [HarmonyPatch(typeof(Game)), HarmonyPatch("OnPrefabInit")]
    static class Patched_Game_OnPrefabInit
    {
      static void Prefix()
      {
        Config.instance = POptions.ReadSettings<Config>();
      }
    }
  }
}
