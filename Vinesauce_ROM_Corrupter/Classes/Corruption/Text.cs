using System;
using System.Collections.Generic;
using System.Linq;

/* 
 * Copyright (C) 2013 Ryan Sammon.
 * 
 * This file is part of the Vinesauce ROM Corruptor.
 * 
 * The Vinesauce ROM Corruptor is free software: you can redistribute
 * it and/or modify it under the terms of the GNU General Public 
 * License as published by the Free Software Foundation, either 
 * version 3 of the License, or (at your option) any later version.
 * 
 * The Vinesauce ROM Corruptor is distributed in the hope that it
 * will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 * See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with the Vinesauce ROM Corruptor.  If not, see 
 * <http://www.gnu.org/licenses/>.
 */

/* Made into some #cool #class by doomy */

namespace Vinesauce_ROM_Corrupter.Corruption
{
   static class Text
    {
        
        public static void Replace(
            string RawTextToReplace, 
            string RawReplaceWith, 
            string RawAnchorWords, 
            bool TextUseByteCorruptionRange)
        {
            // Read in the text and its replacement.
            // Instead of using an array, we split the text using a specified delimiter
            // TODO: use a list of text replacements instead of one big box
            string[] TextToReplace = RawTextToReplace.Split(Client.Delimeter, StringSplitOptions.RemoveEmptyEntries);
            string[] ReplaceWith = RawReplaceWith.Split(Client.Delimeter, StringSplitOptions.RemoveEmptyEntries);

            // Make sure they have equal length - since we're passing in just plain text before converting to an array,
            // the options to replace need to match our options to replace them with.
            // e.g. "replace","me" and "this","cool" would work, since they are symmetrical.
            if (TextToReplace.Length != ReplaceWith.Length)
            {
                //TODO: MessageBox.Show("Number of text sections to replace does not match number of replacements.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Error!  Although we will later make sure this error never happens by forcing the user to 
                // always choose a replacement for their initial text, for now we'll just exit the function
                // and throw an error message.
                return;
            }

            // Create relative offset arrays of the anchors.
            // doomy: not really sure what this is doing
            string[] Anchors = RawAnchorWords.Split(Client.Delimeter, StringSplitOptions.RemoveEmptyEntries);
            int[][] RelativeAnchors = new int[Anchors.Length][];
            for (int i = 0; i < Anchors.Length; i++)
            {
                RelativeAnchors[i] = new int[Anchors[i].Length];
                for (int j = 0; j < Anchors[i].Length; j++)
                {
                    RelativeAnchors[i][j] = Anchors[i][j] - Anchors[i][0];
                }
            }

            // Look for the anchors.
            // doomy: yeah this is definitely doing something important but i guess ill figure it out later
            for (int i = 0; i < RelativeAnchors.Length; i++)
            {
                // Position in ROM.
                long j = 0;

                // Scan the entire ROM.
                while (j < Client.ROM.LongLength)
                {
                    // If a match has been found.
                    bool Match = true;

                    // Look for the relative values.
                    for (int k = 0; k < RelativeAnchors[i].Length; k++)
                    {
                        // Make sure its in range.
                        if (j + k < Client.ROM.LongLength)
                        {
                            // Ignore non-letter characters for matching purposes.
                            if (!Char.IsLetter(Anchors[i][k]))
                            {
                                continue;
                            }

                            // Check if the relative value doesn't match.
                            if ((Client.ROM[j + k] - Client.ROM[j]) != RelativeAnchors[i][k])
                            {
                                // It doesn't, break.
                                Match = false;
                                break;
                            }
                        }
                        else
                        {
                            // Out of range before matching.
                            Match = false;
                            break;
                        }
                    }

                    // If a match was found, update the dictionary.
                    if (Match)
                    {
                        int k = 0;
                        for (k = 0; k < Anchors[i].Length; k++)
                        {
                            if (!Client.TranslationDictionary.ContainsKey(Anchors[i][k]))
                            {
                                Client.TranslationDictionary.Add(Anchors[i][k], Client.ROM[j + k]);
                            }
                        }

                        // Move ahead to the correct location in the ROM.
                        j = j + k + 1;
                    }
                    else
                    {
                        // Move ahead one byte.
                        j = j + 1;
                    }
                }
            }

            // Calculate the offset to translate unknown text, assuming ASCII structure.
            int ASCIIOffset = 0;
            if (Client.TranslationDictionary.Count > 0)
            {
                ASCIIOffset = Client.TranslationDictionary.First().Value - Client.TranslationDictionary.First().Key;
            }

            // Create arrays of the text to be replaced in ROM format.
            byte[][] ByteTextToReplace = new byte[TextToReplace.Length][];
            for (int i = 0; i < TextToReplace.Length; i++)
            {
                ByteTextToReplace[i] = new byte[TextToReplace[i].Length];
                for (int j = 0; j < TextToReplace[i].Length; j++)
                {
                    if (Client.TranslationDictionary.ContainsKey(TextToReplace[i][j]))
                    {
                        ByteTextToReplace[i][j] = Client.TranslationDictionary[TextToReplace[i][j]];
                    }
                    else
                    {
                        int ASCIITranslated = TextToReplace[i][j] + ASCIIOffset;
                        if (ASCIITranslated >= Byte.MinValue && ASCIITranslated <= Byte.MaxValue)
                        {
                            ByteTextToReplace[i][j] = (byte)(ASCIITranslated);
                        }
                        else
                        {
                            // Could not translate.
                            ByteTextToReplace[i][j] = (byte)(TextToReplace[i][j]);
                        }
                    }
                }
            }

            // Create arrays of the replacement text in ROM format.
            byte[][] ByteReplaceWith = new byte[ReplaceWith.Length][];
            for (int i = 0; i < ReplaceWith.Length; i++)
            {
                ByteReplaceWith[i] = new byte[ReplaceWith[i].Length];
                for (int j = 0; j < ReplaceWith[i].Length; j++)
                {
                    if (Client.TranslationDictionary.ContainsKey(ReplaceWith[i][j]))
                    {
                        ByteReplaceWith[i][j] = Client.TranslationDictionary[ReplaceWith[i][j]];
                    }
                    else
                    {
                        int ASCIITranslated = ReplaceWith[i][j] + ASCIIOffset;
                        if (ASCIITranslated >= Byte.MinValue && ASCIITranslated <= Byte.MaxValue)
                        {
                            ByteReplaceWith[i][j] = (byte)(ASCIITranslated);
                        }
                        else
                        {
                            // Could not translate.
                            ByteReplaceWith[i][j] = (byte)(ReplaceWith[i][j]);
                        }
                    }
                }
            }

            // Area of ROM to consider.
            long TextReplacementStartByte = 0;
            long TextReplacementEndByte = Client.ROM.LongLength - 1;

            // Change area if using the byte corruption range.
            if (TextUseByteCorruptionRange)
            {
                TextReplacementStartByte = Client.StartByte;
                TextReplacementEndByte = Client.EndByte;
            }

            // Look for the text to replace.
            for (int i = 0; i < ByteTextToReplace.Length; i++)
            {
                // Position in ROM.
                long j = TextReplacementStartByte;

                // Scan the entire ROM.
                while (j <= TextReplacementEndByte)
                {
                    // If a match has been found.
                    bool Match = true;

                    // Look for the text.
                    for (int k = 0; k < ByteTextToReplace[i].Length; k++)
                    {
                        // Make sure its in range.
                        if (j + k <= TextReplacementEndByte)
                        {
                            // Ignore non-letter characters for matching purposes.
                            if (!Char.IsLetter(TextToReplace[i][k]))
                            {
                                continue;
                            }

                            // Check if the relative value doesn't match.
                            if (Client.ROM[j + k] != ByteTextToReplace[i][k])
                            {
                                // It doesn't, break.
                                Match = false;
                                break;
                            }
                        }
                        else
                        {
                            // Out of range before matching.
                            Match = false;
                            break;
                        }
                    }

                    // If the entire string matched, replace it.
                    if (Match)
                    {
                        // If the area is protected.
                        bool Protected = false;

                        // Length of the replacement.
                        int k = ByteReplaceWith[i].Length - 1;

                        // Check if the area is protected.
                        foreach (long[] ProtectedRegion in Client.ProtectedRegions)
                        {
                            if ((j >= ProtectedRegion[0] && j <= ProtectedRegion[1]) || (j + k >= ProtectedRegion[0] && j + k <= ProtectedRegion[1]) || (j < ProtectedRegion[0] && j + k > ProtectedRegion[1]))
                            {
                                // Yes, its protected.
                                Protected = true;
                                break;
                            }
                        }

                        // If not protected, replace the text.
                        if (!Protected)
                        {
                            for (k = 0; k < ByteReplaceWith[i].Length; k++)
                            {
                                Client.ROM[j + k] = ByteReplaceWith[i][k];
                            }

                            // Protect the inserted text.
                            Client.ProtectedRegions.Add(new long[2] { j, j + k });
                        }

                        // Move ahead to the correct location in the ROM.
                        j = j + k + 1;
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
}
