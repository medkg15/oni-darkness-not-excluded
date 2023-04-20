using System.Collections.Generic;

namespace DarknessNotIncluded
{
  public enum MinionEffectType
  {
    Dim,
    Dark,
  }

  public class MinionEffectsConfig : Dictionary<MinionEffectType, MinionEffectsConfig.EffectConfig>
  {
    public class EffectConfig
    {
      public bool enabled { get; set; }
      public int luxThreshold { get; set; }
      public int statsModifier { get; set; }

      public EffectConfig(bool enabled, int luxThreshold, int athleticsModifier)
      {
        this.enabled = enabled;
        this.luxThreshold = luxThreshold;
        this.statsModifier = athleticsModifier;
      }

      public EffectConfig DeepClone()
      {
        return new EffectConfig(enabled, luxThreshold, statsModifier);
      }
    }

    public MinionEffectsConfig DeepClone()
    {
      var newConfig = new MinionEffectsConfig();
      foreach (var pair in this)
      {
        newConfig.Add(pair.Key, pair.Value.DeepClone());
      }
      return newConfig;
    }
  }
}
