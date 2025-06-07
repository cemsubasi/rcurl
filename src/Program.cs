using System.CommandLine;

namespace CurlRedirector;

public static class Program {
  public const string ConfigFileName = ".mcurl.conf";
  public static readonly string ConfPath = Path.Combine(Environment.CurrentDirectory, ConfigFileName);

  public static int Main(string[] args) {
    var rootCommand = new RootCommand("A simple command line application to redirect curl requests.");
    var argsArgument = new Argument<List<string>>("args") {
      Arity = ArgumentArity.ZeroOrMore

    };

    rootCommand.AddArgument(argsArgument);

    rootCommand.SetHandler(async (args) => {
      await Commands.MainCommand(args.ToList());
    }, argsArgument);

    rootCommand.AddCommand(Commands.RedirectCommand());


    return rootCommand.Invoke(args);
  }
}

