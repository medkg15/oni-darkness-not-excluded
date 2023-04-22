using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DarknessNotIncluded.CellsRespectLightLevels
{
  static class SelectToolBlockedByDarkness
  {
    [HarmonyPatch(typeof(SelectToolHoverTextCard)), HarmonyPatch("UpdateHoverElements")]
    static class Patched_SelectToolHoverTextCard_UpdateHoverElements
    {
      static bool Prefix(SelectToolHoverTextCard __instance, List<KSelectable> hoverObjects)
      {
        int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
        var inspectionLevel = Behavior.InspectionLevelForCell(cell);
        switch (inspectionLevel)
        {
          case InspectionLevel.None:
            RenderUnknownHoverCard(__instance);
            return false;

          case InspectionLevel.BasicDetails:
            RenderBasicHoverCard(__instance, hoverObjects);
            return false;

          case InspectionLevel.FullDetails:
          default:
            return true;
        }
      }

      static void RenderUnknownHoverCard(SelectToolHoverTextCard hoverCard)
      {
        HoverTextDrawer drawer = HoverTextScreen.Instance.BeginDrawing();
        drawer.BeginShadowBar();
        drawer.DrawIcon(hoverCard.iconWarning);
        drawer.DrawText(STRINGS.UI.TOOLS.GENERIC.UNKNOWN, hoverCard.Styles_BodyText.Standard);
        drawer.EndShadowBar();
        hoverCard.recentNumberOfDisplayedSelectables = 1;
        drawer.EndDrawing();
      }

      static void RenderBasicHoverCard(SelectToolHoverTextCard hoverCard, List<KSelectable> hoverObjects)
      {
        HoverTextDrawer drawer = HoverTextScreen.Instance.BeginDrawing();

        foreach (var hoverObject in hoverObjects)
        {
          drawer.BeginShadowBar(SelectTool.Instance.selected == hoverObject);
          drawer.DrawText($"wat {hoverObject}", hoverCard.Styles_Title.Standard);
          drawer.EndShadowBar();
        }

        drawer.EndDrawing();
      }
    }

    [HarmonyPatch(typeof(SelectTool)), HarmonyPatch("Select")]
    static class Patched_SelectTool_Select
    {
      static bool Prefix(KSelectable new_selected, bool skipSound)
      {
        if (new_selected == null) return true;

        var cell = Grid.PosToCell(new_selected);
        return Behavior.InspectionLevelForCell(cell) != InspectionLevel.None;
      }
    }
  }
}
