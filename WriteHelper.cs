using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolegomenon
{
    public static class WriteHelper
    {
        public static async Task WriteFileAsync(string path, string text)
        {
            await writeLock.WaitAsync();
            try
            {
                using StreamWriter sw = new(path, true);
                await sw.WriteLineAsync(text);
            }
            finally
            {
                writeLock.Release();
            }
        }
        private static readonly SemaphoreSlim writeLock = new(1, 1);
    }
}
