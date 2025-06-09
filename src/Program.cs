using System.CommandLine;

namespace CurlRedirector;

public static class Program {
  public const string ConfigFileName = ".rcurl.conf";
  public static readonly string ConfPath = Path.Combine(Environment.CurrentDirectory, ConfigFileName);

  public static int Main(string[] args) {
    var rootCommand = new RootCommand("A simple command line application to redirect curl requests.");
    var argsArgument = new Argument<List<string>>("args") {
      Arity = ArgumentArity.ZeroOrMore
    };

    rootCommand.AddValidator((result) => {
      if (result.Children.Count == 0) {
        result.ErrorMessage = "You must provide at least one argument.";
      }
    });

    rootCommand.AddArgument(argsArgument);

    rootCommand.SetHandler(async (args) => {
      await Commands.MainCommand(args.ToList());
    }, argsArgument);

    rootCommand.AddCommand(Commands.RedirectCommand());


    return rootCommand.Invoke(args);
  }
}

