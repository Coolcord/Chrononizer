Chrononizer
===========

Music synchronization and library management application. Intended for use with a Cowon X7.


-==========================- About Chrononizer -==========================-
 
 Chrononizer is a simple music synchronization application that I designed 
 for myself to make my daily updates to my music library much easier. The
 goal of this project was to make it possible to quickly synchronize my
 music library between multiple devices in as few clicks as possible, while
 following unique rules for each device.
 
 The name Chrononizer is a mash-up of the words "chrono-boost" and
 "synchronizer." Considering that the goal of this application was to
 quickly synchronize multiple devices, it seemed appropriate.
 
 I was inspired to write Chrononizer after I purchased a Cowon X7 portable
 media player. I wanted a quick application that could synchronize my
 library to the device, however, I could not find anything that accomplished
 what I wanted up to my liking. In order for such an application to meet my
 needs, it had to synchronize my music library from my desktop computer to
 both my laptop and PMP, following unique rules for each device.
 
 For the PMP, no chiptune files could be sent to the device. Also, any FLAC
 audio files that are over 24-bit in bit-depth or 48khz in bit-rate must be
 downscaled accordingly. The laptop could be sent chiptunes and FLAC audio
 files with high bit-depths and bit-rates, but it has no use for downscaled
 files. These are the rules that Chrononizer is programmed to follow.
 
 To determine if any files in the music library need to be downscaled, the files
 of the library need to be analyzed. This is where the "Scan Library" function
 comes into play. With this feature, statistics about the user's library can be
 determined. FLAC files will be analyzed during this process to see if they are
 higher than 24-bit in bit-depth and higher than 48khz in bit-rate. If they are,
 they will be added to the list of files that need downscaling. Using this list,
 users can quickly find these files so that they can make a downscaled version
 of them if they desire.
 
 In the program's current state, Chrononizer meets my personal needs. However,
 it is likely that other users may prefer to use different synchronizing rules
 or may wish to add new features. This is why I intend to keep Chrononizer open
 source. I hope that other users out there may be able to use this code as a base
 to create a music synchronizer for their own personal use that meets their needs.

 If you wish to contact me about the application, or anything of the like,
 feel free to send me an email at coolcord24@gmail.com
