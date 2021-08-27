using System;
using System.Runtime.InteropServices;

namespace Psps.Core.Helper
{
    public static class LcMap
    {
        /// <summary>
        /// 使用系統 kernel32.dll 進行轉換
        /// </summary>
        private const int LocaleSystemDefault = 0x0800;

        private const int LcmapSimplifiedChinese = 0x02000000;

        private const int LcmapTraditionalChinese = 0x04000000;

        public static string ToSimplified(string argSource)
        {
            var t = new String(' ', argSource.Length);

            LCMapString(LocaleSystemDefault, LcmapSimplifiedChinese, argSource, argSource.Length, t, argSource.Length);

            return t;
        }

        public static string ToTraditional(string argSource)
        {
            var t = new String(' ', argSource.Length);

            LCMapString(LocaleSystemDefault, LcmapTraditionalChinese, argSource, argSource.Length, t, argSource.Length);

            return t;
        }

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);
    }
}