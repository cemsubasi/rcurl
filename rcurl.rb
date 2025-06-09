class Rcurl < Formula
  desc "rcurl - A CLI tool"
  homepage "https://github.com/cemsubasi/rcurl"
  url "https://github.com/cemsubasi/rcurl/releases/download/v1.0.15/rcurl-osx-arm64.tar.gz"
  sha256 "5bbe2c4a10703e9fd85b8c039ef946f05ae2f9a4e64455b8d97a629a6acbdae0"

  def install
    bin.install "rcurl"
  end

  test do
    system "#{bin}/rcurl", "--version"
  end
end
