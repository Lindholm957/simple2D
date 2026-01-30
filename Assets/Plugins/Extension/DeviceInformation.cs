using System.Linq;
using UnityEngine.Device;

public static class DeviceInformation
{
    public static bool IsTablet()
    {
#if UNITY_EDITOR
        return false;
#endif
        return SystemInfo.deviceName.Contains("iPad");
    }
}