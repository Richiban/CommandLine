﻿namespace Richiban.CommandLine.Tests.DependencyInjection
{
    class SomeDependency : ISomeDependency
    {
        public string SomeMessage => "This is a message from the dependency";
    }
}
