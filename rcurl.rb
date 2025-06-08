class Rcurl < Formula
  desc "rcurl - A CLI tool"
  homepage "https://github.com/cemsubasi/rcurl"
  url "https://github.com/cemsubasi/rcurl/releases/download/v1.0.13/rcurl-osx-arm64.tar.gz"
  sha256 "12d54c99898bc838a104ec1db703ed010df57aa032023c9fd2293044c1abfd81"

  def install
    bin.install "rcurl"
  end

  test do
    system "#{bin}/rcurl", "--version"
  end
end