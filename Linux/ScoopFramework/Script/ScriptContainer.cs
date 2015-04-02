using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace ScoopFramework.Script
{
    public class ScriptContainer
    {
        private ScriptEngine engine;
        private Dictionary<string,ScriptSource> scripts;
        public readonly string DefaultScriptPath;

        /// <summary>
        /// Initializes a new script container.
        /// </summary>
        public ScriptContainer(string defaultScriptPath)
        {
            scripts = new Dictionary<string, ScriptSource>();
            engine = Python.CreateEngine();
            DefaultScriptPath = defaultScriptPath;

            Invariant();
        }

        /// <summary>
        /// Loads a script into the container.
        /// </summary>
        /// <param name="file">The scriptfile. If a relative path is given, the base path will be the default script folder.</param>
        public void Load(string file)
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(DefaultScriptPath,file)))
                scripts.Add(System.IO.Path.Combine(DefaultScriptPath, file), null);
            else if (System.IO.File.Exists(file))
                scripts.Add(file, null);
            else
                throw new ArgumentException("File does not exist");
        }

        /// <summary>
        /// Loads all scripts in the DefaultScriptPath (non-recursive).
        /// </summary>
        public void LoadAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compiles all uncompiled scripts.
        /// </summary>
        /// <returns>Number of compiled scripts.</returns>
        public int Compile()
        {
            int returnValue = 0;
            var keys = scripts.Keys.ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];

                if (scripts[key] == null)
                {
                    scripts[key] = engine.CreateScriptSourceFromFile(key);
                    scripts[key].Compile();
                    returnValue++;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Compiles all scripts.
        /// </summary>
        /// <returns>Number of compiled scripts.</returns>
        public int ReCompile()
        {
            int returnValue = 0;

            foreach (string script in scripts.Keys)
            {
                scripts[script] = engine.CreateScriptSourceFromFile(script);
                scripts[script].Compile();
                returnValue++;
            }

            return returnValue;
        }

        /// <summary>
        /// Throws an exception if the object is inconsistent.
        /// </summary>
        public void Invariant()
        {
            if (
                !System.IO.Directory.Exists(DefaultScriptPath) ||
                engine == null ||
                scripts == null
                )
                throw new Exception("Invariant failed.");
        }

        /// <summary>
        /// Executes a script.
        /// </summary>
        /// <param name="script">The name of the script.</param>
        /// <param name="variables">The variables and functions the script may read and modify. The changes to the variables are persistent as long as the reference remains the same (e.g. v=new Test() is not persistend, but v.attr=12 is).</param>
        /// <returns>If the script specifys the returnValue variable, this variable is returned.</returns>
        public OutputType Execute<OutputType>(string scriptName, Dictionary<string, object> variables)
        {
            if (scripts.Keys.Contains(scriptName))
            {
                var scope = engine.CreateScope();

                foreach (var elem in variables)
                    scope.SetVariable(elem.Key, elem.Value);
                scripts[scriptName].Execute(scope);
                dynamic returnValue = null;
                scope.TryGetVariable("returnValue", returnValue);
                return (OutputType)returnValue;
            }
            else
                throw new ArgumentException("Script is not loaded!");
        }

        /// <summary>
        /// Returns the names of all scripts in the container.
        /// </summary>
        public IEnumerable<string> GetScripts()
        {
            return this.scripts.Keys;
        }
    }
}
