﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Spelling.Drivers
{
    public static class DriverFactory
    {
        private const string HUNSPELL = "Hunspell";

        public static ISpellingDriver GetDriver(string SpellingEngine)
        {
            if (String.Equals(SpellingEngine, HUNSPELL, StringComparison.OrdinalIgnoreCase))
            {
                return SupportedDrivers.Hunspell.GetDriver();
            }
            throw new NotSupportedException("Only HunSpell are currently supported.");
        }

        public static ISpellingDriver GetDriver(this SupportedDrivers driver)
        {
            switch (driver)
            {
                case SupportedDrivers.Hunspell:
                    return new HunSpell();

                default:
                    throw new NotSupportedException("Unknown Driver");
            }
        }
    }
}