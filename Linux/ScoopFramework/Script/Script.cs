using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Reflection;

namespace ScoopFramework.Script
{
    public class Script
    {
        private ScriptEngine engine;
        private ScriptSource source;

        public Script()
        {
            engine = Python.CreateEngine();
        }

        public void AddAssembly(Assembly assembly)
        {
            engine.Runtime.LoadAssembly(assembly);
        }

        public void Load(string file)
        {
            if (System.IO.File.Exists(file))
                source=engine.CreateScriptSourceFromFile(file);
            else
                throw new ArgumentException("File does not exist");
        }

        public ScriptScope Execute(Dictionary<string, object> scope)
        {
            var scriptScope = engine.CreateScope(scope);

            source.Execute(scriptScope);
            return scriptScope;
        }

        public ReturnType Execute<ReturnType>(Dictionary<string, object> scope,string returnValueName="returnValue")
        {
            var scriptScope = engine.CreateScope(scope);

            source.Execute(scriptScope);

            dynamic returnValue = null;
            scriptScope.TryGetVariable("returnValue",out returnValue);
            return (ReturnType)returnValue;
        }
    }
}
