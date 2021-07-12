using CommandLine;
using System;
using Utils.Funcs;

namespace Utils
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Hash.Options, Compress.SeperateOptions, Transfer.Options>(args)
                .WithParsed<Hash.Options>(options => new Hash(options))
                .WithParsed<Compress.SeperateOptions>(options => new Compress(options))
                .WithParsed<Transfer.Options>(options => new Transfer(options));
        }
    }
}
