using CommandLine;
using Utils.Funcs;

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
