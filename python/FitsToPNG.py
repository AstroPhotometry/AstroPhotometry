import os
import sys
import matplotlib.pyplot as plt
from datetime import datetime
from astropy.visualization import astropy_mpl_style
from astropy.io import fits
from ProgressPrint import Progress
import numpy as np

progress = Progress(module_name="FtsMath", stages=1)


# def show_exception_and_exit(exc_type, exc_value, tb):
#     import traceback
#     error = ""
#     for e in traceback.format_exception(exc_type, exc_value, tb):
#         error += e
#         error += '\n'
#     progress.eprint(error)
#     sys.exit(-1)


# sys.excepthook = show_exception_and_exit


def name_the_file(time, mode = "linear"):
    date_string = time
    date_string += f'_{mode}'
    date_string += '.png'
    return date_string


def validate_file(file_path: str):
    if os.path.exists(file_path) is False:
        progress.eprint('file is not exist: ' + file_path)
        sys.exit(1)


def make_png(fits_file, png_loc):
    """
    Function to make a PNG file from fit file
    """
    progress = Progress("FIT to PNG", 6)
    progress.cprint("started")
    progress.cprint("opening fit file")
    first_file = fits.open(fits_file.replace('/', '\\'), mode='readonly')

    # Check if file is empty
    image_pos = 0
    try:
        _ = first_file[0].data
        for i in range(len(first_file)):
            if first_file[i].data is not None:
                image_pos = i
                break
    except Exception as e:
        progress.eprint("file is empty: " + e.__str__())
        sys.exit(1)
    try:
        now = datetime.now().strftime('%m-%d-%Y-%H-%M-%S')
        progress.cprint("saving linear PNG")
        plt.imsave(png_loc + name_the_file(now, "linear"), first_file[image_pos].data)

        progress.cprint("saving logarithm PNG")
        plt.imsave(png_loc + name_the_file(now, "logarithm"), np.log1p(np.array(first_file[image_pos].data)))
        
        progress.cprint("saving exponential PNG")
        plt.imsave(png_loc + name_the_file(now, "exponential"), np.exp(first_file[image_pos].data))

    except Exception as e:
        progress.eprint("saving PNG has failed " + e.__str__())
        sys.exit(1)
    first_file.close()
    progress.cprint("done, saved in " + png_loc)


def main_run(file: str, png_loc: str):
    validate_file(file)
    plt.style.use(astropy_mpl_style)
    make_png(file, png_loc)
