import argparse
import matplotlib.pyplot as plt
from astropy.visualization import astropy_mpl_style
from astropy.io import fits
import StopInCaseOfError


def argument_handling():
    """
    Method to deal with arguments parsing
    :return: file path to fits file and path to a new png file
    """
    parser = argparse.ArgumentParser()
    parser.add_argument('-f', required=True, type=str, help='Insert path file to the fits file')
    parser.add_argument('-o', required=True, type=str,
                        help='Insert location you want to save the file')
    args = parser.parse_args()
    return args.f, args.o


def make_png():
    """
    Function to make a PNG file from fit file
    """
    fits_file, png_loc = argument_handling()
    first_file = fits.open(fits_file.replace('/', '\\'), mode='readonly')
    plt.imsave(png_loc, first_file[0].data)
    first_file.close()


if __name__ == "__main__":
    plt.style.use(astropy_mpl_style)
    make_png()
    #ex = os.listdir(os.path.abspath('../'))
    #raise Exception(ex)
