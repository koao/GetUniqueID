using System;
using System.Management;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            const string drive = "F:";

            Console.WriteLine("Drive {0}'s Model Number is {1}", drive, GetModelFromDrive(drive));
            Console.WriteLine("Drive {0}'s GetSerialNumber is {1}", drive, GetSerialNumber(drive));
        }

        public static string GetModelFromDrive(string driveLetter)
        {
            // Must be 2 characters long.
            // Function expects "C:" or "D:" etc...
            if (driveLetter.Length != 2)
                return "";

            try
            {
                using (var partitions = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='" + driveLetter +
                                                 "'} WHERE ResultClass=Win32_DiskPartition"))
                {
                    foreach (var partition in partitions.Get())
                    {
                        using (var drives = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" +
                                                             partition["DeviceID"] +
                                                             "'} WHERE ResultClass=Win32_DiskDrive"))
                        {
                            foreach (var drive in drives.Get())
                            {
                                return (string)drive["Model"];
                            }
                        }
                    }
                }
            }
            catch
            {
                return "<unknown>";
            }

            // Not Found
            return "<unknown>";
        }

        public static string GetSerialNumber(string driveLetter)
        {
            if (driveLetter.Length != 2)
                return "";

            try
            {
                using (var partitions = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='" + driveLetter +
                                                 "'} WHERE ResultClass=Win32_DiskPartition"))
                {
                    foreach (var partition in partitions.Get())
                    {
                        using (var drives = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" +
                                                             partition["DeviceID"] +
                                                             "'} WHERE ResultClass=Win32_DiskDrive"))
                        {
                            foreach (var drive in drives.Get())
                            {
                                return (string)drive["SerialNumber"];
                            }
                        }
                    }
                }
            }
            catch
            {
                return "<unknown>";
            }

            // Not Found
            return "<unknown>";
        }
    }
}
