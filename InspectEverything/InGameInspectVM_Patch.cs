using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker;
using Kingmaker.Inspect;
using Kingmaker.UI.MVVM._VM.Inspect;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.View;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Kingmaker.UI.MVVM._VM.Tooltip.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.UI.MVVM._PCView.Party;

namespace InspectEverything
{
    [HarmonyPatch(typeof(InGameInspectVM), nameof(InGameInspectVM.OnUnitHover))]
    public class InGameInspectVM_OnUnitHover_Patch
    {
        public static bool Prefix(InGameInspectVM __instance, UnitEntityView unitEntityView, bool isHover)
        {
            if (!Game.Instance.Player.UISettings.ShowInspect)
            {
                __instance.Tooltip.Value = null;
                return false;
            }

            UnitEntityData entityData = unitEntityView.EntityData;
            if (entityData != null && (!entityData.Group.IsPlayerParty || entityData.IsSummoned()))
            {
                UnitEntityData value = __instance.m_HoveredUnitReference.Value;
                if (value == null || !value.View.MouseHighlighted)
                {
                    __instance.m_HoveredUnitReference = (isHover ? unitEntityView.Data : null);
                    __instance.Tooltip.Value = (isHover ? new TooltipTemplateUnitInspect(unitEntityView.EntityData) : null);
                }
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(InGameInspectVM), nameof(InGameInspectVM.OnUnitRightClick))]
    public class InGameInspectVM_OnUnitRightClick_Patch
    {
        public static bool Prefix(InGameInspectVM __instance, UnitEntityView unitEntityView)
        {
            if (Game.Instance.Player.UISettings.ShowInspect)
            {
                __instance.m_HoveredUnitReference = unitEntityView.Data;
                TooltipHelper.ShowInfo(new TooltipTemplateUnitInspect(unitEntityView.EntityData));
            }

            return false;
        }
    }
}
