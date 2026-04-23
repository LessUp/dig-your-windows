#!/usr/bin/env bash
# Pre-commit hook: ensures the solution builds with zero warnings/errors.
# Install via: bash scripts/setup-hooks.sh

set -e

echo "🔍 Pre-commit: dotnet build check..."
dotnet build DigYourWindows.slnx -c Release --no-restore -v quiet 2>&1

echo "✅ Build passed — proceeding with commit"
