using System.Diagnostics;

var web = new { remote = "https://lp-web-dev-app.logipoly.com", local = "http://localhost:5550" };
var auth = new { remote = "https://lp-auth-dev-app.logipoly.com", local = "http://localhost:5540" };
var mobile = new { remote = "https://lp-mobile-dev-app.logipoly.com", local = "http://localhost:5580" };
var admin = new { remote = "https://lp-admin-dev.logipoly.com", local = "http://localhost:5560" };

var input = args.ToList();
/* var custom = new List<string>(); */
input = input.Select(x => x.Contains("curl ") ? string.Empty : x).ToList();
input = input.Select(x => x.Contains("https") ? $"{x}" : x).ToList();
input = input.Select(x => x.Contains(web.remote) ? x.Replace(web.remote, web.local) : x.Contains(auth.remote) ? x.Replace(auth.remote, auth.local) : x.Contains(mobile.remote) ? x.Replace(mobile.remote, mobile.local) : x.Contains(admin.remote) ? x.Replace(admin.remote, admin.local) : x).ToList();
/* custom.Add(input.First(x => x.Contains("http"))); */
/* if (input.Any(x => x.StartsWith("{"))) { */
/*   custom.Add($"--data-raw {input.First(x => x.StartsWith("{")).ToString()}"); */
/* } */

/* var index = 0; */
/* while (!index.Equals(-1)) { */
/*   Console.WriteLine(index); */
/*   index = input.IndexOf("-H", index + 1); */
/*   if (index.Equals(-1)) { */
/*     continue; */
/*   } */

/*   var nextIndex = input.IndexOf("-", index); */
/*   if (!nextIndex.Equals(-1)) { */
/*     custom.Add($"'{string.Join(' ', input.Where((x, i) => i >= index && i < nextIndex))}'"); */
/*   } */
/* } */

/* if (input.Any(x => x.Equals("--jsonpp"))) { */
/*   input = input.Select(x => x.Equals("--jsonpp") ? string.Empty : x).ToList(); */
/* custom.Add(" |json_pp"); */
/* } */

/* custom.ForEach(Console.WriteLine); */
/* input = input.Select(x => !x.StartsWith('-') ? '\'' + x + '\'' : x).ToList(); */
input = input.Select(x => !x.StartsWith('-') ? '\'' + x + '\'' + " \\\n" : x).ToList();
/* Console.WriteLine("curl " + string.Join(' ', input)); */

var psi = new ProcessStartInfo();
psi.FileName = "curl";
psi.Arguments = string.Join(' ', input);
psi.RedirectStandardOutput = true;
var process = Process.Start(psi);
var result = process!.StandardOutput.ReadToEnd();
Console.WriteLine(result);

System.Diagnostics.Process.Start(psi);

/* Console.WriteLine(psi.Arguments); */
