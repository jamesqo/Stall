# Stall for Windows

[![Travis CI](https://travis-ci.org/jamesqo/Stall.svg?branch=master)](https://travis-ci.org/jamesqo/Stall) [![AppVeyor](https://ci.appveyor.com/api/projects/status/github/jamesqo/Stall?branch=master&svg=true)](https://ci.appveyor.com/project/jamesqo/Stall) [![BSD 2-Clause License](https://img.shields.io/badge/license-bsd%202.0-blue.svg?style=flat)](bsd.license)

Ever downloaded a program that didn't come with an installer? **Stall** is a command-line tool that can automatically install, or remove, your favorite Windows desktop apps.

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

## Usage

The format of commands should be: `stall -e program [-i icon] [options] folder`

- `-e` is the executable of your program, e.g. `YourApp.exe`.
- `-i` is the icon file to display on shortcuts, e.g. `icon.ico`. JPG and PNG are not accepted *yet*.
- `folder` is your app's root folder. It should contain all of your app's files, e.g. DLLs and config files.

By default, the paths of `program` and `icon` are relative to the root folder. If you wish to override this behavior, prepend `:` to your path to make it relative to the current directory (or absolute).

Type `stall --help` for more usage.

## Example

**ILSpy** is one program that doesn't come with an installer (which in fact was the initial motivation for Stall). After you download and unzip the binaries from [their site](http://ilspy.net), running

```bash
cd ~/Downloads/ILSpy
stall . -e ILSpy.exe -i ILSpy.exe
```

should install ILSpy.

If you want to make it show up nicely in Control Panel, you could alternatively run

```bash
stall . -e ILSpy.exe -i ILSpy.exe --project-url=ilspy.net -p IC#Code --releases-url=github.com/icsharpcode/ILSpy/releases -v 2.3.1
```

## Removing Apps

Have an app, or apps, installed that you'd like to remove? Running

```bash
stall -un names,of,the,apps
```

will uninstall them.

Unfortunately, the names of the apps need to be the ones listed in the Control Panel, so this is less intuitive than it sounds.

## FAQ

### What's the difference between Stall and [Squirrel](https://github.com/Squirrel/Squirrel.Windows)?

In short, Squirrel is meant to be a framework for self-updating apps. Stall can install apps without having to modify the code of the app itself, but doesn't manage app updates.

### I followed the steps [here](#getting-started), but Stall isn't in my `PATH`.

Usually, opening a new command prompt window should fix this.

### What if my app icon is embedded in the .exe file?

Try

```bash
stall -e YourApp.exe -i YourApp.exe path/to/YourApp
```
