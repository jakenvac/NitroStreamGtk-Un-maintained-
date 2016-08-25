# Nitro Stream Gtk#
A Mono/Gtk# port of NitroStream.
Currently in an alpha phase. **should** match the Windows/WPF version for features (If not in Asthetic)

>Allows the user to easily initiate a connection to NTRViewer for NTR CFW on the Nintendo 3DS
Built on the NTRDebugger by Cell99 - https://github.com/44670/NTRClient

![NitroStream Gtk# Image](https://i.imgur.com/mCZFZdq.png)

## Dependencies
You will need:
* NTRViewer - which can be found in the starter pack here:
https://github.com/44670/BootNTR/releases
* Gtk#
* Mono / .Net 4.5
* ***Non Windows users:*** Even though NitroStream is native, you will need wine to launch NTRViewer
and it will need to be the default program for launching exe files. I will look into executing NTRViewer with wine manually.

## Usage
To use, either place the executable in the same directory as NTViewer.exe or set it's location before connecting in the options menu of the application.

## License
GPLV2