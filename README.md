# New-Vinesauce-ROM-Corruptor
A UI overhaul for [Rikerz's](https://github.com/rikerz) excellent [VRC](https://github.com/Rikerz/VRC).

![image](https://cloud.githubusercontent.com/assets/8846211/16901668/24fb41a2-4c18-11e6-877d-23db18804abb.png)

## Why?
Vinesauce's corruption videos are by far some of the most entertaining video game videos I have watched.  Rikerz's tool is great, but it's a little dated and hasn't been updated in a few years.  This project aims to update the UI and package the corruption code into a nice, neat package.

## What's new?
The original VRC is written using Windows forms, a rather outdated looking technology.  This new repository adds a nicer WPF interface.

## What works?
Currently, NVRC has limited functionality.  The checklist below shows progress.

- [X] Select ROMs/Folders
- [X] Save ROM
- [ ] Overwrite File
- [ ] Select and automatically run Emulator
- [X] Byte corruption using sliders
- [ ] Text corruption
- [ ] Color corruption
- [ ] NES CPU Jam protection option
- [ ] Extended corruption settings
- [X] About page
- [X] Queue (not going to be implemented)
- [ ] Save Settings

## Contributing
Any contributions are welcome.  Please thoroughly document code and changes.  Most features can be completed simply by hooking in a UI element to Rikerz's original backend code.

# Credit & License
The brains behind this corruption tool are the work of [Rikerz](https://github.com/rikerz).  
The improved UI is the work of [piedoom](https://github.com/piedoom).

Any work from the old VRC is licensed under the GNU v3 General Public License.  Any new content is licensed under MIT.
