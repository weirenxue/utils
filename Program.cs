using CommandLine;
using System;
using Utils.Libs;

namespace Utils
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Hash.Options, Compress.SeperateOptions>(args)
                .WithParsed<Hash.Options>(options => new Hash(options))
                .WithParsed<Compress.SeperateOptions>(options => new Compress(options));
        }
    }
}
