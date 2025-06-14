class Rcurl < Formula
  desc "rcurl - A CLI tool"
  homepage "https://github.com/cemsubasi/rcurl"
  url "https://github.com/cemsubasi/rcurl/releases/download/v1.0.16/rcurl-osx-arm64.tar.gz"
  sha256 "b1e188e6057f5c39a947983958c3435559a4521ac21c39dee213574efcec14e7"
  

  def install
    bin.install "rcurl"
  end

  test do
    system "#{bin}/rcurl", "--version"
  end
end
