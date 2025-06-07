var web = new { remote = "lp-web-dev-app.logipoly.com", local = "localhost:5550" };
var auth = new { remote = "lp-auth-dev-app.logipoly.com", local = "localhost:5540" };
var mobile = new { remote = "lp-mobile-dev-app.logipoly.com", local = "localhost:5580" };
var admin = new { remote = "lp-admin-dev.logipoly.com", local = "localhost:5560" };

var input = args.ToList();
input = input.Select(x => x.Contains("curl") ? string.Empty : x).ToList();
input = input.Select(x => x.Contains("https") ? x.Replace("https", "http") : x).ToList();
input = input.Select(x => x.Contains(web.remote) ? x.Replace(web.remote, web.local) : x.Contains(auth.remote) ? x.Replace(auth.remote, auth.local) : x.Contains(mobile.remote) ? x.Replace(mobile.remote, mobile.local) : x.Contains(admin.remote) ? x.Replace(admin.remote, admin.local) : x).ToList();

/* input.ForEach(Console.WriteLine); */

System.Diagnostics.Process.Start("curl", input);
