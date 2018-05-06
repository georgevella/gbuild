using System.Collections.Generic;
using System.Data.SqlTypes;
using CommandLine;

namespace GBuild.Console.VerbOptions
{
    [Verb("init", HelpText = "Creates a new repository or updates the current git repository with gbuild support.")]
    public class InitOptions
    {
//        [Option('d', "useDefaults", Default = true, HelpText = "Initialize current repository with default settings (GitFlow).")]
//        public bool UseDefaults { get; set; }

        [Option("overwrite", Default = false, HelpText = "Overwrites any existing configuration.")]
        public bool Overwrite { get; set; }
    }
}