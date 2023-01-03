using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace DeviceManager
{
    class WebServiceHelper
    {

        private static Hashtable _assemblys = new Hashtable();

        public static object Invoke(string url, string className, string methodName, object[] parameters)
        {
            lock (_assemblys.SyncRoot)
            {
                if (!_assemblys.ContainsKey(url))
                {
                    CompilerResults result = MachineContext._Config.LoadSteam();
                        if (!result.Errors.HasErrors)
                        {
                            _assemblys[url] = result.CompiledAssembly;
                        }
                    }
                }
            
            if (!_assemblys.ContainsKey(url))
            {
                throw new DllNotFoundException();
            }
            Type t = ((Assembly)_assemblys[url]).GetType(className);

            return t.GetMethod(methodName).Invoke(Activator.CreateInstance(t), parameters);

        }
    }
}


