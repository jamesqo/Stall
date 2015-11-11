# Stall

Ever downloaded an EXE, but couldn't install it? **Stall** is a command-line tool that can automatically install, or remove, your favorite Windows desktop apps.

## Usage

```bash
stall -e YourApp.exe path/to/YourApp
```

## Getting Started

The binaries for Stall aren't available yet, so you'll have to build from source.

First clone the repo:

```bash
git clone https://github.com/jamesqo/Stall.git
```

Then open up **Stall.sln** in Visual Studio, toggle the configuration to **Release**, and build.

Once that's finished, the binaries should be in `Stall/bin/Release`. Navigate there in CMD and run

```cmd
mkdir Stall
robocopy . Stall stall.exe
cd Stall
stall . -e stall.exe --script
```

Congratulations! You've just installed Stall using itself.

## FAQ

### I followed the steps [here](#getting-started), but Stall isn't in my `PATH`.

Open up a new command prompt and try again.
