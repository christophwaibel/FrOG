using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Opt_PSO.cs
 * Copyright 2017 Christoph Waibel <chwaibel@student.ethz.ch>
 * 
 * This work is licensed under the GNU GPL license version 3 or later.
*/

namespace FrOG
{
    public class Opt_PSO : ISolver
    {
        /// <summary>
        /// Variable vector of final solution.
        /// </summary>
        public double[] Xopt { get; private set; }
        /// <summary>
        /// Cost of final solution.
        /// </summary>
        public double Fxopt { get; private set; }

        //public Dictionary<string, string> settings = new Dictionary<string, string>();

        private readonly Dictionary<string, Dictionary<string, double>> _presets = new Dictionary<string, Dictionary<string, double>>();

        public Opt_PSO()
        {
            //Prepare settings
            var PSO_Settings = new Dictionary<string, double>{
                {"popsize", 50},
                {"maxgen", 200},
                { "itermax", 10000},
                { "seed", 1},
                { "k", 6},
                { "pcross", 0.7},
                { "pmut", 0.3},
                { "d", 0.1},
                { "r", 0.1}
            };

            _presets.Add("PSO", PSO_Settings);
        }

        public bool RunSolver(List<Variable> variables, Func<IList<decimal>, double> evaluate, string preset, string expertsettings, string installFolder, string documentPath)
        {
            var settings = _presets[preset];

            //System.Windows.Forms.MessageBox.Show(expertsettings);     //use expertsettings to input custom solver parameters

            int? seedin = null;
            string[] expsets = expertsettings.Split(';');
            foreach (string strexp in expsets)
            {
                string[] stre = strexp.Split('=');
                if (string.Equals(stre[0], "seed"))
                {
                    seedin = Convert.ToInt16(stre[1]);
                }
            }

            var dvar = variables.Count;
            var lb = new double[dvar];
            var ub = new double[dvar];
            var integer = new bool[dvar];

            for (var i = 0; i < dvar; i++)
            {
                lb[i] = Convert.ToDouble(variables[i].LowerB);
                ub[i] = Convert.ToDouble(variables[i].UpperB);
                integer[i] = variables[i].Integer;
            }

            Func<double[], double> eval = x =>
            {
                var decis = x.Select(Convert.ToDecimal).ToList();
                return evaluate(decis);
            };

            try
            {
                if (preset.Equals("PSO"))
                {
                    Dictionary<string, object> GAsettings = new Dictionary<string, object>();
                    GAsettings.Add("maxgen", (int)settings["maxgen"]);
                    GAsettings.Add("popsize", (int)settings["popsize"]);
                    GAsettings.Add("k", settings["k"]);
                    GAsettings.Add("pcross", settings["pcross"]);
                    GAsettings.Add("pmut", settings["pmut"]);
                    GAsettings.Add("d", settings["d"]);
                    GAsettings.Add("r", settings["r"]);
                    int seed;
                    if (seedin != null)
                    {
                        seed = Convert.ToInt16(seedin);
                    }
                    else
                    {
                        seed = (int)settings["seed"];
                    }
                    int itermax = (int)settings["itermax"];

                    var ga = new MetaheuristicsLibrary.SolversSO.SimpleGA(lb, ub, integer, itermax, eval, seed, GAsettings);
                    ga.solve();
                    Xopt = ga.get_Xoptimum();
                    Fxopt = ga.get_fxoptimum();
                }
                else
                {
                    var seed = (int)settings["seed"];
                    var stepsize = settings["stepsize"];
                    var itermax = (int)settings["itermax"];
                    var hc = new Hillclimber_Algorithm(lb, ub, stepsize, itermax, eval, seed);
                    hc.Solve();
                    Xopt = hc.get_Xoptimum();
                    Fxopt = hc.get_fxoptimum();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public string GetErrorMessage()
        {
            return "";
        }

        /// <summary>
        /// Get the variable vector of the final solution.
        /// </summary>
        /// <returns>Variable vector.</returns>
        public double[] get_Xoptimum()
        {
            return Xopt;
        }

        public IEnumerable<string> GetPresetNames()
        {
            return _presets.Keys;
        }
    }
}
