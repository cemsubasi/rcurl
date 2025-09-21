class Rcurl < Formula
  desc "rcurl - A CLI tool"
  homepage "https://github.com/cemsubasi/rcurl"
  url "https://github.com/cemsubasi/rcurl/releases/download/v1.0.20/rcurl-osx-arm64.tar.gz"
  sha256 "75ac98aecf061fdd37416a08406e38a25c5a0b3d2679c85b40a84c5eb39c7a69"
  

  def install
    bin.install "rcurl"
  end

  test do
    system "#{bin}/rcurl", "--version"
  end
end
