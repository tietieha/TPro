using System;
using System.Collections.Generic;

public interface IMessageQueue : IDisposable
{
    void Add(byte[] o);

    void MoveTo(List<byte[]> bytesList);

    bool Empty();
}

