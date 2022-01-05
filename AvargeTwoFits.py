# import matplotlib.pyplot as plt
# from astropy.visualization import astropy_mpl_style
# plt.style.use(astropy_mpl_style)

from astropy.io import fits

import sys


def print_usage(file_name: str) -> None:
    split_filename = file_name.split(sep="\\")
    print(
        f"usage: {split_filename[-1]} <first file> <second file> <output file>"
    )


def main(argv: list[str]):
    if len(argv) < 4:
        print_usage(argv[0])
        exit(1)

    first_file_name: str = argv[1]
    second_file_name: str = argv[2]
    output_file_name: str = argv[3]




if __name__ == "__main__":
    main(sys.argv)