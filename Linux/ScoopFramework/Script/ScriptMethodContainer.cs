using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace ScoopFramework.Script
{
    public class ScriptMethodContainer
    {
        private ScriptEngine engine;
        private Dictionary<string,ScriptSource> scripts;
        public readonly string DefaultScriptPath;

        /// <summary>
        /// Initializes a new script container.
        /// </summary>
        public ScriptMethodContainer(string defaultScriptPath)
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
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, T> ExtractDelegate<T>(string methodName)
        {
            ScriptScope scope;
            T value;
            Dictionary<string, T> returnValue = new Dictionary<string, T>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<T>(methodName, out value);
                    if (value != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), value);
                }

            return returnValue;
        }

        #region ExtractAction
        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11>> ExtractAction<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10,in11> action;
            Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11>> returnValue = new Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, in11>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10>> ExtractAction<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10> action;
            Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10>> returnValue = new Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8, in9>> ExtractAction<in1, in2, in3, in4, in5, in6, in7, in8, in9>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2, in3, in4, in5, in6, in7, in8, in9> action;
            Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8, in9>> returnValue = new Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8, in9>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2, in3, in4, in5, in6, in7, in8, in9>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8>> ExtractAction<in1, in2, in3, in4, in5, in6, in7, in8>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2, in3, in4, in5, in6, in7, in8> action;
            Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8>> returnValue = new Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7, in8>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2, in3, in4, in5, in6, in7, in8>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7>> ExtractAction<in1, in2, in3, in4, in5, in6, in7>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2, in3, in4, in5, in6, in7> action;
            Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7>> returnValue = new Dictionary<string, Action<in1, in2, in3, in4, in5, in6, in7>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2, in3, in4, in5, in6, in7>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2, in3, in4, in5, in6>> ExtractAction<in1, in2, in3, in4, in5, in6>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2, in3, in4, in5, in6> action;
            Dictionary<string, Action<in1, in2, in3, in4, in5, in6>> returnValue = new Dictionary<string, Action<in1, in2, in3, in4, in5, in6>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2, in3, in4, in5, in6>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2, in3, in4, in5>> ExtractAction<in1, in2, in3, in4, in5>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2, in3, in4, in5> action;
            Dictionary<string, Action<in1, in2, in3, in4, in5>> returnValue = new Dictionary<string, Action<in1, in2, in3, in4, in5>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2, in3, in4, in5>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2, in3, in4>> ExtractAction<in1, in2, in3, in4>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2, in3, in4> action;
            Dictionary<string, Action<in1, in2, in3, in4>> returnValue = new Dictionary<string, Action<in1, in2, in3, in4>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2, in3, in4>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2, in3>> ExtractAction<in1, in2, in3>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2, in3> action;
            Dictionary<string, Action<in1, in2, in3>> returnValue = new Dictionary<string, Action<in1, in2, in3>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2, in3>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1, in2>> ExtractAction<in1, in2>(string methodName)
        {
            ScriptScope scope;
            Action<in1, in2> action;
            Dictionary<string, Action<in1, in2>> returnValue = new Dictionary<string, Action<in1, in2>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1, in2>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Action<in1>> ExtractAction<in1>(string methodName)
        {
            ScriptScope scope;
            Action<in1> action;
            Dictionary<string, Action<in1>> returnValue = new Dictionary<string, Action<in1>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Action<in1>>(methodName, out action);
                    if (action != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), action);
                }

            return returnValue;
        }
        #endregion

        #region ExtractFunction
        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, in2, in3, in4, in5, in6, in7, in8, in9,in10, out11>> ExtractFunction<in1, in2, in3, in4, in5, in6, in7, in8, in9,in10, out11>(string methodName)
        {
            ScriptScope scope;
            Func<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10,out11> func;
            Dictionary<string, Func<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, out11>> returnValue = new Dictionary<string, Func<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, out11>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1, in2, in3, in4, in5, in6, in7, in8, in9, in10, out11>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, in2, in3, in4, in5, in6, in7, in8, in9, out10>> ExtractFunction<in1, in2, in3, in4, in5, in6, in7, in8, in9, out10>(string methodName)
        {
            ScriptScope scope;
            Func<in1, in2, in3,in4,in5,in6,in7,in8,in9,out10> func;
            Dictionary<string, Func<in1, in2, in3,in4,in5,in6,in7,in8,in9,out10>> returnValue = new Dictionary<string, Func<in1, in2, in3,in4,in5,in6,in7,in8,in9,out10>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1, in2, in3,in4,in5,in6,in7,in8,in9,out10>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, in2, in3, in4, in5, in6, in7, in8, out9>> ExtractFunction<in1, in2, in3, in4, in5, in6, in7, in8, out9>(string methodName)
        {
            ScriptScope scope;
            Func<in1, in2, in3,in4,in5,in6,in7,in8,out9> func;
            Dictionary<string, Func<in1, in2, in3,in4,in5,in6,in7,in8,out9>> returnValue = new Dictionary<string, Func<in1, in2, in3,in4,in5,in6,in7,in8,out9>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1, in2, in3,in4,in5,in6,in7,in8,out9>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, in2, in3, in4, in5, in6, in7, out8>> ExtractFunction<in1, in2, in3, in4, in5, in6, in7, out8>(string methodName)
        {
            ScriptScope scope;
            Func<in1, in2, in3,in4,in5,in6,in7,out8> func;
            Dictionary<string, Func<in1, in2, in3,in4,in5,in6,in7,out8>> returnValue = new Dictionary<string, Func<in1, in2, in3,in4,in5,in6,in7,out8>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1, in2, in3,in4,in5,in6,in7,out8>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, in2, in3, in4, in5, in6, out7>> ExtractFunction<in1, in2, in3, in4, in5, in6, out7>(string methodName)
        {
            ScriptScope scope;
            Func<in1, in2, in3,in4,in5,in6,out7> func;
            Dictionary<string, Func<in1, in2, in3,in4,in5,in6,out7>> returnValue = new Dictionary<string, Func<in1, in2, in3,in4,in5,in6,out7>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1, in2, in3,in4,in5,in6,out7>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, in2, in3, in4, in5, out6>> ExtractFunction<in1, in2, in3, in4, in5, out6>(string methodName)
        {
            ScriptScope scope;
            Func<in1, in2, in3,in4,in5,out6> func;
            Dictionary<string, Func<in1, in2, in3,in4,in5,out6>> returnValue = new Dictionary<string, Func<in1, in2, in3,in4,in5,out6>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1, in2, in3,in4,in5,out6>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, in2, in3, in4, out5>> ExtractFunction<in1, in2, in3, in4, out5>(string methodName)
        {
            ScriptScope scope;
            Func<in1, in2, in3, in4, out5> func;
            Dictionary<string, Func<in1, in2, in3,in4,out5>> returnValue = new Dictionary<string, Func<in1, in2, in3,in4,out5>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1, in2, in3,in4,out5>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, in2, in3, out4>> ExtractFunction<in1, in2, in3, out4>(string methodName)
        {
            ScriptScope scope;
            Func<in1, in2, in3,out4> func;
            Dictionary<string, Func<in1, in2, in3,out4>> returnValue = new Dictionary<string, Func<in1, in2, in3,out4>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1, in2, in3,out4>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, in2, out3>> ExtractFunction<in1, in2, out3>(string methodName)
        {
            ScriptScope scope;
            Func<in1, in2, out3> func;
            Dictionary<string, Func<in1, in2, out3>> returnValue = new Dictionary<string, Func<in1, in2, out3>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1, in2, out3>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<in1, out2>> ExtractFunction<in1, out2>(string methodName)
        {
            ScriptScope scope;
            Func<in1, out2> func;
            Dictionary<string, Func<in1, out2>> returnValue = new Dictionary<string, Func<in1,out2>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<in1,out2>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }

        /// <summary>
        /// Extracts from each script the statet method.
        /// </summary>
        /// <returns>A dictionary with the scriptname (without file extension) and the extracted method. Scripts where no method was found are omitted.</returns>
        public Dictionary<string, Func<out1>> ExtractFunction<out1>(string methodName)
        {
            ScriptScope scope;
            Func<out1> func;
            Dictionary<string, Func<out1>> returnValue = new Dictionary<string, Func<out1>>();

            foreach (var script in scripts)
                if (script.Value != null)
                {
                    scope = engine.CreateScope();
                    script.Value.Execute(scope);
                    scope.TryGetVariable<Func<out1>>(methodName, out func);
                    if (func != null)
                        returnValue.Add(System.IO.Path.GetFileNameWithoutExtension(script.Key), func);
                }

            return returnValue;
        }
        #endregion

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
    }
}
