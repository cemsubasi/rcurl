using System.CommandLine;
using System.Text.Json;

namespace CurlRedirector;

public partial class Commands {
  public static Command RedirectCommand() {
    var rc = new Command("redirect", "Redirects host names to targeted hosts");
    var arguments = new Argument<List<string>>();
    arguments.AddValidator(args => {
      if (args.Tokens.Count < 2) {
        args.ErrorMessage = "You must specify at least two hostname";
      }
    });

    rc.AddArgument(arguments);

    rc.SetHandler((args) => {
      var options = new JsonSerializerOptions {
        WriteIndented = true,
      };

      // read conf file 
      var isExist = System.IO.File.Exists(Program.ConfPath);
      if (!isExist) {
        var stream = System.IO.File.Create(Program.ConfPath);
        stream.Close();
      }

      // validate args
      if (args.Count < 2) {
        Console.WriteLine("You must specify at least two hostname");
        return;
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
          Console.WriteLine("You have already added this source to the config");
          return;
        }

        var newRedirects = ConfigFileModel.FromArgs(args);
        if (newRedirects == null || newRedirects.Count == 0) {
          Console.WriteLine("No valid redirects found in the arguments.");
          return;
        }

        // parse args 
        parsedConf.AddRange(newRedirects);
        parsedConf.DistinctBy(x => x.Source).ToList();

        // save args
        System.IO.File.Delete(Program.ConfPath);
        using var stream = System.IO.File.Create(Program.ConfPath);
        JsonSerializer.Serialize(stream, parsedConf, options);
        Console.WriteLine("Redirects added successfully.");
        Console.WriteLine($"Current redirects: {JsonSerializer.Serialize(parsedConf, options)}");
        stream.Close();
      } catch (Exception ex) {
        Console.WriteLine($"An error occurred: {ex.Message}");
        return;
      }
    }, arguments);

    return rc;
  }
}
