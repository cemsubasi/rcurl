namespace CurlRedirector;

public static class Helper {
  public static bool AskYesNo(string question) {
    while (true) {
      Console.Write($"{question} (y/n): ");
      var key = Console.ReadKey(intercept: true);
      if (key.Key == ConsoleKey.Y) {
        Console.WriteLine("y");
        return true;
      } else if (key.Key == ConsoleKey.N) {
        Console.WriteLine("n");
        return false;
      } else {
        Console.WriteLine("Invalid input. Please press 'y' for yes or 'n' for no.");
      }
    }
  }

  public static void PrintRedirectTable(List<ConfigFileModel> redirects) {
    string separator = new string('-', 90);
    Console.WriteLine();
    Console.WriteLine(separator);
    Console.WriteLine($"| {"Id",-3} | {"Source",-45} | {"Destination",-30} |");
    Console.WriteLine(separator);

    foreach (var redirect in redirects) {
      Console.WriteLine($"| {redirect.Id,-3} | {Truncate(redirect.Source, 45),-45} | {Truncate(redirect.Destination, 30),-30} |");
    }

    Console.WriteLine(separator);
  }

  public static string Truncate(string input, int maxLength) {
    if (string.IsNullOrWhiteSpace(input)) return "";
    return input.Length <= maxLength ? input : input.Substring(0, maxLength - 3) + "...";
  }
}
