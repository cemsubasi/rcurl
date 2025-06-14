using System.CommandLine;
using System.Diagnostics;
using System.Text.Json;

namespace CurlRedirector;

public partial class Commands {
  public async static Task MainCommand(List<string> args) {
    var isConfFileExists = System.IO.File.Exists(Program.ConfPath);
    var defaultRedirects = new List<ConfigFileModel>();

    if (isConfFileExists) {
      var lines = System.IO.File.ReadAllLines(Program.ConfPath);
      defaultRedirects = JsonSerializer.Deserialize<List<ConfigFileModel>>(string.Join(Environment.NewLine, lines));
    }

    var input = args.ToList();

    input = input.Where(x => x != "curl").ToList();

    if (defaultRedirects is not null && defaultRedirects.Any()) {
      input = input.Select(x => Helper.ReplaceRedirects(x, defaultRedirects)).ToList();
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
    if (!string.IsNullOrEmpty(error)) {
      Console.Error.WriteLine(error);
      return;
    }

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
  }
}

