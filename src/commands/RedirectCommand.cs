using System.CommandLine;
using System.Text.Json;

namespace CurlRedirector;

public partial class Commands {
  public static Command RedirectCommand() {
    var rc = new Command("redirect", "Redirects host names to targeted hosts");
    var addArguments = new Argument<List<string>>();
    addArguments.AddValidator(args => {
      if (args.Tokens.Count < 2) {
        args.ErrorMessage = "You must specify at least two hostname";
      }
    });

    var removeArguments = new Argument<List<string>>();
    removeArguments.AddValidator(args => {
      if (args.Tokens.Count < 1) {
        args.ErrorMessage = "You must specify at least one hostname to remove";
      }
    });

    var addSubCommand = new Command("add", "Adds a redirect to the config file");
    addSubCommand.AddArgument(addArguments);
    addSubCommand.SetHandler(AddRedirectHandler, addArguments);

    var removeSubCommand = new Command("remove", "Removes a redirect from the config file");
    removeSubCommand.AddArgument(removeArguments);
    removeSubCommand.SetHandler(RemoveRedirectHandler, removeArguments);
    removeSubCommand.AddAlias("rm");

    var listSubCommand = new Command("list", "Lists all redirects in the config file");
    listSubCommand.SetHandler(ListRedirectHandler);
    listSubCommand.AddAlias("ls");

    /* rc.AddArgument(arguments); */
    rc.AddCommand(addSubCommand);
    rc.AddCommand(removeSubCommand);
    rc.AddCommand(listSubCommand);

    /* rc.SetHandler(RedirectHandler, arguments); */

    return rc;
  }

  public static void AddRedirectHandler(List<string> args) {
    var options = new JsonSerializerOptions {
      WriteIndented = true,
    };

    // read conf file 
    var isExist = System.IO.File.Exists(Program.ConfPath);
    if (!isExist) {
      var stream = System.IO.File.Create(Program.ConfPath);
      stream.Close();
    }

    try {
      var conf = System.IO.File.ReadAllBytes(Program.ConfPath);
      if (conf.Length == 0) {
        Console.WriteLine("Config file is empty, creating a new one.");
        conf = JsonSerializer.SerializeToUtf8Bytes(new List<ConfigFileModel>(), options);
      }

      var parsedConf = JsonSerializer.Deserialize<List<ConfigFileModel>>(conf);
      if (parsedConf == null) {
        parsedConf = new List<ConfigFileModel>();
      }

      if (parsedConf.Any(x => args.SkipLast(1).Any(y => x.Source == y))) {
        Console.WriteLine("You have already added this source to the config file.");
        Console.WriteLine("Current redirects: " + parsedConf.Where(x => args.SkipLast(1).Any(y => x.Source == y)).Select(x => $"{x.Source} -> {x.Destination}").Single());
        var overwriteResult = Helper.AskYesNo("Do you want to overwrite the existing redirect?");
        if (!overwriteResult) {
          Console.WriteLine("Redirects not added.");
          return;
        } else {
          Console.WriteLine("Overwriting existing redirects.");
          parsedConf.RemoveAll(x => args.SkipLast(1).Any(y => x.Source == y));
        }
      }

      var newRedirects = ConfigFileModel.FromArgs(args);
      if (newRedirects == null || newRedirects.Count == 0) {
        Console.WriteLine("No valid redirects found in the arguments.");
        return;
      }

      // parse args 
      parsedConf.AddRange(newRedirects);
      parsedConf = parsedConf.Select((x, i) => ConfigFileModel.FromArg(x.Source, x.Destination, (uint)i)).ToList();

      // save args
      System.IO.File.Delete(Program.ConfPath);
      using var stream = System.IO.File.Create(Program.ConfPath);
      JsonSerializer.Serialize(stream, parsedConf, options);
      Console.WriteLine("Redirects added successfully.");
      Helper.PrintRedirectTable(parsedConf);
      stream.Close();
    } catch (Exception ex) {
      Console.WriteLine($"An error occurred: {ex.Message}");
      return;
    }
  }

  public static void RemoveRedirectHandler(List<string> args) {
    var options = new JsonSerializerOptions {
      WriteIndented = true,
    };

    // read conf file 
    var isExist = System.IO.File.Exists(Program.ConfPath);
    if (!isExist) {
      using var stream = System.IO.File.Create(Program.ConfPath);
      stream.Close();

      Console.WriteLine("Config file does not exist. Nothing to remove.");
      return;
    }

    var conf = System.IO.File.ReadAllBytes(Program.ConfPath);
    if (conf.Length == 0) {
      Console.WriteLine("Config file is empty. Nothing to remove.");
      return;
    }

    var parsedConf = JsonSerializer.Deserialize<List<ConfigFileModel>>(conf);
    if (parsedConf == null) {
      parsedConf = new List<ConfigFileModel>();
    }

    var redirectsToRemove = parsedConf.Where(x => args.Any(y => x.Source == y || x.Id.ToString() == y)).ToList();
    if (!redirectsToRemove.Any()) {
      Console.WriteLine("No redirects found to remove.");
      return;
    }

    parsedConf.RemoveAll(x => args.Any(y => x.Source == y || x.Id.ToString() == y));
    parsedConf = parsedConf.Select((x, i) => ConfigFileModel.FromArg(x.Source, x.Destination, (uint)i)).ToList();
    System.IO.File.Delete(Program.ConfPath);
    using var createStream = System.IO.File.Create(Program.ConfPath);
    JsonSerializer.Serialize(createStream, parsedConf, options);
    Console.WriteLine("Redirects removed successfully.");
    Helper.PrintRedirectTable(parsedConf);
    createStream.Close();
  }

  public static void ListRedirectHandler() {
    var options = new JsonSerializerOptions {
      WriteIndented = true,
    };

    var isExist = System.IO.File.Exists(Program.ConfPath);
    if (!isExist) {
      Console.WriteLine("Config file does not exist. Nothing to list.");
      return;
    }

    var conf = System.IO.File.ReadAllBytes(Program.ConfPath);
    if (conf.Length == 0) {
      Console.WriteLine("Config file is empty. Nothing to list.");
      return;
    }

    var parsedConf = JsonSerializer.Deserialize<List<ConfigFileModel>>(conf);
    if (parsedConf == null || !parsedConf.Any()) {
      Console.WriteLine("No redirects found in the config file.");
      return;
    }

    Helper.PrintRedirectTable(parsedConf);
  }
}
