﻿using System;
using System.Collections.Generic;
using static Richiban.CommandLine.Prelude;

namespace Richiban.CommandLine
{
    abstract class ParameterMapping
    {
        private ParameterMapping(
            ParameterModel parameterModel,
            IReadOnlyList<string> suppliedValues)
        {
            SuppliedValues = suppliedValues;
            ConvertToType = parameterModel.ParameterType;
        }

        public IReadOnlyList<string> SuppliedValues { get; }
        public Type ConvertToType { get; }

        public sealed class NamedValue : ParameterMapping
        {
            public NamedValue(
                ParameterModel parameterModel, IReadOnlyList<string> suppliedValues)
                : base(parameterModel, suppliedValues)
            {
            }
        }

        public sealed class PositionalValue : ParameterMapping
        {
            public PositionalValue(
                ParameterModel parameterModel, IReadOnlyList<string> suppliedValues) 
                : base(parameterModel, suppliedValues)
            {
            }
        }

        public sealed class NoValue : ParameterMapping
        {
            public NoValue(ParameterModel parameterModel)
                : base(parameterModel, ListOf<string>())
            {
            }
        }

        public sealed class Flag : ParameterMapping
        {
            public Flag(ParameterModel parameterModel)
                : base(parameterModel, ListOf($"{true}"))
            {
            }
        }
    }
}