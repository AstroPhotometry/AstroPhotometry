# Python math system

The system uses json file as an input parse it and then decides what to do.

---

The json is this:

```json
{
  "fitsToPNG": [],
  "bias": [],
  "dark": [],
  "flat": [],
  "light": [],
  "outputMasterBias": "",
  "outputMasterDark": "",
  "outputMasterFlat": "",
  "outputCallibrationFile": "",
  "outputCallibratedFolder": "",
  "solve_stars_plate": false
}
```

### Where:

- _fitsToPNG_ has 2 paths in the array: input, output.
- _bias_ is a list of paths to files and folders containing FITS file.
- _dark_ is a list of paths to files and folders containing FITS file.
- _flat_ is a list of paths to files and folders containing FITS file.
- _light_ is a list of paths to files and folders containing FITS file.
- _outputMasterBias_ output folder for the files in case you want it.
- _outputMasterDark_ output folder for the files in case you want it.
- _outputMasterFlat_ output folder for the files in case you want it.
- _outputCallibrationFile_ output folder for the files in case you want it.
- _outputCallibratedFolder_ folder for the finale files.
- _solve_stars_plate_ is if to solve star plate.
