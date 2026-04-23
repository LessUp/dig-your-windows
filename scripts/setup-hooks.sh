#!/usr/bin/env bash
# Installs project git hooks. Run once after cloning:
#   bash scripts/setup-hooks.sh

set -e

REPO_ROOT="$(git -C "$(dirname "$0")" rev-parse --show-toplevel)"
HOOKS_DIR="$REPO_ROOT/.git/hooks"
SCRIPTS_DIR="$REPO_ROOT/scripts"

cp "$SCRIPTS_DIR/pre-commit.sh" "$HOOKS_DIR/pre-commit"
chmod +x "$HOOKS_DIR/pre-commit"

echo "✅ Git hooks installed:"
echo "   pre-commit → scripts/pre-commit.sh (dotnet build check)"
