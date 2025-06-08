
# rcurl

**rcurl** is a lightweight wrapper around `curl` designed to simplify redirecting HTTP requests by hostname. The name stands for **redirect curl**.

---

## What is rcurl?

When working with local API testing or complex HTTP requests, it’s often tedious to manually modify curl commands for different hosts or endpoints. **rcurl** helps you manage this easily by allowing you to define hostname redirects.

For example, if you add a redirect from host `x` to `y`, then any curl command containing `x` will automatically be redirected to `y` when executed via rcurl.

Additionally, rcurl beautifies and colorizes JSON output for better readability in the terminal.

---

## Key Features

- Redirect HTTP requests from one hostname to another on the fly  
- Simplifies local API testing by reducing manual URL changes  
- Parses and formats JSON responses with syntax highlighting  
- Lightweight, self-contained, and fast CLI tool  
- Currently supports **macOS ARM64** (Apple Silicon)  
- Code-signed and notarized for macOS security compliance  
- Homebrew formula for easy installation on macOS  

---

## How It Works

You can add a redirect rule like this:

```
rcurl redirect add <source-hostname> <target-hostname>
```

For example:

```
rcurl redirect add api.local staging.api.com
```

Now, if you run any curl command that includes `api.local`, rcurl will automatically replace `api.local` with `staging.api.com` before executing the request.

---

## Installation

### Using Homebrew (macOS ARM64)

```
brew tap cemsubasi/rcurl https://github.com/cemsubasi/homebrew-rcurl
brew install rcurl
```

### Manual Download

Download the prebuilt macOS ARM64 binary from the [Releases](https://github.com/your-repo/rcurl/releases) page.

---

## Usage Examples

Add a redirect rule:

```
rcurl redirect add example.com api.example.com
```

Run a curl command through rcurl:

```
rcurl curl -X GET https://example.com/api/data
```

The request will actually be sent to `https://api.example.com/api/data`.

---

## Development & Build

Build locally for macOS ARM64:

```
dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true -o release
```

---

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests.

---

## License

MIT License — see the LICENSE file for details.

---

## Contact

Created and maintained by Cem Subaşı  
GitHub: [@cemsubasi](https://github.com/cemsubasi)  
Email: cemsubasi@me.com
