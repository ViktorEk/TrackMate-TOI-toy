# TrackMate TOI toy (TTT)
## What is this?
A simple helper to manually allocate tracks-of-interest (TOIs) when using the ImageJ/Fiji TrackMate plugin. Your TOIs can then be used to differ between a specific set of tracks and the rest of the population in later analysis.

##   Guide
###   How to use
1. Do your image analysis in ImageJ/Fiji, and track your particles using TrackMate. Export your track and spot data in ImageJ, and save this window open. Select the "Display spot names" checkbox -- this will show the ID of each spot, which we will use. 
2. Open TTT. Choose an output 'working folder' in the first pop-up window, so that the program knows where to put the exported .csv files it generates.
3. Manually write the ID number ("name") of your chosen population of spots as well as any IDs you would like to ignore in any downstream processing into the two textboxes on top.
4. Choose your output settings. Note that several of the settings are only there to help generate several output files, e.g. the auto-incrementation of file names.
5. Press export. The program will create a new .csv file in the working directory consisting of three rows: (i) your TOIs, comma-separated, (ii) your IDs to ignore, comma-separated, (iii) and an empty row to finish of the file.
6. Pair the spot IDs chosen to their relative tracks using the "Track ID" column of the spot data you exported from TrackMate and the output file of TTT. Now you have your TOIs!

### Note
The program automatically saves most settings, and you can continue where you left off the next time you open up. 
