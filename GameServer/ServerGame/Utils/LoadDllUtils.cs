using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

class LoadDllUtils
{
    private const string dllPath = "libs";
    private const string dllEnd = ".dll";
    public static void LoadDll(string dllName) {
        string dll = dllName + dllEnd;
        string finalPath = Path.Combine(Environment.CurrentDirectory, dllPath, dll);
        try {
            Assembly assembly = Assembly.LoadFrom(finalPath);
            Type[] tt = assembly.GetTypes();
            foreach (Type t in tt)
            {
                Debug.Log(t.Name);
            }
        }
        catch (Exception e) {
            Debug.Log("Load Assembly Failed : "+e);
        }
    }
    public static void LoadAllDLL() { 
        
    }
}

