class Rcurl < Formula
  desc "rcurl - A CLI tool"
  homepage "https://github.com/cemsubasi/rcurl"
  url "https://github.com/cemsubasi/rcurl/releases/download/v1.0.17/rcurl-osx-arm64.tar.gz"
  sha256 "e6be1cf8e6d8a3a21159517b2592243ed126036f24a665e32646b40cc95adbe7"
  

  def install
    bin.install "rcurl"
  end

  test do
    system "#{bin}/rcurl", "--version"
  end
end
