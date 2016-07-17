using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinesauce_ROM_Corrupter.Corruption
{
    static class Color
    {
        public static void Corrupt(
            string rawColorsToReplace, 
            string rawReplaceWithColors, 
            bool colorUseByteCorruptionRange,
            long startByte,
            long endByte
            ) { 
            // Read in the text and its replacement.
            string[] ColorsToReplace = rawColorsToReplace.Split(Client.Delimeter, StringSplitOptions.RemoveEmptyEntries);
            string[] ColorsReplaceWith = rawReplaceWithColors.Split(Client.Delimeter, StringSplitOptions.RemoveEmptyEntries);

                    // Make sure they have equal length.
                    if (ColorsToReplace.Length != ColorsReplaceWith.Length)
                    {
                        //TODO: MessageBox.Show("Number of colors to replace does not match number of replacements.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Convert the strings.
                    byte[] ColorsToReplaceBytes = new byte[ColorsToReplace.Length];
            byte[] ColorsReplaceWithBytes = new byte[ColorsReplaceWith.Length];
                    for (int i = 0; i<ColorsToReplace.Length; i++)
                    {
                        try
                        {
                            byte Converted = Convert.ToByte(ColorsToReplace[i], 16);
            ColorsToReplaceBytes[i] = Converted;
                        }
                        catch
                        {
                            //TODO: MessageBox.Show("Invalid color to replace.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    for (int i = 0; i<ColorsReplaceWithBytes.Length; i++)
                    {
                        try
                        {
                            byte Converted = Convert.ToByte(ColorsReplaceWith[i], 16);
    ColorsReplaceWithBytes[i] = Converted;
                        }
                        catch
                        {
                            //TODO: MessageBox.Show("Invalid color replacement.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Area of ROM to consider.
                    long ColorReplacementStartByte = 0;
    long ColorReplacementEndByte = Client.ROM.LongLength - 1;

                    // Change area if using the byte corruption range.
                    if (colorUseByteCorruptionRange)
                    {
                        ColorReplacementStartByte = startByte;
                        ColorReplacementEndByte = endByte;
                    }

                    // Position in ROM.
                    long j = ColorReplacementStartByte;

                    // Scan the entire ROM.
                    while (j <= ColorReplacementEndByte)
                    {
                        // If a palette has been found.
                        bool Palette = true;

                        // Look for a palette.
                        for (int k = 0; k< 4; k++)
                        {
                            // Make sure its in range.
                            if (j + k <= ColorReplacementEndByte)
                            {
                                // Check if value exceeds the maximum valid color value.
                                if (Client.ROM[j + k] > 0x3F)
                                {
                                    // It does, break.
                                    Palette = false;
                                    break;
                                }
                            }
                            else
                            {
                                // Out of range before matching.
                                Palette = false;
                                break;
                            }
                        }

                        // If a possible palette was found, do color replacement.
                        if (Palette)
                        {
                            for (int i = 0; i<ColorsToReplaceBytes.Length; i++)
                            {
                                for (int k = 0; k< 4; k++)
                                {
                                    if (Client.ROM[j + k] == ColorsToReplaceBytes[i])
                                    {
                                        // If the byte is protected.
                                        bool Protected = false;

                                        // Check if the byte is protected.
                                        foreach (long[] ProtectedRegion in Client.ProtectedRegions)
                                        {
                                            if (j + k >= ProtectedRegion[0] && j + k <= ProtectedRegion[1])
                                            {
                                                // Yes, its protected.
                                                Protected = true;
                                                break;
                                            }
                                        }

                                        // If its not protected, do the replacement.
                                        if (!Protected)
                                        {
                                        Client.ROM[j + k] = ColorsReplaceWithBytes[i];
                                            Client.ProtectedRegions.Add(new long[2] { j + k, j + k });
                                        }
                                    }
                                }
                            }

                            // Move ahead to the correct location in the ROM.
                            j = j + 4;
                        }
                        else
                        {
                            // Move ahead one byte.
                            j = j + 1;
                        }
                }
        }
    }
}
