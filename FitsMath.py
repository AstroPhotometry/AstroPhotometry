import numpy as np
from astropy.io import fits
import sys


# TODO: add stuff like -o to overwrite or list of files
# TODO: return or print the output file name for piping
def print_usage(file_name: str) -> None:
    split_filename = file_name.split(sep="\\")
    print(
        f"""usage: {split_filename[-1]} -operand <first file> <second file> <output file or -s for show>
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
        output_file_name = None
    else:
        output_file_name = argv[-1]
    
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
    first_file_name = argv[-3]
    second_file_name = argv[-2]

    return (show,first_file_name,second_file_name,output_file_name,operator)

def main(argv: list[str]):
    if len(argv) < 5:
        print_usage(argv[0])
        exit(1)

    operand: str = argv[1]
    if len(operand) < 2:
        print_usage(argv[0])
        exit(1)

    (show,first_file_name,second_file_name,output_file_name,operator) = parser(argv)
    # get the file names
    # first_file_name: str = argv[2]
    # second_file_name: str = argv[3]
    # output_file_name: str = argv[4]

    # open the files TODO: check if file exists
    with fits.open(first_file_name, mode='readonly') as first_file, fits.open(
            second_file_name, mode='readonly') as second_file:

        # operator = ""  # the operator to write into the header

        if operator == "avarage":
            out_picture = first_file[0].data[:, :]
            out_picture += second_file[0].data[:, :]
            out_picture = out_picture[:, :] / 2
        elif operator == "minus":
            out_picture = first_file[0].data[:, :]
            out_picture -= second_file[0].data[:, :]
        elif operator == "multiplication":
            out_picture = first_file[0].data[:, :]
            out_picture *= second_file[0].data[:, :]
        elif operator == "addition":
            out_picture = first_file[0].data[:, :]
            out_picture += second_file[0].data[:, :]
        elif operator == "division":  # TODO: check for 0
            out_picture = first_file[0].data[:, :]
            out_picture = out_picture / second_file[0].data[:, :]
        # elif operand[0] == '-' and operand[1:].isdigit(): # unary operand
        #     num: int = int(operand[1:])
        #     if num == 0:
        #         print("ERROR: can not divide by 0")
        #         print_usage(argv[0])
        #         exit(1)
        #     out_picture = first_file[0].data[:, :]
        #     out_picture = out_picture / num
        else:
            first_file.close()
            second_file.close()
            print_usage(argv[0])
            exit(1)

        if show:
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
            import datetime
            date = datetime.datetime.now().strftime("%d/%m/%Y %H:%M")
            hdr['history'] = f"= edited on {date}"
            hdr['COMMENT'] = f"= this is an {operator} of 2 fits files"
            hdu = fits.PrimaryHDU(data=out_picture, header=hdr)

            hdul = fits.HDUList([hdu])
            hdul.writeto(output_file_name, overwrite=True)


if __name__ == "__main__":
    main(sys.argv)
