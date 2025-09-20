class Rcurl < Formula
  desc "rcurl - A CLI tool"
  homepage "https://github.com/cemsubasi/rcurl"
  url "https://github.com/cemsubasi/rcurl/releases/download/v1.0.18/rcurl-osx-arm64.tar.gz"
  sha256 "f8f44ffeed0c21f5c050d09c7687499fa10d3211f4e2d808d6037994982c5"
  

  def install
    bin.install "rcurl"
  end

  test do
    system "#{bin}/rcurl", "--version"
  end
end
