using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// <summary>
    /// Setting that determines what type of corruption we will perform on the ROM
    /// </summary>
    public enum ByteCorruptionOptions
    {
        AddXToByte,
        ShiftRightXBytes,
        ReplaceByteXwithY
    }

    public static class Client
    {

        /// <summary>
        /// Some Magic numbers that prevent the NES from locking up
        /// Presumably, the following protect certain bytes 
        /// </summary>
        static internal List<byte> NESCPUJamProtection_Avoid = new List<byte>() { 0x48, 0x08, 0x68, 0x28, 0x78, 0x00, 0x02, 0x12, 0x22, 0x32, 0x42, 0x52, 0x62, 0x72, 0x92, 0xB2, 0xD2, 0xF2 };
        static internal List<byte> NESCPUJamProtection_Protect_1 = new List<byte>() { 0x48, 0x08, 0x68, 0x28, 0x78, 0x40, 0x60, 0x00, 0x90, 0xB0, 0xF0, 0x30, 0xD0, 0x10, 0x50, 0x70, 0x4C, 0x6C, 0x20 };
        static internal List<byte> NESCPUJamProtection_Protect_2 = new List<byte>() { 0x90, 0xB0, 0xF0, 0x30, 0xD0, 0x10, 0x50, 0x70, 0x4C, 0x6C, 0x20 };
        static internal List<byte> NESCPUJamProtection_Protect_3 = new List<byte>() { 0x4C, 0x6C, 0x20 };

        static internal long StartByte { set; get; }
        static internal long EndByte { set; get; }
        static internal List<long[]> ProtectedRegions { set; get; }

        static internal string RawTextToReplace { get; set; }
        static internal string RawReplaceWith { get; set; }
        static internal string RawAnchorWords { get; set; }
        static internal bool TextUseByteCorruptionRange { get; set; }

        static internal bool ByteCorruptionEnable { get; set; }
        static internal ByteCorruptionOptions ByteCorruptionOption  { get; set; }
        static internal uint EveryNthByte                           { get; set; }
        static internal int AddXtoByte                              { get; set; }
        static internal int ShiftRightXBytes                        { get; set; }
        static internal byte ReplaceByteXwithYByteX                 { get; set; }
        static internal byte ReplaceByteXwithYByteY                 { get; set; }
        static internal bool EnableNESCPUJamProtection              { get; set; }
        static internal bool TextReplacementEnable                  { get; set; }
        static internal bool ColorReplacementEnable                 { get; set; }
        static internal bool ColorUseByteCorruptionRange            { get; set; }
        static internal string RawColorsToReplace                   { get; set; }
        static internal string RawReplaceWithColors                 { get; set; }

        // The ROM itself!  We will modify it as time goes on.
        static internal byte[] ROM { get; set; }

        // Translation dictionary.
        static internal Dictionary<char, byte> TranslationDictionary { get; set; }

        // Delimeter for text sections.
        // It's always the same and will soon be removed so we'll just define it here.
        internal static char[] Delimeter = new char[1] { '|' };

        /// <summary>
        /// To keep classes simple and contained, our Client object will hold a lot of settings we will use
        /// for corruption.  We will set them here.  This method should be called before any other corruption
        /// methods are used.  After the initial setup, values can be added on a one-per basis if wanted.  The main thing
        /// is that we gotta get some values in here.
        /// </summary>       
        public static void Setup(
            long startByte, 
            long endByte, 
            string rawTextToReplace, 
            string rawReplaceWith,
            string rawAnchorWords,
            bool textUseByteCorruptionRange,
            bool byteCorruptionEnable, 
            ByteCorruptionOptions byteCorruptionOption,
            uint everyNthByte, 
            int addXtoByte, 
            int shiftRightXBytes, 
            byte replaceByteXwithYByteX, 
            byte replaceByteXwithYByteY, 
            bool enableNESCPUJamProtection,
            bool textReplacementEnable,
            bool colorReplacementEnable, 
            bool colorUseByteCorruptionRange, 
            string rawColorsToReplace, 
            string rawReplaceWithColors)
        {
            StartByte = startByte;
            EndByte = endByte;

            RawTextToReplace = rawTextToReplace;
            RawReplaceWith = rawReplaceWith;
            RawAnchorWords = rawAnchorWords;
            TextUseByteCorruptionRange = textUseByteCorruptionRange;

            // Areas to not corrupt.
            ProtectedRegions = new List<long[]>();

            // initialize our Translation Dictionary
            TranslationDictionary = new Dictionary<char, byte>();
        }


        async public static Task<byte[]> CorruptAsync()
        {
            await Task.Run(() =>
            {
                // Do text replacement if desired.
                if (TextReplacementEnable)
                    Text.Replace(RawTextToReplace, RawReplaceWith, RawAnchorWords, TextUseByteCorruptionRange);

                // Do color replacement if desired.
                if (ColorReplacementEnable)
                    Color.Corrupt(RawColorsToReplace, RawReplaceWithColors, ColorUseByteCorruptionRange, StartByte, EndByte);

                // Do byte corruption if desired.
                if (ByteCorruptionEnable)
                    RByte.Corrupt(ByteCorruptionOption, AddXtoByte, StartByte, EveryNthByte, ShiftRightXBytes, ReplaceByteXwithYByteX, ReplaceByteXwithYByteY, EndByte);
            });
            
            // return ROM when finished
            return Client.ROM;
        }
    }

}
