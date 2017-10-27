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
    public class Opt_FIPSO : ISolver
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

        public Opt_FIPSO()
        {
            //Prepare settings
            var FIPSO_Settings = new Dictionary<string, double>{
                {"popsize", 24},
                { "seed", 1},
                { "itermax", 10000},
                {"chi", 0.1},           // constriction coefficient
                {"phi", 4.0},           // attraction to best particle 
                {"v0max", 0.2},         // max velocity at initialisation. fraction of domain.
                {"x0samplingmode", 0},  // 0 = uniform, 1 = gaussian
                {"pxupdatemode", 0},    //0 = update after population. 1 = update after each evaluation
                {"s0", 1.0}             //initial sampling step size, only for gaussian initialization
            };

            _presets.Add("FIPSO", FIPSO_Settings);
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
                if (preset.Equals("FIPSO"))
                {
                    Dictionary<string, object> FIPSOsettings = new Dictionary<string, object>();    
                    FIPSOsettings.Add("popsize", (int)settings["popsize"]);
                    FIPSOsettings.Add("chi", settings["chi"]);
                    FIPSOsettings.Add("phi", settings["phi"]);
                    FIPSOsettings.Add("v0max", settings["v0max"]);
                    FIPSOsettings.Add("x0samplingmode", (int)settings["x0samplingmode"]);
                    FIPSOsettings.Add("pxupdatemode", (int)settings["pxupdatemode"]);
                    FIPSOsettings.Add("s0", settings["s0"]);
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

                    var fipso = new MetaheuristicsLibrary.SolversSO.FIPSO(lb, ub, integer, itermax, eval, seed, FIPSOsettings);
                    fipso.solve();
                    Xopt = fipso.get_Xoptimum();
                    Fxopt = fipso.get_fxoptimum();
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
