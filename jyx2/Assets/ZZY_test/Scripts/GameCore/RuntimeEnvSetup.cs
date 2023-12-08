
using ZZY_test.MOD.ModV2;

namespace ZZY_test
{
    /// <summary>
    /// 游戏运行时的初始化
    /// </summary>
    public static class RuntimeEnvSetup
    {
        private static bool _isSetup;
        public static MODRootConfig CurrentModConfig { get; private set; } = null;
        private static GameModBase _currentMod;

        public static string CurrentModId => _currentMod?.Id;

        public static void SetCurrentMod(GameModBase mod)
        {
            _currentMod = mod;
        }

        public static GameModBase GetCurrentMod() => _currentMod;

        public static bool IsLoading { get; private set; } = false;

        public static void ForceClear()
        {
            _isSetup = false;
            CurrentModConfig = null;
            _currentMod = null;
            IsLoading = false;
            _successInited = false;
        }

        private static bool _successInited;
    }
}