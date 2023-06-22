# PrintToSpool

A simple utility to send a (raw) print file directly to a printer on Windows.

## Setting up

1. Compile the program (not `Build` but `Publish`). Building should work, too, but only Publish is able to bundle it into a single executable in the current .NET era.
Find the single `.exe` executable in the `bin\Release\net6.0-windows\publish` folder, you can ignore the `.pdb` file.

2. Copy the executable to wherever you please.

3. Open _File Explorer_, type `shell:sendTo` into the address bar. Create a shortcut here (`context menu > New > Shortcut`), point it to the exe (or, alternatively, create a shortcut
to the Desktop and drag-and-drop it here). Practically, name it as your printer. Go back into _Properties,_ select an icon you like (an obvious choice would be the printer icon
from `SHELL32.dll` but do as you please). Make sure your target is:

```
    "\path-to-the-executable\PrintToSpool.exe" "your-printer's-exact-name"
```

One argument, not two, the print file will be added automatically by Windows. Make sure you type the printer's name exactly as visible in `Settings > Printers & Scanners`.

4. To test, find a print file, right click for the context menu, `Send To > your new printer shortcut` and it should appear from the printer all right. If it doesn't, debug it,
you have the source. :-)

## Note

I saw another solution here and there on the web that suggests to copy the already existing printer shortcut from `shell:PrintersFolder` to `shell:sendTo`. Unfortunately,
that doesn't work, it will still expect document files (eg. a `.txt`) to process, not a raw print file. That's why I decided to write this one...

Enjoy it.

_Gábor DEÁK JAHN<br>
deakjahn@gmail.com_
