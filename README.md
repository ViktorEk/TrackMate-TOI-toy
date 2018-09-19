# TrackMate TOI toy
## What is this?
A helper to manually allocate tracks-of-interest (TOIs) when using the ImageJ/Fiji TrackMate plugin. Your TOIs can then be used to differ between a specific set of tracks and the rest of the population in later analysis.

##   How to use
1. Choose an output 'working folder' in the first pop-up window, so that the program knows where to put the exported .csv files it generates.
2. Write the ID number of your tracks-of-interest (TOIs) as well as any IDs you would like to ignore in any downstream processing into the two textboxes on top.
3. Choose your output settings. Note that several of the settings are only there to help generate several output files, e.g. the auto-incrementation of file names.
4. Press export. The program will create a new .csv file in the working directory consisting of three rows: (i) your TOIs, comma-separated, (ii) your IDs to ignore, comma-separate, (iii) and an empty row to finish of the file.
5. Export your track and spot data in ImageJ. 
