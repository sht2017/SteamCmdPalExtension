// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SteamCmdPalExtension;

[Guid("53a8e562-344d-463f-9798-5d675c7cc518")]
public sealed partial class Extension(ManualResetEvent extensionDisposedEvent) : IExtension, IDisposable
{
    private readonly ManualResetEvent _extensionDisposedEvent = extensionDisposedEvent;

    private readonly CommandsProvider _provider = new();

    public object? GetProvider(ProviderType providerType)
    {
        return providerType switch
        {
            ProviderType.Commands => _provider,
            _ => null,
        };
    }

    public void Dispose() => _extensionDisposedEvent.Set();
}
