using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinesauce_ROM_Corrupter.Corruption
{
    class RByte
    {
        public static void Corrupt(
            ByteCorruptionOptions byteCorruptionOption, 
            int addXtoByte,
            long startByte,
            uint everyNthByte,
            int shiftRightXBytes,
            byte replaceByteXwithYByteX,
            byte replaceByteXwithYByteY,
            long endByte)
        {
            if (byteCorruptionOption == ByteCorruptionOptions.AddXToByte && addXtoByte != 0)
            {
                for (long i = startByte + everyNthByte; i <= endByte; i = i + everyNthByte)
                {
                    // If the byte is protected.
                    bool Protected = false;

                    // Check if the byte is protected.
                    foreach (long[] ProtectedRegion in Client.ProtectedRegions)
                    {
                        if (i >= ProtectedRegion[0] && i <= ProtectedRegion[1])
                        {
                            // Yes, its protected.
                            Protected = true;
                            break;
                        }
                    }

                    // Do NES CPU jam protection if desired.
                    if (Client.EnableNESCPUJamProtection)
                    {
                        if (!Protected && i >= 2)
                        {
                            if (Client.NESCPUJamProtection_Avoid.Contains((byte)((Client.ROM[i] + addXtoByte) % (Byte.MaxValue + 1)))
                                || Client.NESCPUJamProtection_Protect_1.Contains(Client.ROM[i])
                                || Client.NESCPUJamProtection_Protect_2.Contains(Client.ROM[i - 1])
                                || Client.NESCPUJamProtection_Protect_3.Contains(Client.ROM[i - 2]))
                            {
                                Protected = true;
                            }
                        }
                    }

                    // If the byte is not protected, corrupt it.
                    if (!Protected)
                    {
                        int NewValue = (Client.ROM[i] + addXtoByte) % (Byte.MaxValue + 1);
                        Client.ROM[i] = (byte)NewValue;
                    }
                }
            }
            else if (byteCorruptionOption == ByteCorruptionOptions.ShiftRightXBytes && shiftRightXBytes != 0)
            {
                for (long i = startByte + everyNthByte; i <= endByte; i = i + everyNthByte)
                {
                    long j = i + shiftRightXBytes;

                    if (j >= startByte && j <= endByte)
                    {
                        // If the byte is protected.
                        bool Protected = false;

                        // Check if the byte is protected.
                        foreach (long[] ProtectedRegion in Client.ProtectedRegions)
                        {
                            if (j >= ProtectedRegion[0] && j <= ProtectedRegion[1])
                            {
                                // Yes, its protected.
                                Protected = true;
                                break;
                            }
                        }

                        // Do NES CPU jam protection if desired.
                        if (Client.EnableNESCPUJamProtection)
                        {
                            if (!Protected && j >= 2)
                            {
                                if (Client.NESCPUJamProtection_Avoid.Contains(Client.ROM[i])
                                    || Client.NESCPUJamProtection_Protect_1.Contains(Client.ROM[j])
                                    || Client.NESCPUJamProtection_Protect_2.Contains(Client.ROM[j - 1])
                                    || Client.NESCPUJamProtection_Protect_3.Contains(Client.ROM[j - 2]))
                                {
                                    Protected = true;
                                }
                            }
                        }

                        // If the byte is not protected, corrupt it.
                        if (!Protected)
                        {
                            Client.ROM[j] = Client.ROM[i];
                        }
                    }
                }
            }
            else if (byteCorruptionOption == ByteCorruptionOptions.ReplaceByteXwithY && replaceByteXwithYByteX != replaceByteXwithYByteY)
            {
                for (long i = startByte + everyNthByte; i <= endByte; i = i + everyNthByte)
                {
                    if (Client.ROM[i] == replaceByteXwithYByteX)
                    {
                        // If the byte is protected.
                        bool Protected = false;

                        // Check if the byte is protected.
                        foreach (long[] ProtectedRegion in Client.ProtectedRegions)
                        {
                            if (i >= ProtectedRegion[0] && i <= ProtectedRegion[1])
                            {
                                // Yes, its protected.
                                Protected = true;
                                break;
                            }
                        }

                        // If the byte is not protected, corrupt it.
                        if (!Protected)
                        {
                            Client.ROM[i] = replaceByteXwithYByteY;
                        }
                    }
                }
            }
        }
    }
}
