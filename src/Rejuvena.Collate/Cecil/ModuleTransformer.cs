﻿using Microsoft.Build.Utilities;
using Mono.Cecil;

namespace Rejuvena.Collate.Cecil
{
    public abstract class ModuleTransformer<TContext> where TContext : class
    {
        public abstract bool Transform(TaskLoggingHelper log, ModuleDefinition module, TContext ctx);
    }
}