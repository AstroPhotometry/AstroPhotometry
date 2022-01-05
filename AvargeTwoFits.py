# import matplotlib.pyplot as plt
# from astropy.visualization import astropy_mpl_style
# plt.style.use(astropy_mpl_style)

from astropy.io import fits

import sys

# TODO: add stuff like -o to overwrite or list of files


def print_usage(file_name: str) -> None:
    split_filename = file_name.split(sep="\\")
    print(
        f"usage: {split_filename[-1]} <first file> <second file> <output file>"
    )


def main(argv: list[str]):
    if len(argv) < 4:
        print_usage(argv[0])
        exit(1)

    # get the file names
    first_file_name: str = argv[1]
    second_file_name: str = argv[2]
    output_file_name: str = argv[3]

    # open the files TODO: check if file exists
    first_file = fits.open(first_file_name, mode='readonly')
    second_file = fits.open(second_file_name, mode='readonly')

    out_picture = first_file[0].data[:, :]
    out_picture += second_file[0].data[:, :]
    out_picture = out_picture[:, :] / 2

    hdr = fits.Header()
    hdr['testing'] = 'first edition'
    hdr['COMMENT'] = "this is an avarage of 2 fits files"
    # empty_primary = fits.PrimaryHDU(header=hdr) # to make with only header
    hdu = fits.PrimaryHDU(data=out_picture, header=hdr)

    hdul = fits.HDUList([hdu])
    hdul.writeto(output_file_name, overwrite=True)


if __name__ == "__main__":
    main(sys.argv)