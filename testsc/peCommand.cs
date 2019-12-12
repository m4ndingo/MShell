using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace testsc
{
    class peCommand : CoreCommand
    {
        public override string Help(params string[] help_args)
        {
            return "Parses PE file";
        }
        public override void Run()
        {
            parsePEbytes(last_message);
        }
        private void parsePEbytes(string sBytes)
        {
            byte[] bytes = Encoding.Default.GetBytes(sBytes);

            List<string> results = new List<string>();
            
            // IMAGE_DOS_HEADER
            IMAGE_DOS_HEADER dosHeader = BytesToStructure<IMAGE_DOS_HEADER>(bytes);
            results.Add("IMAGE_DOS_HEADER.e_magic;" + dosHeader.e_magic + ";0x" + dosHeader.e_magic.ToString("x") + ";Magic number");
            results.Add("IMAGE_DOS_HEADER.e_cblp;" + dosHeader.e_cblp + ";;Bytes on last page of file");
            results.Add("IMAGE_DOS_HEADER.e_cp;" + dosHeader.e_cp + ";;Relocations");
            results.Add("IMAGE_DOS_HEADER.e_crlc;" + dosHeader.e_crlc + ";;Bytes on last page of file");
            results.Add("IMAGE_DOS_HEADER.e_cparhdr;" + dosHeader.e_cparhdr + ";;Size of header in paragraphs");
            results.Add("IMAGE_DOS_HEADER.e_minalloc;" + dosHeader.e_minalloc + ";;Minimum extra paragraphs needed");
            results.Add("IMAGE_DOS_HEADER.e_maxalloc;" + dosHeader.e_maxalloc + ";;Maximum extra paragraphs needed");
            results.Add("IMAGE_DOS_HEADER.e_ss;" + dosHeader.e_ss + ";;Initial (relative) SS value");
            results.Add("IMAGE_DOS_HEADER.e_sp;" + dosHeader.e_sp + ";;Initial SP value");
            results.Add("IMAGE_DOS_HEADER.e_csum;" + dosHeader.e_csum + ";;Checksum");
            results.Add("IMAGE_DOS_HEADER.e_ip;" + dosHeader.e_ip + ";;Initial IP value");
            results.Add("IMAGE_DOS_HEADER.e_cs;" + dosHeader.e_cs + ";;Initial (relative) CS value");
            results.Add("IMAGE_DOS_HEADER.e_lfarlc;" + dosHeader.e_lfarlc + ";;File address of relocation table");
            results.Add("IMAGE_DOS_HEADER.e_ovno;" + dosHeader.e_ovno + ";;Overlay number");
            results.Add("IMAGE_DOS_HEADER.e_res_0;" + dosHeader.e_res_0 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res_1;" + dosHeader.e_res_1 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res_2;" + dosHeader.e_res_2 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res_3;" + dosHeader.e_res_3 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_oemid;" + dosHeader.e_oemid + ";;OEM identifier (for e_oeminfo)");
            results.Add("IMAGE_DOS_HEADER.e_oeminfo;" + dosHeader.e_oeminfo + ";;OEM information (e_oemid specific)");
            results.Add("IMAGE_DOS_HEADER.e_res2_0;" + dosHeader.e_res2_0 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res2_1;" + dosHeader.e_res2_1 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res2_2;" + dosHeader.e_res2_2 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res2_3;" + dosHeader.e_res2_3 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res2_4;" + dosHeader.e_res2_4 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res2_5;" + dosHeader.e_res2_5 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res2_6;" + dosHeader.e_res2_6 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res2_7;" + dosHeader.e_res2_7 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res2_8;" + dosHeader.e_res2_8 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_res2_9;" + dosHeader.e_res2_9 + ";;Reserved words");
            results.Add("IMAGE_DOS_HEADER.e_lfanew;" + dosHeader.e_lfanew + ";;File address of new exe header");
            
            // IMAGE_FILE_HEADER
            IMAGE_FILE_HEADER fileHeader = BytesToStructure<IMAGE_FILE_HEADER>(bytes.Skip((int)dosHeader.e_lfanew+4).ToArray());
            results.Add("IMAGE_FILE_HEADER.Machine;" + fileHeader.Machine + ";0x" + fileHeader.Machine.ToString("x"));
            results.Add("IMAGE_FILE_HEADER.NumberOfSections;" + fileHeader.NumberOfSections);
            results.Add("IMAGE_FILE_HEADER.TimeDateStamp;" + fileHeader.TimeDateStamp);
            results.Add("IMAGE_FILE_HEADER.PointerToSymbolTable;" + fileHeader.PointerToSymbolTable);
            results.Add("IMAGE_FILE_HEADER.NumberOfSymbols;" + fileHeader.NumberOfSymbols);
            results.Add("IMAGE_FILE_HEADER.SizeOfOptionalHeader;" + fileHeader.SizeOfOptionalHeader);
            results.Add("IMAGE_FILE_HEADER.Characteristics;" + fileHeader.Characteristics + ";0x" + fileHeader.Characteristics.ToString("x"));

            bool Is32BitHeader = fileHeader.Machine != 0 && fileHeader.Machine != 0x8664;
            UInt64 ImageBase = 0;
            // IMAGE_OPTIONAL_HEADER
            IMAGE_OPTIONAL_HEADER32 optionalHeader32;
            IMAGE_OPTIONAL_HEADER64 optionalHeader64;
            IMAGE_DATA_DIRECTORIES[] dataDirectories = new IMAGE_DATA_DIRECTORIES[16];
            long offset = dosHeader.e_lfanew + 4 + Marshal.SizeOf(typeof(IMAGE_FILE_HEADER));
            if (Is32BitHeader)
            {
                optionalHeader32 = BytesToStructure<IMAGE_OPTIONAL_HEADER32>(bytes.Skip((int)offset).ToArray());
                offset += Marshal.SizeOf(typeof(IMAGE_OPTIONAL_HEADER32));

                ImageBase = optionalHeader32.ImageBase;

                results.Add("IMAGE_OPTIONAL_HEADER32.Magic;" + optionalHeader32.Magic);
                results.Add("IMAGE_OPTIONAL_HEADER32.MajorLinkerVersion;" + optionalHeader32.MajorLinkerVersion);
                results.Add("IMAGE_OPTIONAL_HEADER32.MinorLinkerVersion;" + optionalHeader32.MinorLinkerVersion);
                results.Add("IMAGE_OPTIONAL_HEADER32.SizeOfCode;" + optionalHeader32.SizeOfCode);
                results.Add("IMAGE_OPTIONAL_HEADER32.SizeOfInitializedData;" + optionalHeader32.SizeOfInitializedData);
                results.Add("IMAGE_OPTIONAL_HEADER32.SizeOfUninitializedData;" + optionalHeader32.SizeOfUninitializedData);
                results.Add(string.Format("IMAGE_OPTIONAL_HEADER32.AddressOfEntryPoint;{0};0x{1:x}", optionalHeader32.AddressOfEntryPoint, optionalHeader32.AddressOfEntryPoint.ToString("x")));
                results.Add("IMAGE_OPTIONAL_HEADER32.BaseOfCode;" + optionalHeader32.BaseOfCode);
                results.Add("IMAGE_OPTIONAL_HEADER32.BaseOfData;" + optionalHeader32.BaseOfData);
                results.Add("IMAGE_OPTIONAL_HEADER32.ImageBase;" + optionalHeader32.ImageBase + ";0x" + optionalHeader32.ImageBase.ToString("x"));
                results.Add("IMAGE_OPTIONAL_HEADER32.SectionAlignment;" + optionalHeader32.SectionAlignment);
                results.Add("IMAGE_OPTIONAL_HEADER32.FileAlignment;" + optionalHeader32.FileAlignment);
                results.Add("IMAGE_OPTIONAL_HEADER32.MajorOperatingSystemVersion;" + optionalHeader32.MajorOperatingSystemVersion);
                results.Add("IMAGE_OPTIONAL_HEADER32.MinorOperatingSystemVersion;" + optionalHeader32.MinorOperatingSystemVersion);
                results.Add("IMAGE_OPTIONAL_HEADER32.MajorImageVersion;" + optionalHeader32.MajorImageVersion);
                results.Add("IMAGE_OPTIONAL_HEADER32.MinorImageVersion;" + optionalHeader32.MinorImageVersion);
                results.Add("IMAGE_OPTIONAL_HEADER32.MajorSubsystemVersion;" + optionalHeader32.MajorSubsystemVersion);
                results.Add("IMAGE_OPTIONAL_HEADER32.MinorSubsystemVersion;" + optionalHeader32.MinorSubsystemVersion);
                results.Add("IMAGE_OPTIONAL_HEADER32.Win32VersionValue;" + optionalHeader32.Win32VersionValue);
                results.Add("IMAGE_OPTIONAL_HEADER32.SizeOfImage;" + optionalHeader32.SizeOfImage);
                results.Add("IMAGE_OPTIONAL_HEADER32.SizeOfHeaders;" + optionalHeader32.SizeOfHeaders);
                results.Add("IMAGE_OPTIONAL_HEADER32.CheckSum;" + optionalHeader32.CheckSum);
                results.Add("IMAGE_OPTIONAL_HEADER32.Subsystem;" + optionalHeader32.Subsystem);
                results.Add("IMAGE_OPTIONAL_HEADER32.DllCharacteristics;" + optionalHeader32.DllCharacteristics);
                results.Add("IMAGE_OPTIONAL_HEADER32.SizeOfStackReserve;" + optionalHeader32.SizeOfStackReserve);
                results.Add("IMAGE_OPTIONAL_HEADER32.SizeOfStackCommit;" + optionalHeader32.SizeOfStackCommit);
                results.Add("IMAGE_OPTIONAL_HEADER32.SizeOfHeapReserve;" + optionalHeader32.SizeOfHeapReserve);
                results.Add("IMAGE_OPTIONAL_HEADER32.SizeOfHeapCommit;" + optionalHeader32.SizeOfHeapCommit);
                results.Add("IMAGE_OPTIONAL_HEADER32.LoaderFlags;" + optionalHeader32.LoaderFlags);
                results.Add("IMAGE_OPTIONAL_HEADER32.NumberOfRvaAndSizes;" + optionalHeader32.NumberOfRvaAndSizes);
            }
            else
            {
                optionalHeader64 = BytesToStructure<IMAGE_OPTIONAL_HEADER64>(bytes.Skip((int)offset).ToArray());                
                offset += Marshal.SizeOf(typeof(IMAGE_OPTIONAL_HEADER64));

                ImageBase = optionalHeader64.ImageBase;

                results.Add("IMAGE_OPTIONAL_HEADER64.Magic;" + optionalHeader64.Magic);
                results.Add("IMAGE_OPTIONAL_HEADER64.MajorLinkerVersion;" + optionalHeader64.MajorLinkerVersion);
                results.Add("IMAGE_OPTIONAL_HEADER64.MinorLinkerVersion;" + optionalHeader64.MinorLinkerVersion);
                results.Add("IMAGE_OPTIONAL_HEADER64.SizeOfCode;" + optionalHeader64.SizeOfCode);
                results.Add("IMAGE_OPTIONAL_HEADER64.SizeOfInitializedData;" + optionalHeader64.SizeOfInitializedData);
                results.Add("IMAGE_OPTIONAL_HEADER64.SizeOfUninitializedData;" + optionalHeader64.SizeOfUninitializedData);
                results.Add("IMAGE_OPTIONAL_HEADER64.AddressOfEntryPoint;" + (optionalHeader64.ImageBase + optionalHeader64.AddressOfEntryPoint) + ";0x" + (optionalHeader64.ImageBase + optionalHeader64.AddressOfEntryPoint).ToString("x"));
                results.Add("IMAGE_OPTIONAL_HEADER64.BaseOfCode;" + optionalHeader64.BaseOfCode);
                results.Add("IMAGE_OPTIONAL_HEADER64.ImageBase;" + optionalHeader64.ImageBase + ";0x" + optionalHeader64.ImageBase.ToString("x"));
                results.Add("IMAGE_OPTIONAL_HEADER64.SectionAlignment;" + optionalHeader64.SectionAlignment);
                results.Add("IMAGE_OPTIONAL_HEADER64.FileAlignment;" + optionalHeader64.FileAlignment);
                results.Add("IMAGE_OPTIONAL_HEADER64.MajorOperatingSystemVersion;" + optionalHeader64.MajorOperatingSystemVersion);
                results.Add("IMAGE_OPTIONAL_HEADER64.MinorOperatingSystemVersion;" + optionalHeader64.MinorOperatingSystemVersion);
                results.Add("IMAGE_OPTIONAL_HEADER64.MajorImageVersion;" + optionalHeader64.MajorImageVersion);
                results.Add("IMAGE_OPTIONAL_HEADER64.MinorImageVersion;" + optionalHeader64.MinorImageVersion);
                results.Add("IMAGE_OPTIONAL_HEADER64.MajorSubsystemVersion;" + optionalHeader64.MajorSubsystemVersion);
                results.Add("IMAGE_OPTIONAL_HEADER64.MinorSubsystemVersion;" + optionalHeader64.MinorSubsystemVersion);
                results.Add("IMAGE_OPTIONAL_HEADER64.Win32VersionValue;" + optionalHeader64.Win32VersionValue);
                results.Add("IMAGE_OPTIONAL_HEADER64.SizeOfImage;" + optionalHeader64.SizeOfImage);
                results.Add("IMAGE_OPTIONAL_HEADER64.SizeOfHeaders;" + optionalHeader64.SizeOfHeaders);
                results.Add("IMAGE_OPTIONAL_HEADER64.CheckSum;" + optionalHeader64.CheckSum);
                results.Add("IMAGE_OPTIONAL_HEADER64.Subsystem;" + optionalHeader64.Subsystem);
                results.Add("IMAGE_OPTIONAL_HEADER64.DllCharacteristics;" + optionalHeader64.DllCharacteristics);
                results.Add("IMAGE_OPTIONAL_HEADER64.SizeOfStackReserve;" + optionalHeader64.SizeOfStackReserve);
                results.Add("IMAGE_OPTIONAL_HEADER64.SizeOfStackCommit;" + optionalHeader64.SizeOfStackCommit);
                results.Add("IMAGE_OPTIONAL_HEADER64.SizeOfHeapReserve;" + optionalHeader64.SizeOfHeapReserve);
                results.Add("IMAGE_OPTIONAL_HEADER64.SizeOfHeapCommit;" + optionalHeader64.SizeOfHeapCommit);
                results.Add("IMAGE_OPTIONAL_HEADER64.LoaderFlags;" + optionalHeader64.LoaderFlags);
                results.Add("IMAGE_OPTIONAL_HEADER64.NumberOfRvaAndSizes;" + optionalHeader64.NumberOfRvaAndSizes);
            }

            // IMAGE_DATA_DIRECTORIES            
            for (int i = 0; i < 16; i++)
            {
                long dd_offset = offset + i * Marshal.SizeOf(typeof(IMAGE_DATA_DIRECTORIES));
                dataDirectories[i] = BytesToStructure<IMAGE_DATA_DIRECTORIES>(bytes.Skip((int)dd_offset).ToArray());
            }

            for (int i = 0; i < 16; i++)
            {
                if (dataDirectories[i].VirtualAddress == 0 && dataDirectories[i].Size == 0) continue;
                results.Add("IMAGE_DATA_DIRECTORIES[" + i + "].VirtualAddress;" + dataDirectories[i].VirtualAddress + ";0x" + dataDirectories[i].VirtualAddress.ToString("x") + ";" + ((Dir)i).ToString());
                results.Add("IMAGE_DATA_DIRECTORIES[" + i + "].Size;" + dataDirectories[i].Size);
            }

            //number of sections
            int ns = Math.Min((int)fileHeader.NumberOfSections, 32);

            //IMAGE_SECTION_HEADER
            offset += 16 * Marshal.SizeOf(typeof(IMAGE_DATA_DIRECTORIES));
            IMAGE_SECTION_HEADER[] sectHeaders = new IMAGE_SECTION_HEADER[ns];
            for (int i = 0; i < ns; i++)
            {
                long dd_offset = offset + i * Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER));
                sectHeaders[i] = BytesToStructure<IMAGE_SECTION_HEADER>(bytes.Skip((int)dd_offset).ToArray());
            }
            
            results.Add("IMAGE_SECTION_HEADER.Columns;Name;VirtualSize;VirtualAddress;VirtualAddress;SizeOfRawData;PointerToRawData;PointerToRawData;PointerToRelocations;PointerToLinenumbers;NumberOfRelocations;NumberOfLinenumbers;Characteristics_dec;Characteristics_hex;Characteristics");
            for (int i = 0; sectHeaders != null && i < ns; i++)
            {
                String sectName = sectHeaders[i].Name.ToString("X");
                StringBuilder secname = new StringBuilder(8);
                for (int x = sectName.Length - 2; x >= 0; x -= 2)
                {
                    int value = Convert.ToInt32(sectName.Substring(x, 2), 16);
                    if (value > 0) secname.Append(Char.ConvertFromUtf32(value));
                }
                results.Add("IMAGE_SECTION_HEADER[" + i + "];" + secname + ";" +
                    sectHeaders[i].VirtualSize + ";" +
                    (ImageBase + sectHeaders[i].VirtualAddress) + ";" +
                    "0x" + (ImageBase + sectHeaders[i].VirtualAddress).ToString("x") + ";" +
                    sectHeaders[i].SizeOfRawData + ";" +
                    sectHeaders[i].PointerToRawData + ";" +
                    "0x" + sectHeaders[i].PointerToRawData.ToString("x") + ";" +
                    sectHeaders[i].PointerToRelocations + ";" +
                    sectHeaders[i].PointerToLinenumbers + ";" +
                    sectHeaders[i].NumberOfRelocations + ";" +
                    sectHeaders[i].NumberOfLinenumbers + ";" +
                    sectHeaders[i].Characteristics + ";" +
                    "0x" + sectHeaders[i].Characteristics.ToString("x") + ";" +
                    "characteristics");
            }
            
            offset = sectHeaders[(int)Dir.Resource].PointerToRawData;
            
            RESOURCE_DIRECTORY ResourceDir = BytesToStructure<RESOURCE_DIRECTORY>(bytes.Skip((int)offset).ToArray());
            results.Add("RESOURCE_DIR_ROOT.Characteristics;" + ResourceDir.Characteristics);
            results.Add("RESOURCE_DIR_ROOT.TimeDateStamp;" + ResourceDir.TimeDateStamp);
            results.Add("RESOURCE_DIR_ROOT.MajorVersion;" + ResourceDir.MajorVersion);
            results.Add("RESOURCE_DIR_ROOT.MinorVersion;" + ResourceDir.MinorVersion);
            results.Add("RESOURCE_DIR_ROOT.NumberOfNamedEntries;" + ResourceDir.NumberOfNamedEntries);
            results.Add("RESOURCE_DIR_ROOT.NumberOfldEntries;" + ResourceDir.NumberOfldEntries);

            offset += Marshal.SizeOf(typeof(RESOURCE_DIRECTORY_ENTRY))+8;
            // RESOURCE_DIRECTORY_ENTRY 
            List<RESOURCE_DIRECTORY_ENTRY> res_entries = new List<RESOURCE_DIRECTORY_ENTRY>();
            RESOURCE_DIRECTORY_ENTRY res_ent;
            
            for (int ie = 0; ie < ResourceDir.NumberOfNamedEntries + ResourceDir.NumberOfldEntries; ie++)
            {
                // read resource entries 
                long dd_offset = offset + ie * Marshal.SizeOf(typeof(RESOURCE_DIRECTORY_ENTRY));
                res_ent = BytesToStructure<RESOURCE_DIRECTORY_ENTRY>(bytes.Skip((int)dd_offset).ToArray());
                res_entries.Add(res_ent);
                // TODO
                //long name_offset = sectHeaders[(int)Dir.Resource].PointerToRawData + (res_ent.Name); //resname
                //string name=string.Concat(bytes.Skip((int)name_offset).Take(32).Where(b => b > 0).Select(c => (char)c));
            }

            for (int i = 0; i < res_entries.Count; i++)
            {
                //string resname = res_entry_names[i].Length > 0 ? res_entry_names[i] : resType(res_entries[i].Name);
                results.Add("RESOURCE_ENTRY[" + i + "];" +
                    res_entries[i].Name// + ";" +
                    //"name"
                    /*res_entries[i].OffsetToData + ";" +
                    res_dirs[i].Characteristics + ";" +
                    res_dirs[i].TimeDateStamp + ";" +
                    res_dirs[i].MajorVersion + ";" +
                    res_dirs[i].MinorVersion + ";" +
                    res_dirs[i].NumberOfNamedEntries + ";" +
                    res_dirs[i].NumberOfldEntries*/
                    );
            }
            // finaly
            ConsoleWrite_Atom(string.Join("\n", results));
        }        
        public struct IMAGE_DOS_HEADER
        {
            public UInt16 e_magic;              // Magic number
            public UInt16 e_cblp;               // Bytes on last page of file
            public UInt16 e_cp;                 // Pages in file
            public UInt16 e_crlc;               // Relocations
            public UInt16 e_cparhdr;            // Size of header in paragraphs
            public UInt16 e_minalloc;           // Minimum extra paragraphs needed
            public UInt16 e_maxalloc;           // Maximum extra paragraphs needed
            public UInt16 e_ss;                 // Initial (relative) SS value
            public UInt16 e_sp;                 // Initial SP value
            public UInt16 e_csum;               // Checksum
            public UInt16 e_ip;                 // Initial IP value
            public UInt16 e_cs;                 // Initial (relative) CS value
            public UInt16 e_lfarlc;             // File address of relocation table
            public UInt16 e_ovno;               // Overlay number
            public UInt16 e_res_0;              // Reserved words
            public UInt16 e_res_1;              // Reserved words
            public UInt16 e_res_2;              // Reserved words
            public UInt16 e_res_3;              // Reserved words
            public UInt16 e_oemid;              // OEM identifier (for e_oeminfo)
            public UInt16 e_oeminfo;            // OEM information; e_oemid specific
            public UInt16 e_res2_0;             // Reserved words
            public UInt16 e_res2_1;             // Reserved words
            public UInt16 e_res2_2;             // Reserved words
            public UInt16 e_res2_3;             // Reserved words
            public UInt16 e_res2_4;             // Reserved words
            public UInt16 e_res2_5;             // Reserved words
            public UInt16 e_res2_6;             // Reserved words
            public UInt16 e_res2_7;             // Reserved words
            public UInt16 e_res2_8;             // Reserved words
            public UInt16 e_res2_9;             // Reserved words
            public UInt32 e_lfanew;             // File address of new exe header
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_FILE_HEADER
        {
            public UInt16 Machine;
            public UInt16 NumberOfSections;
            public UInt32 TimeDateStamp;
            public UInt32 PointerToSymbolTable;
            public UInt32 NumberOfSymbols;
            public UInt16 SizeOfOptionalHeader;
            public UInt16 Characteristics;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER32
        {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt32 BaseOfData;
            public UInt32 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt32 SizeOfStackReserve;
            public UInt32 SizeOfStackCommit;
            public UInt32 SizeOfHeapReserve;
            public UInt32 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER64
        {
            public UInt16 Magic;
            public Byte MajorLinkerVersion;
            public Byte MinorLinkerVersion;
            public UInt32 SizeOfCode;
            public UInt32 SizeOfInitializedData;
            public UInt32 SizeOfUninitializedData;
            public UInt32 AddressOfEntryPoint;
            public UInt32 BaseOfCode;
            public UInt64 ImageBase;
            public UInt32 SectionAlignment;
            public UInt32 FileAlignment;
            public UInt16 MajorOperatingSystemVersion;
            public UInt16 MinorOperatingSystemVersion;
            public UInt16 MajorImageVersion;
            public UInt16 MinorImageVersion;
            public UInt16 MajorSubsystemVersion;
            public UInt16 MinorSubsystemVersion;
            public UInt32 Win32VersionValue;
            public UInt32 SizeOfImage;
            public UInt32 SizeOfHeaders;
            public UInt32 CheckSum;
            public UInt16 Subsystem;
            public UInt16 DllCharacteristics;
            public UInt64 SizeOfStackReserve;
            public UInt64 SizeOfStackCommit;
            public UInt64 SizeOfHeapReserve;
            public UInt64 SizeOfHeapCommit;
            public UInt32 LoaderFlags;
            public UInt32 NumberOfRvaAndSizes;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_DATA_DIRECTORIES
        {
            public UInt32 VirtualAddress;
            public UInt32 Size;
        }
        public enum Dir
        {
            Export,
            Import,
            Resource,
            Exception,
            Security,
            BaseRelocationTable,
            DebugDirectory,
            CopyrightOrArchitectureSpecificData,
            GlobalPtr,
            TLSDirectory,
            LoadConfigurationDirectory,
            BoundImportDirectory,
            ImportAddressTable,
            DelayLoadImportDescriptors,
            COMRuntimedescriptor,
            Reserved
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_SECTION_HEADER
        {
            public UInt64 Name;
            public UInt32 VirtualSize;
            public UInt32 VirtualAddress;
            public UInt32 SizeOfRawData;
            public UInt32 PointerToRawData;
            public UInt32 PointerToRelocations;
            public UInt32 PointerToLinenumbers;
            public UInt16 NumberOfRelocations;
            public UInt16 NumberOfLinenumbers;
            public UInt32 Characteristics;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RESOURCE_DIRECTORY
        {
            public UInt32 Characteristics;
            public UInt32 TimeDateStamp;
            public UInt16 MajorVersion;
            public UInt16 MinorVersion;
            public UInt16 NumberOfNamedEntries;
            public UInt16 NumberOfldEntries;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RESOURCE_DIRECTORY_ENTRY
        {
            public UInt32 Name;
            public UInt32 OffsetToData;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RESOURCE_DATA_ENTRY
        {
            public UInt32 OffsetToData;
            public UInt32 Size;
            public UInt32 CodePage;
            public UInt32 Reserved;
        }
        public static T BytesToStructure<T>(byte[] bytes) where T : struct
        {            
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                if (bytes.Length >= size)
                    Marshal.Copy(bytes, 0, ptr, size);
                return Marshal.PtrToStructure<T>(ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
        public static T ClearStruct<T>()
        {
            byte[] bytes = new byte[Marshal.SizeOf(typeof(T))];
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return theStructure;
        }
    }
}
