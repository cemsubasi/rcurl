using System.CommandLine;
using System.Diagnostics;
using System.Text.Json;

namespace CurlRedirector;

public partial class Commands {
  public async static Task MainCommand(List<string> args) {
    var lines = System.IO.File.ReadAllLines(Program.ConfPath);
    var defaultRedirects = JsonSerializer.Deserialize<List<ConfigFileModel>>(string.Join(Environment.NewLine, lines));

    string ReplaceWithLocal(string input) {
      foreach (var each in defaultRedirects) {
        if (input.Contains(each.Source)) {
          return input.Replace(each.Source, each.Destination);
        }
      }
      return input;
    }

    var input = args.ToList();

    input = input.Where(x => x != "curl").ToList();

    if (defaultRedirects is not null && defaultRedirects.Any()) {
      input = input.Select(ReplaceWithLocal).ToList();
    }

    string argsString = string.Join(" ", input.Select(arg =>
        arg.Contains(' ') || arg.Contains('"') ? $"\"{arg.Replace("\"", "\\\"")}\"" : arg
    ));

    var psi = new ProcessStartInfo {
      FileName = "curl",
      Arguments = argsString,
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      UseShellExecute = false
    };

    var process = Process.Start(psi)!;
    var output = process.StandardOutput.ReadToEnd();
    var error = process.StandardError.ReadToEnd();
    process.WaitForExit();

    Console.WriteLine(output);
    if (!string.IsNullOrEmpty(error))
      Console.Error.WriteLine(error);

    var jqpsi = new ProcessStartInfo {
      FileName = "jq",
      Arguments = "-C .",
      RedirectStandardInput = true,
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      UseShellExecute = false
    };

    var jqProcess = Process.Start(jqpsi)!;

    await jqProcess.StandardInput.WriteAsync(output);
    jqProcess.StandardInput.Close();

    var jqOutput = await jqProcess.StandardOutput.ReadToEndAsync();
    var jqError = await jqProcess.StandardError.ReadToEndAsync();

    jqProcess.WaitForExit();

    Console.WriteLine(jqOutput);
    if (!string.IsNullOrEmpty(jqError))
      Console.Error.WriteLine(jqError);

    /* var command = new Command("main", "Main command to handle curl requests and redirect them") */
    /* { */
    /*   new Argument<List<string>>("args", "Arguments for the curl command") */
    /* }; */

    /* return command; */
  }
}

