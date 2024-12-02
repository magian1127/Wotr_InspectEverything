using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Inspect;
using Kingmaker.UI.MVVM._VM.Inspect;
using Kingmaker.UI.MVVM._VM.Tooltip.Bricks;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Owlcat.Runtime.UI.Tooltips;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace InspectEverything
{
    [HarmonyPatch(typeof(TooltipTemplateUnitInspect), nameof(TooltipTemplateUnitInspect.GetBody))]
    public class TooltipTemplateUnitInspect_GetBody_Patch
    {
        public static bool Prefix(ref IEnumerable<ITooltipBrick> __result, TooltipTemplateUnitInspect __instance, TooltipTemplateType type)
        {
            if (__instance.Unit.Group.IsPlayerParty)
            {
                using (ProfileScope.New("TooltipTemplateUnitInspect GetBody"))
                {
                    List<ITooltipBrick> list = new List<ITooltipBrick>();

                    if (__instance.InspectInfo == null)
                    {
                        __result = list;
                        return false;
                    }

                    List<BlueprintAbility> list2;
                    List<BlueprintActivatableAbility> list3;
                    List<FeatureUIData> list4;
                    using (ProfileScope.New("TooltipTemplateUnitInspect GetBody Collect"))
                    {
                        list2 = __instance.InspectInfo.AbilitiesPart?.Abilities.ToList();
                        list3 = __instance.InspectInfo.AbilitiesPart?.ActivatableAbilities.ToList();
                        list4 = __instance.ClearFromDublicatedFeatures(__instance.InspectInfo.AbilitiesPart?.Features);
                    }

                    using (ProfileScope.New("TooltipTemplateUnitInspect GetBody Clear"))
                    {
                        __instance.ClearRaceAndFeatureSelections(list4);
                        __instance.ClearFromDublicatedFeaturesByAbilities(list4, list2, list3);
                        list2 = __instance.ClearAbilitiesGotFromFeat(list2, list4);
                        list3 = __instance.ClearActivatableAbilitiesGotFromFeat(list3, list4);
                    }

                    switch (type)
                    {
                        case TooltipTemplateType.Tooltip:
                            using (ProfileScope.New("TooltipTemplateUnitInspect GetBody TooltipType"))
                            {
                                __instance.AddAlignmentTypeRace(list, __instance.InspectInfo.BasePart);
                                __instance.AddClasses(list, __instance.InspectInfo.BasePart);
                                __instance.AddHP(list, __instance.InspectInfo.DefencePart, __instance.InspectInfo.BasePart);
                                __instance.AddSizeSpeedInitiative(list, __instance.InspectInfo.BasePart, type);
                                list.Add(new TooltipBrickSeparator());
                                list.Add(new TooltipBrickTitle(UIStrings.Instance.CharacterSheet.Defense, TooltipTitleType.H3, saberFormat: true));
                                __instance.AddArmorClass(list, __instance.InspectInfo.DefencePart, type);
                                __instance.AddSaves(list, __instance.InspectInfo.DefencePart, type);
                                __instance.AddSpellResist(list, __instance.InspectInfo.DefencePart);
                                __instance.AddDamageReduction(list, __instance.InspectInfo.DefencePart);
                                __instance.AddEnergyResistance(list, __instance.InspectInfo.DefencePart, type);
                                __instance.AddActiveBuffs(list, __instance.InspectInfo.ActiveBuffsPart, __instance.Unit, type);
                                __instance.AddImmunities(list, __instance.InspectInfo.DefencePart);
                                list.Add(new TooltipBrickSeparator());
                                list.Add(new TooltipBrickTitle(UIStrings.Instance.Tooltips.Offence, TooltipTitleType.H3, saberFormat: true));
                                __instance.AddAttacks(list, __instance.InspectInfo.OffencePart, type);
                                __instance.AddStats(list, __instance.InspectInfo.BasePart);
                                __instance.AddExperience(list, __instance.InspectInfo.BasePart);
                                list.Add(new TooltipBrickSeparator());
                                list.Add(new TooltipBrickTitle(UIStrings.Instance.CharacterSheet.FeaturesAndAbilitites, TooltipTitleType.H3, saberFormat: true));
                                __instance.AddAbilities(list, list2.ToArray(), type);
                                __instance.AddActivatableAbilities(list, list3.ToArray(), type);
                                __instance.AddFeatures(list, list4.ToArray(), type);
                                //__instance.AddSpells(list, __instance.InspectInfo.AbilitiesPart?.Spells, type);
                                __instance.AddSkills(list, __instance.InspectInfo.BasePart);
                                if (__instance.InspectInfo.IsEmpty)
                                {
                                    __instance.AddAxiomatic(list);
                                }
                            }

                            break;
                        case TooltipTemplateType.Info:
                            using (ProfileScope.New("TooltipTemplateUnitInspect GetBody InfoType"))
                            {
                                __instance.AddAlignmentTypeRace(list, __instance.InspectInfo.BasePart);
                                __instance.AddClasses(list, __instance.InspectInfo.BasePart);
                                __instance.AddHP(list, __instance.InspectInfo.DefencePart, __instance.InspectInfo.BasePart);
                                __instance.AddSizeSpeedInitiative(list, __instance.InspectInfo.BasePart, type);
                                __instance.AddStats(list, __instance.InspectInfo.BasePart);
                                __instance.AddExperience(list, __instance.InspectInfo.BasePart);
                                __instance.AddActiveBuffs(list, __instance.InspectInfo.ActiveBuffsPart, __instance.Unit, type);
                                list.Add(new TooltipBrickSeparator());
                                list.Add(new TooltipBrickTitle(UIStrings.Instance.CharacterSheet.Defense, TooltipTitleType.H3, saberFormat: true));
                                __instance.AddRegeneration(list, __instance.InspectInfo.DefencePart);
                                __instance.AddArmorClass(list, __instance.InspectInfo.DefencePart, type);
                                __instance.AddSaves(list, __instance.InspectInfo.DefencePart, type);
                                __instance.AddSpellResist(list, __instance.InspectInfo.DefencePart);
                                __instance.AddDamageReduction(list, __instance.InspectInfo.DefencePart);
                                __instance.AddEnergyResistance(list, __instance.InspectInfo.DefencePart, type);
                                __instance.AddImmunities(list, __instance.InspectInfo.DefencePart);
                                __instance.AddVulnerabilities(list, __instance.InspectInfo.DefencePart);
                                list.Add(new TooltipBrickSeparator());
                                list.Add(new TooltipBrickTitle(UIStrings.Instance.Tooltips.Offence, TooltipTitleType.H3, saberFormat: true));
                                __instance.AddAttacks(list, __instance.InspectInfo.OffencePart, type);
                                __instance.AddBab(list, __instance.InspectInfo.OffencePart, type);
                                list.Add(new TooltipBrickSeparator());
                                list.Add(new TooltipBrickTitle(UIStrings.Instance.CharacterSheet.FeaturesAndAbilitites, TooltipTitleType.H3, saberFormat: true));
                                __instance.AddAbilities(list, list2.ToArray(), type);
                                __instance.AddActivatableAbilities(list, list3.ToArray(), type);
                                __instance.AddFeatures(list, list4.ToArray(), type);
                                //__instance.AddSpells(list, __instance.InspectInfo.AbilitiesPart?.Spells, type);
                                __instance.AddSkills(list, __instance.InspectInfo.BasePart);
                                __instance.AddAxiomatic(list);
                            }

                            break;
                    }
                    __result = list;
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(TooltipTemplateUnitInspect), nameof(TooltipTemplateUnitInspect.Prepare))]
    public class TooltipTemplateUnitInspect_Prepare_Patch
    {
        public static bool Prefix(TooltipTemplateUnitInspect __instance, TooltipTemplateType type)
        {
            using (ProfileScope.New("TooltipTemplateUnitInspect Prepare"))
            {
                if (__instance.Unit != null)
                {
                    using (ProfileScope.New("TooltipTemplateUnitInspect Prepare ForceRevealUnitInfo"))
                    {
                        Game.Instance.Player.InspectUnitsManager.ForceRevealUnitInfo(__instance.Unit);
                    }

                    using (ProfileScope.New("TooltipTemplateUnitInspect Prepare GetInfo First"))
                    {
                        __instance.InspectInfo = InspectUnitsHelper.GetInfo(__instance.Unit.BlueprintForInspection, force: true, __instance.Unit);
                    }

                    using (ProfileScope.New("TooltipTemplateUnitInspect Prepare GetBuffs"))
                    {
                        __instance.Unit.Ensure<UnitPartInspectedBuffs>().GetBuffs(__instance.InspectInfo);
                    }
                }

                using (ProfileScope.New("TooltipTemplateUnitInspect Prepare GetInfo Second"))
                {
                    if (__instance.m_BlueprintUnit != null)
                    {
                        __instance.InspectInfo = InspectUnitsHelper.GetInfo(__instance.m_BlueprintUnit, force: true);
                    }
                }
            }
            return false;
        }
    }

}
