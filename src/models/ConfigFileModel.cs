namespace CurlRedirector;

public class ConfigFileModel {
  public uint Id { get; set; }

  public string Source { get; set; } = default!;

  public string Destination { get; set; } = default!;

  public static ConfigFileModel FromArg(string source, string destination, uint id) {
    return new ConfigFileModel {
      Id = id,
      Source = source,
      Destination = destination,
    };
  }

  public static List<ConfigFileModel> FromArgs(List<string> args) {
    var fileModel = args.SkipLast(1).Select((x, i) => ConfigFileModel.FromArg(x, args.Last(), (uint)i)).ToList();

    return fileModel;
  }
}
