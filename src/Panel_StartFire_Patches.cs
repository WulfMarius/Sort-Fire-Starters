using Harmony;
using System.Collections.Generic;

namespace SortFireStarters
{
    [HarmonyPatch(typeof(Panel_FireStart), "RefreshList")]
    internal class Panel_StartFire_RefreshList
    {
        internal static void Postfix(List<GearItem> gearList, FireStartMaterialType type)
        {
            if (type == FireStartMaterialType.FireStarter)
            {
                try
                {
                    gearList.Sort(CompareFireStarters);
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        private static int CompareFireStarters(GearItem item1, GearItem item2)
        {
            if (item1.m_TorchItem?.IsBurning() == true && item2.m_TorchItem?.IsBurning() != true)
            {
                return -1;
            }

            if (item1.m_TorchItem?.IsBurning() != true && item2.m_TorchItem?.IsBurning() == true)
            {
                return 1;
            }

            if (item1.m_FlareItem?.IsBurning() == true && item2.m_FlareItem?.IsBurning() != true)
            {
                return -1;
            }

            if (item1.m_FlareItem?.IsBurning() != true && item2.m_FlareItem?.IsBurning() == true)
            {
                return 1;
            }

            FireStarterItem fireStarter1 = item1.m_FireStarterItem;
            FireStarterItem fireStarter2 = item2.m_FireStarterItem;

            if (!fireStarter1.m_RequiresSunLight && fireStarter2.m_RequiresSunLight)
            {
                return -1;
            }

            if (fireStarter1.m_RequiresSunLight && !fireStarter2.m_RequiresSunLight)
            {
                return 1;
            }

            int chanceComparison = fireStarter1.m_FireStartSkillModifier.CompareTo(fireStarter2.m_FireStartSkillModifier);
            if (chanceComparison != 0)
            {
                return -chanceComparison;
            }

            return item1.m_DisplayName.CompareTo(item2.m_DisplayName);
        }
    }
}