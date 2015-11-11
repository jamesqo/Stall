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

## More Usage

The format of commands should be: `stall -e program [-i icon] [options] folder`

- `-e` is the executable of your program, e.g. `YourApp.exe`.
- `-i` is the icon file to display on shortcuts, e.g. `icon.ico`. JPG and PNG are not accepted *yet*.
- `folder` is your app's root folder. It should contain all of your app's files, e.g. DLLs and config files.

By default, the paths of `program` and `icon` are relative to the root folder. If you wish to override this behavior, prepend `:` to your path to make it relative to the current directory (or absolute).

Type `stall --help` for more help.

## FAQ

### I followed the steps [here](#getting-started), but Stall isn't in my `PATH`.

Open a new command prompt and try again.
