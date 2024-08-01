using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AtsEx.PluginHost.Handles;
using AtsEx.PluginHost.Native;
using AtsEx.PluginHost.Plugins;
using BveTypes.ClassWrappers;
using AtsEx.PluginHost.Panels.Native;
using AtsEx.PluginHost;
using System.Diagnostics;
using SlimDX;

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
        private readonly IAtsPanelValue<int> limitSpeed;
        private readonly IAtsPanelValue<int> limitSpeed_5;
        private double pattern;
        private double signalPattern;
        private double curvePattern;
        private double distance;
        private double maxSpeed;
        private double deceleration;
        private bool isCurvePattern;
        private double targetLimit;
        private double targetDistance;
        private bool B;
        private int optional;
        private int type;
        private SectionManager sectionManager;
        public SectionManager SectionManager { get => sectionManager; set => sectionManager = value; }
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);
        /// <summary>
        /// プラグインが読み込まれた時に呼ばれる
        /// 初期化を実装する
        /// </summary>
        /// <param name="builder"></param>
        public VehiclePluginMain(PluginBuilder builder) : base(builder)
        {

            var path = new FileInfo(Location);
            int capacitySize = 256;
            deceleration = 3.0;
            curvePattern = 120;
            StringBuilder sb = new StringBuilder(capacitySize);
            uint ret = GetPrivateProfileString("Data", "Deceleration", "none", sb, Convert.ToUInt32(sb.Capacity), $@"{path.DirectoryName}\D-ATC.ini");
            if(0 < ret)
            {
                deceleration = double.Parse(sb.ToString());
            }
            Native.BeaconPassed += new BeaconPassedEventHandler(BeaconPassed);
            BveHacker.ScenarioCreated += ScenarioCreated;
            limitSpeed = Native.AtsPanelValues.RegisterInt32(67);
            limitSpeed_5 = Native.AtsPanelValues.RegisterInt32(66);
        }

        private void ScenarioCreated(ScenarioCreatedEventArgs e)
        {
            sectionManager = e.Scenario.SectionManager;
        }

        /// <summary>
        /// プラグインが解放されたときに呼ばれる
        /// 後処理を実装する
        /// </summary>
        public override void Dispose()
        {
            Native.BeaconPassed -= BeaconPassed;
            BveHacker.ScenarioCreated -= ScenarioCreated;
            limitSpeed.Dispose();
            limitSpeed_5.Dispose();
        }

        public void BeaconPassed(BeaconPassedEventArgs e)
        {
            type = e.Type;
            optional = e.Optional;
            switch (e.Type)
            {
                case 211:
                    maxSpeed = e.Optional;
                    isCurvePattern = false;
                    curvePattern = maxSpeed;
                    break;
                case 212:
                    isCurvePattern = true;
                    string optional = e.Optional.ToString();
                    targetDistance = int.Parse(optional.Substring(0, optional.Length - 3));
                    targetLimit = int.Parse(optional.Substring(optional.Length - 3, 3));
                    Debug.WriteLine(targetDistance + "&" + targetLimit);
                    B = true;
                    break;
                case 213:
                    break;
                case 214:
                    break;
            }
        }
        /// <summary>
        /// シナリオ読み込み中に毎フレーム呼び出される
        /// </summary>
        /// <param name="elapsed">前回フレームからの経過時間</param>
        public override TickResult Tick(TimeSpan elapsed)
        {
            Beacon FindClosestBeacon(Func<Beacon, bool> predicate)
            {
                MapObjectList beacons = BveHacker.Scenario.Route.Beacons;
                Beacon closestBeacon = null;
                double closestLocationDiff = double.MaxValue;

                foreach (Beacon beacon in beacons.Cast<Beacon>())
                {
                    double locationDiff = Native.VehicleState.Location - beacon.Location;
                    if (locationDiff >= 0 && locationDiff < closestLocationDiff && predicate(beacon))
                    {
                        closestLocationDiff = locationDiff;
                        closestBeacon = beacon;
                    }
                }

                if (closestBeacon == null)
                    throw new InvalidOperationException("条件に合致するビーコンが見つかりませんでした。");
                return closestBeacon;
            }
            if (B)
            {
                Beacon closestBeacon = FindClosestBeacon(beacon => beacon.Type == type && beacon.SendData == optional);
                targetDistance = targetDistance + closestBeacon.Location;
                B = false;
            }
            int brake = Native.Handles.Brake.Notch;
            int targetSectionIndex = sectionManager.StopSignalSectionIndexes[0];
            double limitDistance;
            VehiclePluginTickResult ret = new VehiclePluginTickResult();
            distance = sectionManager.Sections[targetSectionIndex].Location;
            limitDistance = distance - 10 - Native.VehicleState.Location;
            //Debug.WriteLine(limitDistance);
            signalPattern = CalculatePattern(deceleration, limitDistance, 0);
            if(isCurvePattern)
            {
                double pattern = CalculatePattern(deceleration, targetDistance - Native.VehicleState.Location, targetLimit);
                curvePattern = Math.Max(double.IsNaN(pattern) ? 0 : pattern, targetLimit);
                Debug.WriteLine(curvePattern);
            }
            pattern = Math.Min(120, Math.Min(signalPattern, curvePattern));
            if (Native.VehicleState.Speed > pattern)
            {
                brake = Native.Handles.Brake.MaxServiceBrakeNotch;
            }
            limitSpeed.Value = (int)pattern;
            limitSpeed_5.Value = (int)Math.Round(pattern / 5) * 5;
            //Debug.WriteLine(targetLimit);
            //Debug.WriteLine(curvePattern);
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
