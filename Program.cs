using System.Diagnostics;

var remoteToLocal = new Dictionary<string, string>
{
    { "https://lp-web-dev-app.logipoly.com", "http://localhost:5550" },
    { "https://lp-auth-dev-app.logipoly.com", "http://localhost:5540" },
    { "https://lp-mobile-dev-app.logipoly.com", "http://localhost:5580" },
    { "https://lp-admin-dev.logipoly.com", "http://localhost:5560" },
};

string ReplaceWithLocal(string input) {
  foreach (var kvp in remoteToLocal) {
    if (input.Contains(kvp.Key)) {
      return input.Replace(kvp.Key, kvp.Value);
    }
  }
  return input;
}

var input = args.ToList();

input = input.Where(x => x != "curl").ToList();

input = input.Select(ReplaceWithLocal).ToList();

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
jqProcess.StandardInput.Close(); // stdin'i kapatmazsan jq beklemede kalabilir

var jqOutput = await jqProcess.StandardOutput.ReadToEndAsync();
var jqError = await jqProcess.StandardError.ReadToEndAsync();

jqProcess.WaitForExit();

Console.WriteLine(jqOutput);
if (!string.IsNullOrEmpty(jqError))
  Console.Error.WriteLine(jqError);
