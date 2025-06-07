namespace CurlRedirector;

public class ConfigFileModel {
  public string Source { get; set; } = default!;

  public string Destination { get; set; } = default!;

  public static ConfigFileModel FromArg(string source, string destination) {
    return new ConfigFileModel {
      Source = source,
      Destination = destination,
    };
  }

  public static List<ConfigFileModel> FromArgs(List<string> args) {
    var fileModel = args.SkipLast(1).Select(x => ConfigFileModel.FromArg(x, args.Last())).ToList();

    return fileModel;
  }
}
