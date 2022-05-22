## [Short summary](https://www.macobservatory.com/blog/2021/4/20/what-are-astrophotography-calibration-frames-and-why-should-i-use-them)
Bias - bias noise signal - frame is an image taken while the camera is covered

Flat Dark - substitute for bias frames on cameras that require them - These frames are taken with the camera covered like a bias frame, but instead of a short exposure, you need to take it for the same length of time that you exposed your flat frames

Dark - frame showing amp glow and heated pixels on the camera sensor - frames are meant to correct out heat noise

Flat -  shows some reflective mottling that appears on the sensor - to correct for minor vignette, sensor mottling (if any), as well as dust and other stray particles

----

Light - uncalibrated light frame - not calibration but to be calibrated

-------
## Algorithm
The calibration process goes like this:

1. You integrate (stack) the bias frames to create a master bias.

1. You subtract the master bias from the dark frames.

1. You integrate (stack) the bias subtracted dark frames to create a master dark.

1. You subtract the master bias from the flat frames.  Since they're short exposure, there's no need to subtract the long exposure master dark.

1. You integrate (stack) the bias subtracted flat frames to create a master flat.

1. You subtract the master bias and the master dark from the light frames.  You divide the result by the master flat.

1. (optional) You integrate (stack) the calibrated lights.

----

## Math
1. (bias) avg = (master bias) - MEDIAN averaging --  https://www.statisticshowto.com/probability-and-statistics/statistics-definitions/mean-median-mode/

1. ((dark) - (master bias)) avg = master dark - MEDIAN averaging

1. ((flat) - ((((master dark) * (flat time)) / (dark time)) + (master bias))) avg = master flat - MEDIAN averaging

1. ((light) - ((master bias) + (((master dark) * (light time)) / (dark time)))) / (master flat) -> calibrated frames

/ -> divide by will look - 200/0 -> None