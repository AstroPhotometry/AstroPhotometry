# import matplotlib.pyplot as plt
# from astropy.visualization import astropy_mpl_style
# plt.style.use(astropy_mpl_style)

from astropy.io import fits
import sys


# TODO: add stuff like -o to overwrite or list of files, -s show after
# TODO: return or print the output file name for piping
def print_usage(file_name: str) -> None:
    split_filename = file_name.split(sep="\\")
    print(
        f"""usage: {split_filename[-1]} -operand <first file> <second file> <output file or -s for show>
        operand can be:
        -m      minus
        -a      avarage
        -M      multiplication
        -<num>  divide by num
        """)


def main(argv: list[str]):
    if len(argv) < 5:
        print_usage(argv[0])
        exit(1)

    operand: str = argv[1]
    if len(operand) < 2:
        print_usage(argv[0])
        exit(1)

    # get the file names
    first_file_name: str = argv[2]
    second_file_name: str = argv[3]
    output_file_name: str = argv[4]

    # open the files TODO: check if file exists
    first_file = fits.open(first_file_name, mode='readonly')
    second_file = fits.open(second_file_name, mode='readonly')

    if operand == "-a":
        out_picture = first_file[0].data[:, :]
        out_picture += second_file[0].data[:, :]
        out_picture = out_picture[:, :] / 2
    elif operand == "-m":
        out_picture = first_file[0].data[:, :]
        out_picture -= second_file[0].data[:, :]
    elif operand == "-M":
        out_picture = first_file[0].data[:, :]
        out_picture *= second_file[0].data[:, :]
    elif operand[0] == '-' and operand[1:].isdigit():
        num: int = int(operand[1:])
        if num == 0:
            print("ERROR: can not divide by 0")
            print_usage(argv[0])
            exit(1)
        out_picture = first_file[0].data[:, :]
        out_picture = out_picture/num
    else:
        first_file.close()
        second_file.close()
        print_usage(argv[0])
        exit(1)

    if output_file_name == "-s":
        import matplotlib.pyplot as plt
        from astropy.visualization import astropy_mpl_style
        plt.style.use(astropy_mpl_style)
        plt.figure()
        plt.grid(False)
        plt.imshow(out_picture, cmap='gray')
        plt.colorbar()
        plt.show()
    else:
        hdr = fits.Header()
        hdr['testing'] = 'first edition'
        hdr['COMMENT'] = "this is an avarage of 2 fits files"
        # empty_primary = fits.PrimaryHDU(header=hdr) # to make with only header
        hdu = fits.PrimaryHDU(data=out_picture, header=hdr)

        hdul = fits.HDUList([hdu])
        hdul.writeto(output_file_name, overwrite=True)

    first_file.close()
    second_file.close()


if __name__ == "__main__":
    main(sys.argv)