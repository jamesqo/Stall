using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stall
{
    public static class Constants
    {
        public const int EnglishLocale = 0x0409;
        public const string UninstallKey = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

        public const string Usage = @"stall is a command-line tool to manage your Windows desktop apps.

Options:
    -?, -h, --help           Show help and exit
    --add-path               Add your program to $PATH
    -a, --args=<args>        Pass <args> to your app from its shortcut (TODO)
    -c, --config=<file>      Configure options from <file> (TODO)
    -e, --executable=<file>  <file> is the app's program
    -i, --icon=<file>        <file> is the app's icon
    -n, --name=<name>        Specify the app's name
    --overwrite              Overwrite existing folders/keys if present
    --project-url=<url>      Set <url> as the project's homepage URL
    -p, --publisher=<name>   Specify publisher's name
    --releases-url=<url>     Releases for your app can be found at <url>
    -s, --script             ONLY add your program to $PATH (implies --add-path)
    -un <csv>                Uninstall the apps specified by <csv>
    -v, --version=<ver>      Set app version to <ver>

Usage:
    stall -e program [-i icon] [options] folder
    Installs an app to your computer.

    Parameters:
        <folder> is copied to AppData\Local. It should contain all of your app's important files.
        <program> is your app's executable. It will be called when the shortcut is clicked.
        <icon> is your app's icon file. It will be displayed on shortcuts.

    Example:
        stall -e App.exe -i icon.ico path/to/MyApp

        You do not have to specify the full path to <program> and <icon>; they are relative to the root folder (MyApp) by default.
        See below for more.

    Remarks:
        - JPGs and PNGs are NOT accepted at the moment; only ICOs and EXEs.

        - Your app folder should not contain a file named uninstall.cmd at its root. If present, it will be overwritten.

        - By default, the paths for <program> and <icon> are relative to the root folder.
        To override this behavior, prefix the paths with ':':

        path=path/to/MyApp
        stall -e :$path/App.exe -i :$path/icon.ico $path

        This resolves paths relative to the current directory.
        In this example, the shortcut will point to the files in the original folder.
        Absolute paths may only be used with ':', so this is also useful if you want to reference a fixed location on disk.

Removing apps:
    stall -un <apps>
    Removes <apps> from your computer.

    Parameters:
        <apps> is a CSV-style list of apps to remove.

    Example:
        theplague=""Java SE Development Kit 7 Update 55""
        stall -un ""$theplague,Xamarin""

    Remarks:
        - Removing any app from your computer is supported, even ones we didn't install.

        - <apps> is whitespace and case insensitive; "" NOde.JS "" has the same effect as ""Node.js"".";
    }
}
