﻿using System;
using System.Collections.Generic;
using System.Linq;
using Rejuvena.Collate.Util;
using TML.Files.Extraction;

namespace Rejuvena.Collate.Packing.References;

/// <summary>
///     An implementation of <see cref="IReferencesProvider"/> which combines the references provided by the given <see cref="CombinedProvider{TProvider}.Providers"/>.
/// </summary>
public class ReferencesProvider : CombinedProvider<IReferencesProvider>, IReferencesProvider
{
    public IEnumerable<ModReference> GetModReferences() {
        var references = new Dictionary<string, List<Version?>>();

        foreach (var modRef in Providers.SelectMany(x => x.GetModReferences()))
            (references.ContainsKey(modRef.Mod) ? references[modRef.Mod] : references[modRef.Mod] = new List<Version?>()).Add(modRef.TargetVersion);

        return references.Select(x => new ModReference(x.Key, x.Value.Max()));
    }

    public IEnumerable<string> GetAssemblyReferences() {
        return hashUnion(provider => provider.GetAssemblyReferences());
    }

    public IEnumerable<string> GetPackageReferences() {
        // TODO: Handle versions here as well.
        return hashUnion(provider => provider.GetPackageReferences());
    }

    private IEnumerable<string> hashUnion(Func<IReferencesProvider, IEnumerable<string>> func) {
        var refs = new HashSet<string>();

        foreach (var provider in Providers) refs.UnionWith(func(provider));
        
        return refs;
    }
}
