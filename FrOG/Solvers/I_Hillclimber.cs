using System;
using System.Collections.Generic;
using System.Linq;
using MetaheuristicsLibrary;

/*
 * Opt_HC.cs
 * Copyright 2017 Christoph Waibel <chwaibel@student.ethz.ch>
 * 
 * This work is licensed under the GNU GPL license version 3 or later.
*/

namespace FrOG.Solvers
{
    public class I_Hillclimber : ISolver
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

        public I_Hillclimber()
        {
            var HC_Settings = new Dictionary<string, double>{
                { "itermax", 10000},
                { "seed", 1},
                { "stepsize", 1}
            };

            _presets.Add("HC", HC_Settings);
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
                if (preset.Equals("HC"))
                {
                    var seed = (int)settings["seed"];
                    var stepsize = settings["stepsize"];
                    var itermax = (int)settings["itermax"];
                    var hc = new MetaheuristicsLibrary.SingleObjective.Hillclimber(lb, ub, integer, itermax, eval, seed, stepsize);
                    hc.solve();
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
