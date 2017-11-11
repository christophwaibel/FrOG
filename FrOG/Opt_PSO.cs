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
           
            //cluster 3
            var FIPSO_settings_n4_A = new Dictionary<string, double>
            {
                {"popsize", 14},
                {"chi", 0.36334},
                {"phi", 8.79188},
                {"v0max", 0},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            //cluster 2
            var FIPSO_settings_n4_B = new Dictionary<string, double>
            {
                {"popsize", 25},
                {"chi", 0.06836},
                {"phi", 48.62888},
                {"v0max", 0},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            //cluster 1
            var PSO_settings_n4_A = new Dictionary<string, double>
            {
                {"popsize", 55},
                {"chi", 0.001},
                {"v0max", 10.6235},
                {"pxupdatemode", 0},
                {"psomode", 1},
                {"phi1", 1.44653},
                {"phi2",1.46098},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            //cluster 2
            var PSO_settings_n4_B = new Dictionary<string, double>
            {
                {"popsize", 10},
                {"chi", 0.3386},
                {"v0max", 0.65278},
                {"pxupdatemode", 0},
                {"psomode", 2},
                {"phi1", 5.0},
                {"phi2",5.0},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            _presets.Add("FIPSO_n4_A", FIPSO_settings_n4_A);
            _presets.Add("FIPSO_n4_B", FIPSO_settings_n4_B);
            _presets.Add("PSO_n4_A", PSO_settings_n4_A);
            _presets.Add("PSO_n4_B", PSO_settings_n4_B);
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
                if (preset.Equals("FIPSO_n4_A") || preset.Equals("FIPSO_n4_B") || preset.Equals("PSO_n4_A") || preset.Equals("PSO_n4_B"))
                {
                    Dictionary<string, object> PSOsettings = new Dictionary<string, object>();    
                    PSOsettings.Add("popsize", (int)settings["popsize"]);
                    PSOsettings.Add("chi", settings["chi"]);
                    PSOsettings.Add("phi", settings["phi"]);
                    PSOsettings.Add("phi1", settings["phi1"]);
                    PSOsettings.Add("phi2", settings["phi2"]);
                    PSOsettings.Add("v0max", settings["v0max"]);
                    PSOsettings.Add("psomode", (int)settings["psomode"]);
                    PSOsettings.Add("pxupdatemode", (int)settings["pxupdatemode"]);
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

                    var pso = new MetaheuristicsLibrary.SolversSO.PSO(lb, ub, integer, itermax, eval, seed, PSOsettings);
                    pso.solve();
                    Xopt = pso.get_Xoptimum();
                    Fxopt = pso.get_fxoptimum();
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
