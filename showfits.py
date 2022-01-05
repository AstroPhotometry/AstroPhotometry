import matplotlib.pyplot as plt
from astropy.visualization import astropy_mpl_style

plt.style.use(astropy_mpl_style)

from astropy.io import fits

import sys


# TODO: add -s show only , -c colors
def print_usage(file_name: str) -> None:
    split_filename = file_name.split(sep="\\")
    print(f"usage: {split_filename[-1]} <file> optional: <output file>")


def main(argv: list[str]):
    if len(argv) < 2:
        print_usage(argv[0])
        exit(1)

    first_file = fits.open(argv[1].replace('/', '\\'), mode='readonly')

    first_file.info()

    first_file[0].header

    plt.figure()
    plt.imshow(first_file[0].data, cmap='gray')
    plt.colorbar()
    plt.grid(False)

    if len(argv) > 2:
        plt.savefig(argv[2], format='png', bbox_inches='tight', dpi=80)
    plt.show()

    first_file.close()


if __name__ == "__main__":
    main(sys.argv)