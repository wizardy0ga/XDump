using dnlib.DotNet.MD;
using dnlib.DotNet;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDump.src
{
    public static class MetadataParser
    {
        
        // Retrieves values held at fields by parsing class and variable names from tokens and then
        // using the assembly object to perform a look up of the value held at the class and variable.
        public static string GetFieldData(string assemblyPath, uint metadataTokenValue, Assembly assembly)
        {
            try
            {
                // Load the assembly using dnlib
                ModuleDefMD module = ModuleDefMD.Load(assemblyPath);

                // Find the metadata token in the module's metadata
                uint rid        = metadataTokenValue & 0x00FFFFFF;
                uint tableIndex = metadataTokenValue >> 24;

                // Create token object
                MDToken mdToken = new MDToken((Table)tableIndex, rid);

                // Resolve the token to the member and print data
                IMemberDef member = module.ResolveToken(mdToken) as IMemberDef;

                // Make sure the data is a field and is not null
                if (member is FieldDef field && member != null)
                {
                    
                    string Variable  = field.Name;
                    string Classname = field.FullName.Replace($"::{Variable}", "").Replace("System.String ", "");

                    #if DEBUG
                    Console.WriteLine($"Extracted Class Name: {Classname}");
                    Console.WriteLine($"Extracted Field Name: {Variable}");
                    #endif
                    
                    try 
                    {   
                        // Parse out the class 
                        Type Class = assembly.GetType(Classname);
                        if (Class != null)
                        {   
                            // Get the variable from the class
                            FieldInfo FieldVariable = Class.GetField(Variable, BindingFlags.Public | BindingFlags.Static);  
                            if (FieldVariable != null) 
                            { 
                                var Value = FieldVariable.GetValue(null);
                                return (string)Value;
                            }
                        }
                        else 
                        {
                            return null;
                        }
                    }
                    catch ( Exception ex ) 
                    {
                        Console.Write($"Error: {ex.Message}");
                    }

                    return null;
                }
                else
                {
                    Console.WriteLine("Unsupported metadata token type.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        // Searches for the mutex by locating the 16 character string.
        // 
        public static string GetMutex(string assemblyPath, Assembly assembly) 
        {
            foreach (XWormConfiguration setting in Enum.GetValues(typeof(XWormConfiguration)))
            {
                string data = GetFieldData(assemblyPath, (uint)setting, assembly);
                if (data.Length == 16)
                    return data;
                else if (data == null)
                { 
                }
            }
            return null;
        }

    }
}
