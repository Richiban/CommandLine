﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Richiban.CommandLine
{
    public class CommandLine
    {
        private readonly IObjectFactory _objectFactory;
        private readonly Action<string> _helpOutput;

        public CommandLine(IObjectFactory objectFactory, Action<string> helpOutput)
        {
            _objectFactory = objectFactory;
            _helpOutput = helpOutput;
        }

        public static void Run(string[] args)
        {
            var commandLine = new CommandLine(new SystemActivatorObjectFactory(), Console.WriteLine);

            commandLine.Execute(args);
        }

        public void Execute(string[] args)
        {
            var model = AssemblyModel.Scan(Assembly.GetEntryAssembly());
            var commandLineArgs = CommandLineArgumentList.Parse(args);

            var commandLineActions = new CommandLineActionFactory(model, _objectFactory)
                .Create(commandLineArgs);

            if (commandLineArgs.IsCallForHelp)
            {
                _helpOutput(GenerateHelp(model, commandLineArgs));

                return;
            }

            if (commandLineActions.Count == 0)
            {
                _helpOutput("Could not match the given arguments to a command");
                _helpOutput(GenerateHelp(model, commandLineArgs));

                return;
            }

            if (commandLineActions.Count > 1)
            {
                _helpOutput("The given arguments are ambigous between the following commands:");
                _helpOutput(GenerateHelp(model, commandLineArgs));

                return;
            }

            commandLineActions.Single().Execute();
        }

        private static string GenerateHelp(IEnumerable<MethodModel> model, CommandLineArgumentList commandLineArgs)
        {
            var sb = new StringBuilder();

            var modelsForHelp = model;

            if (commandLineArgs.Count == 0)
            {
                sb.AppendLine($"Usage:");
            }
            else
            {
                sb.AppendLine($"Help for {commandLineArgs}:");

                modelsForHelp = modelsForHelp
                    .MaxByAll(m => m.GetPartialMatchAccuracy(commandLineArgs));
            }

            sb.Append(
                string.Join($"{Environment.NewLine}{Environment.NewLine}",
                modelsForHelp
                .Select(t => $"\t{AppDomain.CurrentDomain.FriendlyName} {t.Help}")));

            return sb.ToString();
        }
    }
}
