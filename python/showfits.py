import matplotlib.pyplot as plt
from astropy.visualization import astropy_mpl_style

plt.style.use(astropy_mpl_style)

from astropy.io import fits

import sys


def eprint(*args, **kwargs):
    print(*args, file=sys.stderr, **kwargs)

# TODO: add -s show only , -c colors
def print_usage(file_name: str) -> None:
    split_filename = file_name.split(sep="\\")
    eprint(f"usage: {split_filename[-1]} <file or -p for piping> optional: <png output file>")


def main(argv: list[str]):
    if len(argv) < 2:
        print_usage(argv[0])
        exit(1)

    if "-p" in argv:
        argv.remove("-p")
        input_file = input()
        input_file = input_file.removeprefix("\ufeff")
        if len(input_file) == 0:
            eprint("PIPING ERROR: did not received file name")
            exit(1)
    else:
        input_file = argv[-1]

    first_file = fits.open(input_file.replace('/', '\\'), mode='readonly')

    first_file.info()

    first_file[0].header

    plt.figure()
    plt.imshow(first_file[0].data, cmap='gray')
    plt.colorbar()
    plt.grid(False)

    if len(argv) > 1:
        plt.savefig(argv[2], format='png', bbox_inches='tight', dpi=80)
    plt.show()

    first_file.close()


if __name__ == "__main__":
    main(sys.argv)