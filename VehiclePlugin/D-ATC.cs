using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AtsEx.PluginHost.Handles;
using AtsEx.PluginHost.Native;
using AtsEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;

namespace AtsExCsTemplate.VehiclePlugin
{
    /// <summary>
    /// プラグインの本体
    /// Plugin() の第一引数でこのプラグインの仕様を指定
    /// Plugin() の第二引数でこのプラグインが必要とするAtsEX本体の最低バージョンを指定（オプション）
    /// </summary>
    [Plugin(PluginType.VehiclePlugin)]
    internal class VehiclePluginMain : AssemblyPluginBase
    {
        private double pattern;
        private double signalPattern;
        private double curvePattern;
        private double distance;
        private double speed;
        private double maxSpeed;
        private double deceleration;
        private SectionManager sectionManager;
        public SectionManager SectionManager { get => sectionManager; set => sectionManager = value; }
        /// <summary>
        /// プラグインが読み込まれた時に呼ばれる
        /// 初期化を実装する
        /// </summary>
        /// <param name="builder"></param>
        public VehiclePluginMain(PluginBuilder builder) : base(builder)
        {
            maxSpeed = 120;
        }

        /// <summary>
        /// プラグインが解放されたときに呼ばれる
        /// 後処理を実装する
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// シナリオ読み込み中に毎フレーム呼び出される
        /// </summary>
        /// <param name="elapsed">前回フレームからの経過時間</param>
        public override TickResult Tick(TimeSpan elapsed)
        {
            int brake = Native.Handles.Brake.Notch;
            int targetSectionIndex = sectionManager.StopSignalSectionIndexes[0];
            double limitDistance;
            VehiclePluginTickResult ret = new VehiclePluginTickResult();
            distance = sectionManager.Sections[targetSectionIndex].Location;
            limitDistance = distance - 10 - Native.VehicleState.Location;
            signalPattern = CalculatePattern(-3, limitDistance, 0);
            pattern = Math.Min(signalPattern, curvePattern);
            if (Native.VehicleState.Speed > pattern)
            {
                brake = Native.Handles.Brake.MaxServiceBrakeNotch;
            }
            ret.HandleCommandSet = new HandleCommandSet(
            Native.Handles.Power.GetCommandToSetNotchTo(Native.Handles.Power.Notch),
            Native.Handles.Brake.GetCommandToSetNotchTo(brake),
            ReverserPositionCommandBase.Continue,
            ConstantSpeedCommand.Continue);
            return ret;
        }
        double CalculatePattern(double deceleration, double targetDistance, double targetSpeed)
        {
            return 3.6 * (Math.Sqrt((2 * (deceleration / 3.6) * targetDistance) + Math.Pow((targetSpeed / 3.6), 2)));
        }
    }
}
