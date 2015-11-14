# Stall

![Logo](icons/icon_236_217.jpg)

[![Travis CI](https://travis-ci.org/jamesqo/Stall.svg?branch=master)](https://travis-ci.org/jamesqo/Stall) [![AppVeyor](https://ci.appveyor.com/api/projects/status/github/jamesqo/Stall?branch=master&svg=true)](https://ci.appveyor.com/project/jamesqo/Stall) [![BSD 2-Clause License](https://img.shields.io/badge/license-bsd%202.0-blue.svg?style=flat)](bsd.license)

Ever downloaded a program that didn't come with an installer? **Stall** is a command-line tool that can automagically install, or remove, your favorite Windows desktop apps.

## Getting Started

First, grab the latest release at https://github.com/jamesqo/Stall/releases.

After you've downloaded the binaries, open up CMD and run

```cmd
cd Downloads
mkdir Stall
move stall.exe Stall
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

Let's try installing an app in real life! As an example, **ILSpy** is one program that doesn't come with an installer. To install, download and unzip the binaries from [their site](http://ilspy.net), and run

```bash
cd ~/Downloads/ILSpy
stall . -e ILSpy.exe -i ILSpy.exe
```

which should install ILSpy.

If you want it to show up nicely in Control Panel, you could alternatively run

```bash
stall . -e ILSpy.exe -i ILSpy.exe --project-url=ilspy.net -p IC#Code --releases-url=github.com/icsharpcode/ILSpy/releases -v 2.3.1
```

which shows up as

![Results](http://i.imgur.com/keyKvRg.png)

## Removing Apps

Have an app, or apps, installed that you'd like to remove? Running

```bash
stall -un names,of,the,apps
```

will uninstall them.

Unfortunately, the names of the apps need to be the ones listed in the Control Panel, so this is less intuitive than it sounds.

## Building the Repo

Prerequisites:
- Git
- VS 2015

First clone the repo:

```bash
git clone https://github.com/jamesqo/Stall.git
```

Then open up **Stall.sln** in Visual Studio, toggle the configuration to **Release**, and build.

Alternatively, if you prefer a command-line interface, open up CMD and run

```cmd
cd Stall
msbuild /p:Configuration=Release
```

Once that's finished, the binaries should be in `Stall/bin/Release`. Follow the steps [mentioned above](#getting-started) and you'll have a fresh copy of Stall installed.

## License

Stall is distributed under the [BSD 2-clause](bsd.license) license.

## FAQ

### What's the difference between Stall and [Squirrel](https://github.com/Squirrel/Squirrel.Windows)?

In short, Squirrel is meant to be a framework for self-updating apps. Stall can install apps without having to modify the code of the app itself, but doesn't manage app updates.

If your app requires a newer version of .NET to be installed (e.g. runs on .NET 4.6 but targets Windows 7), then you should probably use Squirrel as Stall doesn't offer that kind of functionality.

### How does Stall compare to Windows Installer or ClickOnce?

Stall has:

- **no** wizards
- **no** reboots
- **no** UAC dialogs

plus the points mentioned [with Squirrel](#).

### I followed the steps [here](#getting-started), but Stall isn't in my `PATH`.

Usually, opening a new command prompt window should fix this.

### What if my app icon is embedded in the .exe file?

Try running

```bash
stall -e YourApp.exe -i YourApp.exe path/to/YourApp
```

### Where can I contact you?

You can reach me on Twitter at [@jameskodev](https://twitter.com/jameskodev), or [/u/Subtle__](https://www.reddit.com/user/Subtle__/) on Reddit.
