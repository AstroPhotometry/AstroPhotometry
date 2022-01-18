import numpy as np
from astropy.io import fits
import sys


def eprint(*args, **kwargs):
    print(*args, file=sys.stderr, **kwargs)


def get_astropy_mpl_style(
):  # the setting for the graph that show on the screen
    return {
        # Lines
        'lines.linewidth': 1.7,
        'lines.antialiased': True,

        # Patches
        'patch.linewidth': 1.0,
        'patch.facecolor': '#348ABD',
        'patch.edgecolor': '#CCCCCC',
        'patch.antialiased': True,

        # Images
        'image.cmap': 'gist_heat',
        'image.origin': 'upper',

        # Font
        'font.size': 12.0,

        # Axes
        'axes.facecolor': '#FFFFFF',
        'axes.edgecolor': '#AAAAAA',
        'axes.linewidth': 1.0,
        # 'axes.grid': True, # making an WARNING
        'axes.titlesize': 'x-large',
        'axes.labelsize': 'large',
        'axes.labelcolor': 'k',
        'axes.axisbelow': True,

        # Ticks
        'xtick.major.size': 0,
        'xtick.minor.size': 0,
        'xtick.major.pad': 6,
        'xtick.minor.pad': 6,
        'xtick.color': '#565656',
        'xtick.direction': 'in',
        'ytick.major.size': 0,
        'ytick.minor.size': 0,
        'ytick.major.pad': 6,
        'ytick.minor.pad': 6,
        'ytick.color': '#565656',
        'ytick.direction': 'in',

        # Legend
        'legend.fancybox': True,
        'legend.loc': 'best',

        # Figure
        'figure.figsize': [8, 6],
        'figure.facecolor': '1.0',
        'figure.edgecolor': '0.50',
        'figure.subplot.hspace': 0.5,

        # Other
        'savefig.dpi': 72,
    }


# TODO: add stuff like -o to overwrite or list of files
# TODO: return or print the output file name for piping
def print_usage(file_name: str) -> None:
    split_filename = file_name.split(sep="\\")
    eprint(
        f"""usage: {split_filename[-1]} -operand [file names...] <output file or -s for show>
        operand can be:
        -A      avarage
        -a      addition
        -m      minus
        -M      multiplication
        -d      division
        """)
    # -<num>  divide by num #unary operand


def parser(argv: list[str]):
    show = "-s" in argv
    if show:
        argv.remove("-s")
        output_file_name = None
    else:
        output_file_name = argv[-1]
        argv.remove(output_file_name)

    if "-A" in argv:
        argv.remove("-A")
        operator = "avarage"
    elif "-a" in argv:
        argv.remove("-a")
        operator = "addition"
    elif "-m" in argv:
        argv.remove("-m")
        operator = "minus"
    elif "-M" in argv:
        argv.remove("-M")
        operator = "multiplication"
    elif "-d" in argv:
        argv.remove("-d")
        operator = "division"
    else:
        print_usage(argv[0])
        exit(1)

    # TODO: check if they are paths or exist or unary
    input_files = []
    for arg in argv[1:]:
        input_files.append(arg)
    return (show, input_files, output_file_name, operator)


def main(argv: list[str]):
    if len(argv) < 5:
        print_usage(argv[0])
        exit(1)

    operand: str = argv[1]
    if len(operand) < 2:
        print_usage(argv[0])
        exit(1)

    (show, input_files, output_file_name, operator) = parser(argv)

    if len(input_files) == 0:
        eprint("ERROR: no input file detected")
        print_usage(argv[0])
        exit(1)

    out_picture = []
    # open the files TODO: check if file exists
    with fits.open(input_files[0], mode='readonly') as base_file:
        out_picture = base_file[0].data[:, :]

    average: int = len(input_files)
    for input_file in input_files[1:]:
        with fits.open(input_file, mode='readonly') as next_file:
            if operator == "avarage":
                out_picture += next_file[0].data[:, :]
            elif operator == "minus":
                out_picture = base_file[0].data[:, :]
                out_picture -= next_file[0].data[:, :]
            elif operator == "multiplication":
                out_picture *= next_file[0].data[:, :]
            elif operator == "addition":
                out_picture = base_file[0].data[:, :]
                out_picture += next_file[0].data[:, :]
            elif operator == "division":  # TODO: check for 0
                out_picture = base_file[0].data[:, :]
                out_picture = out_picture / next_file[0].data[:, :]
            else:
                print_usage(argv[0])
                exit(1)

    if operator == "avarage":
        out_picture = out_picture[:, :] / average

    # elif operand[0] == '-' and operand[1:].isdigit(): # unary operand
    #     num: int = int(operand[1:])
    #     if num == 0:
    #         print("ERROR: can not divide by 0")
    #         print_usage(argv[0])
    #         exit(1)
    #     out_picture = first_file[0].data[:, :]
    #     out_picture = out_picture / num

    if show:
        import matplotlib.pyplot as plt
        plt.style.use(get_astropy_mpl_style())
        plt.figure()
        plt.imshow(out_picture, cmap='gray')
        plt.grid(False)
        plt.colorbar()
        plt.show()

    else:
        hdr = fits.Header()
        import datetime
        date = datetime.datetime.now().strftime("%d/%m/%Y %H:%M")
        hdr['history'] = f"= edited on {date}"
        hdr['COMMENT'] = f"= this is an {operator} of 2 fits files"
        hdu = fits.PrimaryHDU(data=out_picture, header=hdr)

        hdul = fits.HDUList([hdu])
        hdul.writeto(output_file_name, overwrite=True)
        print(output_file_name)


if __name__ == "__main__":
    main(sys.argv)
