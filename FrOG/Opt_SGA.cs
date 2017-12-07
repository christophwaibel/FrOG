using System;
using System.Collections.Generic;
using System.Linq;
using MetaheuristicsLibrary;

/*
 * Opt_SGA.cs
 * Copyright 2017 Christoph Waibel <chwaibel@student.ethz.ch>
 * 
 * This work is licensed under the GNU GPL license version 3 or later.
*/

namespace FrOG
{
    public class Opt_SGA : ISolver
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

        public Opt_SGA()
        {
            var SGA_Settings_WW = new Dictionary<string, double>{
                {"maxgen", 10000},
                { "itermax", 10000},
                { "seed", 1},
                { "popsize", 14},
                { "k", 11},
                { "pcross", 1},
                { "pmut", 0.2},
                { "d", 0.01},
                { "r", 0.2},
                { "elite", 1}
            };

            var SGA_Settings_n4_A = new Dictionary<string, double>{
                {"maxgen", 10000},
                { "itermax", 10000},
                { "seed", 1},
                { "popsize", 14},
                { "k", 21.93823},
                { "pcross", 0.99301},
                { "pmut", 1},
                { "d", 0.01159},
                { "r", 0.15604},
                { "elite", 2}
            };

            var SGA_Settings_n4_B = new Dictionary<string, double>{
                {"maxgen", 10000},
                { "itermax", 10000},
                { "seed", 1},
                { "popsize", 6},
                { "k", 36.7352},
                { "pcross", 0.87624},
                { "pmut", 0.8189},
                { "d", 0.63072},
                { "r", 1.75361},
                { "elite", 1}
            };

            var SGA_Settings_n11_A = new Dictionary<string, double>{
                {"maxgen", 10000},
                { "itermax", 10000},
                { "seed", 1},
                { "popsize", 6},
                { "k", 99.78889},
                { "pcross", 0.97875},
                { "pmut", 0.9781},
                { "d", 0.13499},
                { "r", 1.95781},
                { "elite", 1}
            };

            //cluster C
            var SGA_Settings_n11_B = new Dictionary<string, double>{
                {"maxgen", 10000},
                { "itermax", 10000},
                { "seed", 1},
                { "popsize", 6},
                { "k", 47.43024},
                { "pcross", 0.98116},
                { "pmut", 0.99377},
                { "d", 0.01},
                { "r", 0.30406},
                { "elite", 1}
            };

            //cluster 1
            var SGA_Settings_n13_A = new Dictionary<string, double>{
                {"maxgen", 10000},
                { "itermax", 10000},
                { "seed", 1},
                { "popsize", 6},
                { "k", 71.23471},
                { "pcross", 0.96031},
                { "pmut", 0.97181},
                { "d", 0.01},
                { "r", 0.28861},
                { "elite", 1}
            };

            //cluster 2
            var SGA_Settings_n13_B = new Dictionary<string, double>{
                {"maxgen", 10000},
                { "itermax", 10000},
                { "seed", 1},
                { "popsize", 6},
                { "k", 87.72228},
                { "pcross", 0.83724},
                { "pmut", 1},
                { "d", 0.15312},
                { "r", 1.95743},
                { "elite", 1}
            };


            _presets.Add("SGA_WW", SGA_Settings_WW);
            _presets.Add("SGA_n4_A", SGA_Settings_n4_A);
            _presets.Add("SGA_n4_B", SGA_Settings_n4_B);
            _presets.Add("SGA_n11_A", SGA_Settings_n11_A);
            _presets.Add("SGA_n11_B", SGA_Settings_n11_B);
            _presets.Add("SGA_n13_A", SGA_Settings_n13_A);
            _presets.Add("SGA_n13_B", SGA_Settings_n13_B);
        }

        public bool RunSolver(List<Variable> variables, Func<IList<decimal>, double> evaluate, string preset, string expertsettings, string installFolder, string documentPath)
        {
            var settings = _presets[preset];

            //System.Windows.Forms.MessageBox.Show(expertsettings);     //use expertsettings to input custom solver parameters

            int? seedin = null;
            string [] expsets = expertsettings.Split(';');
            foreach (string strexp in expsets)
            {
                string[] stre = strexp.Split('=');
                if(string.Equals(stre[0],"seed"))
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
                if (preset.Equals("SGA_n4_A") || preset.Equals("SGA_n4_B") || 
                    preset.Equals("SGA_WW") ||
                    preset.Equals("SGA_n11_A") || preset.Equals("SGA_n11_B") ||
                    preset.Equals("SGA_n13_A") || preset.Equals("SGA_n13_B"))
                {
                    Dictionary<string, object> GAsettings = new Dictionary<string, object>();
                    GAsettings.Add("maxgen", (int)settings["maxgen"]);
                    GAsettings.Add("popsize", (int)settings["popsize"]);
                    GAsettings.Add("k", settings["k"]);
                    GAsettings.Add("pcross", settings["pcross"]);
                    GAsettings.Add("pmut", settings["pmut"]);
                    GAsettings.Add("d", settings["d"]);
                    GAsettings.Add("r", settings["r"]);
                    GAsettings.Add("elite", settings["elite"]);
                    int seed;
                    if (seedin != null)
                    {
                        seed = Convert.ToInt16(seedin);
                    }
                    else
                    {
                        Random rnd = new Random();
                        seed = rnd.Next();
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
