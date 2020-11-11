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

namespace FrOG.Solvers
{
    public class I_PSO : ISolver
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

        public I_PSO()
        {
            //Prepare settings
            #region FIPSO

            var FIPSO_settings_untuned = new Dictionary<string, double>
            {
                {"popsize", 20},
                {"chi", 0.1},
                {"phi", 4.0},
                {"v0max", 0.2},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n4_A = new Dictionary<string, double>
            {
                {"popsize", 10},
                {"chi", 0.19208},
                {"phi", 18.69223},
                {"v0max", 9.72903},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n4_B = new Dictionary<string, double>
            {
                {"popsize", 10},
                {"chi", 0.39852},
                {"phi", 9.24731},
                {"v0max", 0.01699},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n10_A = new Dictionary<string, double>
            {
                {"popsize", 12},
                {"chi", 0.22413	},
                {"phi", 16.97488	},
                {"v0max", 0.2231},
                {"pxupdatemode", 1},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n10_B = new Dictionary<string, double>
            {
                {"popsize", 14},
                {"chi", 0.13462	},
                {"phi", 27.40733	},
                {"v0max", 5.91936},
                {"pxupdatemode", 1},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n11_A = new Dictionary<string, double>
            {
                {"popsize", 19},
                {"chi", 0.30146},
                {"phi", 12.17548},
                {"v0max", 19.91643},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n11_B = new Dictionary<string, double>
            {
                {"popsize", 17},
                {"chi", 0.28648},
                {"phi", 12.90676},
                {"v0max", 0.30475},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n13_A = new Dictionary<string, double>
            {
                {"popsize", 18},
                {"chi", 0.24883},
                {"phi", 14.73917},
                {"v0max", 10.125},
                {"pxupdatemode", 1},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            //cluster 1
            var FIPSO_settings_n13_B = new Dictionary<string, double>
            {
                {"popsize", 20},
                {"chi", 0.20543},
                {"phi", 17.51249},
                {"v0max", 0.08502},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n18_A = new Dictionary<string, double>
            {
                {"popsize", 17},
                {"chi", 0.31809},
                {"phi", 11.75517},
                {"v0max", 11.25127},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n18_B = new Dictionary<string, double>
            {
                {"popsize", 21},
                {"chi", 0.43696},
                {"phi", 8.01647},
                {"v0max", 0.44891},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n20_A = new Dictionary<string, double>
            {
                {"popsize", 11},
                {"chi", 0.34133},
                {"phi", 11.36757},
                {"v0max", 0.23946},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n20_B = new Dictionary<string, double>
            {
                {"popsize", 24},
                {"chi", 0.40611},
                {"phi", 8.79538},
                {"v0max", 0},
                {"pxupdatemode", 0},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n35_A = new Dictionary<string, double>
            {
                {"popsize", 4},
                {"chi", 0.25452 },
                {"phi", 12.42113    },
                {"v0max", 0.30566},
                {"pxupdatemode", 1},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var FIPSO_settings_n35_B = new Dictionary<string, double>
            {
                {"popsize", 29},
                {"chi", 0.6803	},
                {"phi", 2.58573	},
                {"v0max", 0},
                {"pxupdatemode", 1},
                {"psomode", 0},
                {"phi1", 0},        //not used
                {"phi2", 0},        //not used
                { "itermax", 10000},
                {"seed", 1}
            };
            #endregion

            #region PSO
            var PSO_settings_untuned = new Dictionary<string, double>
            {
                {"popsize", 24},
                {"chi", 0.7298},    
                {"v0max", 0.4},
                {"pxupdatemode", 0},
                {"psomode", 1},
                {"phi1", 2.05},
                {"phi2",2.05},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n4_A = new Dictionary<string, double>
            {
                {"popsize", 15},
                {"chi", 0.33667},
                {"v0max", 0.18927},
                {"pxupdatemode", 0},
                {"psomode", 2},
                {"phi1", 4.11204},
                {"phi2",5},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n4_B = new Dictionary<string, double>
            {
                {"popsize", 17},
                {"chi", 0.28889},
                {"v0max", 0},
                {"pxupdatemode", 0},
                {"psomode", 2},
                {"phi1", 4.745},
                {"phi2",4.75793},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n10_A = new Dictionary<string, double>
            {
                {"popsize", 20},
                {"chi", 0.10542 },
                {"v0max", 18.76201},
                {"pxupdatemode", 0},
                {"psomode", 1},
                {"phi1", 1.42516    },
                {"phi2",2.03202},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n10_B = new Dictionary<string, double>
            {
                {"popsize", 25},
                {"chi", 0.35724	},
                {"v0max", 0.00805},
                {"pxupdatemode", 1},
                {"psomode", 2},
                {"phi1", 4.77251	},
                {"phi2",3.5374},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n11_A = new Dictionary<string, double>
            {
                {"popsize", 18},
                {"chi", 0.001},
                {"v0max", 19.64313},
                {"pxupdatemode", 1},
                {"psomode", 1},
                {"phi1", 0.55123},
                {"phi2",1.97214},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n11_B = new Dictionary<string, double>
            {
                {"popsize", 22},
                {"chi", 0.43853},
                {"v0max", 0.05017},
                {"pxupdatemode", 1},
                {"psomode", 2},
                {"phi1", 3.41835},
                {"phi2",3.04672},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            //cluster 1
            var PSO_settings_n13_A = new Dictionary<string, double>
            {
                {"popsize", 38},
                {"chi", 0.32361},
                {"v0max", 0},
                {"pxupdatemode", 0},
                {"psomode", 2},
                {"phi1", 2.40854},
                {"phi2",4.99429},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            //cluster 3
            var PSO_settings_n13_B = new Dictionary<string, double>
            {
                {"popsize", 30},
                {"chi", 0.39712},
                {"v0max", 0.05514},
                {"pxupdatemode", 1},
                {"psomode", 2},
                {"phi1", 4.82129},
                {"phi2",3.26303},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n18_A = new Dictionary<string, double>
            {
                {"popsize", 48},
                {"chi", 0.03098},
                {"v0max", 0.09856},
                {"pxupdatemode", 1},
                {"psomode", 1},
                {"phi1", 0},
                {"phi2",1.84805},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n18_B = new Dictionary<string, double>
            {
                {"popsize", 4},
                {"chi", 0.23885},
                {"v0max", 0.00012},
                {"pxupdatemode", 1},
                {"psomode", 2},
                {"phi1", 5},
                {"phi2",4.93995},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n20_A = new Dictionary<string, double>
            {
                {"popsize", 29},
                {"chi", 0.55973},
                {"v0max", 0},
                {"pxupdatemode", 1},
                {"psomode", 1},
                {"phi1", 1.79169},
                {"phi2",1.11233},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n20_B = new Dictionary<string, double>
            {
                {"popsize", 13},
                {"chi", 0.001},
                {"v0max", 10.10498},
                {"pxupdatemode", 0},
                {"psomode", 1},
                {"phi1", 0.51091},
                {"phi2",2.0331},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n35_A = new Dictionary<string, double>
            {
                {"popsize", 6},
                {"chi", 0.04556	},
                {"v0max", 0.03143},
                {"pxupdatemode", 1},
                {"psomode", 1},
                {"phi1", 0.6952	},
                {"phi2",2.08372},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };

            var PSO_settings_n35_B = new Dictionary<string, double>
            {
                {"popsize", 4},
                {"chi", 0.29269	},
                {"v0max", 0},
                {"pxupdatemode", 1},
                {"psomode", 2},
                {"phi1", 2.16103	},
                {"phi2",4.97884},
                {"phi", 0},         //not used
                { "itermax", 10000},
                {"seed", 1}
            };
            #endregion

            _presets.Add("FIPSO_unt", FIPSO_settings_untuned);
            _presets.Add("FIPSO_n4_A", FIPSO_settings_n4_A);
            _presets.Add("FIPSO_n4_B", FIPSO_settings_n4_B);
            _presets.Add("FIPSO_n10_A", FIPSO_settings_n10_A);
            _presets.Add("FIPSO_n10_B", FIPSO_settings_n10_B);
            _presets.Add("FIPSO_n11_A", FIPSO_settings_n11_A);
            _presets.Add("FIPSO_n11_B", FIPSO_settings_n11_B);
            _presets.Add("FIPSO_n13_A", FIPSO_settings_n13_A);
            _presets.Add("FIPSO_n13_B", FIPSO_settings_n13_B);
            _presets.Add("FIPSO_n18_A", FIPSO_settings_n18_A);
            _presets.Add("FIPSO_n18_B", FIPSO_settings_n18_B);
            _presets.Add("FIPSO_n20_A", FIPSO_settings_n20_A);
            _presets.Add("FIPSO_n20_B", FIPSO_settings_n20_B);
            _presets.Add("FIPSO_n35_A", FIPSO_settings_n35_A);
            _presets.Add("FIPSO_n35_B", FIPSO_settings_n35_B);
            _presets.Add("PSO_unt", PSO_settings_untuned);
            _presets.Add("PSO_n4_A", PSO_settings_n4_A);
            _presets.Add("PSO_n4_B", PSO_settings_n4_B);
            _presets.Add("PSO_n10_A", PSO_settings_n10_A);
            _presets.Add("PSO_n10_B", PSO_settings_n10_B);
            _presets.Add("PSO_n11_A", PSO_settings_n11_A);
            _presets.Add("PSO_n11_B", PSO_settings_n11_B);
            _presets.Add("PSO_n13_A", PSO_settings_n13_A);
            _presets.Add("PSO_n13_B", PSO_settings_n13_B);
            _presets.Add("PSO_n18_A", PSO_settings_n18_A);
            _presets.Add("PSO_n18_B", PSO_settings_n18_B);
            _presets.Add("PSO_n20_A", PSO_settings_n20_A);
            _presets.Add("PSO_n20_B", PSO_settings_n20_B);
            _presets.Add("PSO_n35_A", PSO_settings_n35_A);
            _presets.Add("PSO_n35_B", PSO_settings_n35_B);
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
                if (preset.Equals("FIPSO_n4_A") || preset.Equals("FIPSO_n4_B")
                    || preset.Equals("FIPSO_n10_A") || preset.Equals("FIPSO_n10_B")
                    || preset.Equals("FIPSO_n11_A") || preset.Equals("FIPSO_n11_B")
                    || preset.Equals("FIPSO_n13_A") || preset.Equals("FIPSO_n13_B")
                    || preset.Equals("FIPSO_n18_A") || preset.Equals("FIPSO_n18_B")
                    || preset.Equals("FIPSO_n20_A") || preset.Equals("FIPSO_n20_B")
                    || preset.Equals("FIPSO_n35_A") || preset.Equals("FIPSO_n35_B")
                    || preset.Equals("FIPSO_unt") 
                    || preset.Equals("PSO_n4_A") || preset.Equals("PSO_n4_B")
                    || preset.Equals("PSO_n10_A") || preset.Equals("PSO_n10_B")
                    || preset.Equals("PSO_n11_A") || preset.Equals("PSO_n11_B")
                    || preset.Equals("PSO_n13_A") || preset.Equals("PSO_n13_B")
                    || preset.Equals("PSO_n18_A") || preset.Equals("PSO_n18_B")
                    || preset.Equals("PSO_n20_A") || preset.Equals("PSO_n20_B")
                    || preset.Equals("PSO_n35_A") || preset.Equals("PSO_n35_B")
                    || preset.Equals("PSO_unt"))
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
                        Random rnd = new Random();
                        seed = rnd.Next();
                    }
                    int itermax = (int)settings["itermax"];

                    var pso = new MetaheuristicsLibrary.SingleObjective.ParticleSwarmOptimization(lb, ub, integer, itermax, eval, seed, PSOsettings);
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
