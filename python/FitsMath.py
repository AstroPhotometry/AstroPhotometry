import sys
import os
import argparse
import datetime
import StopInCaseOfError
from astropy.io import fits


def eprint(*args, **kwargs):
    print(*args, file=sys.stderr, **kwargs)


def argument_handling():
    """
    Method to deal with arguments parsing
    :return: file path to fits file and path to a new png file
    """
    parser = argparse.ArgumentParser()
    parser.add_argument('-o', help='Overwrite the existing files', action='store_true')
    parser.add_argument('-folder', type=str,
                        help='Insert a folder path with multiple files that we compute on', required=True)
    parser.add_argument('-f', type=str,
                        help='Filepath to output the outcome', required=True)
    group = parser.add_mutually_exclusive_group(required=True)
    group.add_argument('-A', help='Compute average', action='store_true')
    group.add_argument('-a', help='Compute addition', action='store_true')
    group.add_argument('-M', help='Compute multiplication', action='store_true')
    group.add_argument('-m', help='Compute minus', action='store_true')
    group.add_argument('-d', help='Compute division', action='store_true')
    args = parser.parse_args()
    return args


def get_filenames_from_folder(folder_path):
    if os.path.exists(folder_path) and os.path.isdir(folder_path):
        only_files = [file for file in os.listdir(folder_path) if os.path.isfile(os.path.join(folder_path, file))]
        # TODO: check that is a fit file
        for file in only_files:
            if file.endswith('.fit') is False:
                eprint('File in folder is not in fit format')
                sys.exit(1)
        only_files = [(folder_path + '\\' + file) for file in only_files]
        return only_files
    eprint('Folder is not exist or its not a directory')
    sys.exit(1)


def compute_process():
    args = argument_handling()
    folder_path, output_file_path, overwrite_flag = args.folder, args.f, args.o
    input_files = get_filenames_from_folder(folder_path)
    files_amount = len(input_files)
    if files_amount == 0:
        eprint("ERROR: no input file detected")
        sys.exit(1)

    with fits.open(input_files[0], mode='readonly') as base_file:
        out_picture = base_file[0].data[:, :]
    for input_file in input_files[1:]:
        with fits.open(input_file, mode='readonly') as next_file:
            # Average
            if args.A is True:
                out_picture += next_file[0].data[:, :]
            # Minus
            elif args.m is True:
                out_picture = base_file[0].data[:, :]
                out_picture -= next_file[0].data[:, :]
            # Multiplication
            elif args.M is True:
                out_picture *= next_file[0].data[:, :]
            # Addition
            elif args.a is True:
                out_picture = base_file[0].data[:, :]
                out_picture += next_file[0].data[:, :]
            # Division
            elif args.d is True:
                if next_file[0].data[:, :] == 0:
                    eprint('Division by 0')
                    sys.exit(1)
                out_picture = base_file[0].data[:, :]
                out_picture = out_picture / next_file[0].data[:, :]
            else:
                sys.exit(1)

    if args.A is True:
        out_picture = out_picture[:, :] / files_amount

    hdr = fits.Header()
    date = datetime.datetime.now().strftime("%d/%m/%Y %H:%M")
    hdr['history'] = f"= edited on {date}"
    hdu = fits.PrimaryHDU(data=out_picture, header=hdr)
    hdul = fits.HDUList([hdu])
    hdul.writeto(output_file_path, overwrite=overwrite_flag)  # check for errors
    print(output_file_path)


if __name__ == "__main__":
    compute_process()