class Rcurl < Formula
  desc "rcurl - A CLI tool"
  homepage "https://github.com/cemsubasi/rcurl"
  url "https://github.com/cemsubasi/rcurl/releases/download/v1.0.18/rcurl-osx-arm64.tar.gz"
  sha256 "63837ada3d5412aa5bed8d9a6d630b2b66805a79d1e5e18def10f1a8cc4fae82"
  

  def install
    bin.install "rcurl"
  end

  test do
    system "#{bin}/rcurl", "--version"
  end
end
