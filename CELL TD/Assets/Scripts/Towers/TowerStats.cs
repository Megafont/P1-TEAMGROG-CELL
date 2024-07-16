using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Towers
{
    /// <summary>
    /// This enum is used to identify a tower stat in the upgrade system.
    /// </summary>
    /// <remnarks>
    ///
    /// IMPORTANT: When you add an entry to this enum, make sure you also add it
    ///            to the TowerStatsExtensions class's lookup table below this class as well!
    ///            
    /// </remnarks>
    public enum TowerStats
    {
        // Stats Common to All Tower Types
        // ----------------------------------------------------------------------------------------------------
        MaxHealth = 0,
        DamageValue,
        FireRate,
        Range,
        NumberOfTargets,
        RefundPercentage,
        UpgradeCost,
        UnitAmount,
        UnitDamage,
        UnitHealth,
        EffectStrength,
    }



    /// <summary>
    /// This class contains extension methods for the TowerStats enum.
    /// </summary>
    public static class TowerStatsExtensions
    {
        /// <summary>
        /// This dictionary is just a simple lookup table that lets us easily get the
        /// display name for a given stat.
        /// </summary>
        private static Dictionary<TowerStats, string> _StatsNameLookup = new Dictionary<TowerStats, string>
        {
            // Stats Common to All Tower Types
            // ----------------------------------------------------------------------------------------------------
            { TowerStats.MaxHealth,         "Max Health" },
            { TowerStats.DamageValue,       "Damage" },
            { TowerStats.FireRate,          "Fire Rate" },
            { TowerStats.Range,             "Range" },
            { TowerStats.NumberOfTargets,   "Number of Targets" },
            { TowerStats.UpgradeCost,       "Upgrade Cost" },
            { TowerStats.RefundPercentage,  "Refund Percentage" },

            { TowerStats.UnitAmount,        "Amount of Units to Spawn" },
            { TowerStats.UnitDamage,        "Amount of Damage from Units" },
            { TowerStats.UnitHealth,        "Amount of Health per Unit" },

            { TowerStats.EffectStrength,    "Controls Effect Strength"},
        };



        /// <summary>
        /// Returns the display name for the specified stat.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public static string GetStatName(this TowerStats stat)
        {
            return _StatsNameLookup[stat];
        }
    }

}
