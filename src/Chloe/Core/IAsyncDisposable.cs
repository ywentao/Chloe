﻿#if netfx
using System;

namespace System
{
    internal interface IAsyncDisposable
    {
        ValueTask DisposeAsync();
    }
}
#endif
